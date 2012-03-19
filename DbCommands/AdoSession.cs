using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using Ninject;

namespace Adotic
{
    public class AdoSession : IAdoSession
    {
        private readonly Func<DbConnection> _createConnection;

        [Inject] private GetPropertyInfo GetPropertyInfo { get; set; }

        public AdoSession(Func<DbConnection> createConnection)
        {
            _createConnection = createConnection;
        }

        public virtual void Execute(Action<IDbCommand> assignParametersOnCmd)
        {
            IEnumerable<KeyValuePair<string, object>> parameters;
            Execute(assignParametersOnCmd, out parameters);
        }

        public virtual void Execute(Action<IDbCommand> assignParametersOnCmd, out IEnumerable<KeyValuePair<string, object>> parameters)
        {
            using (var connection = _createConnection())
            using (var cmd = connection.CreateCommand())
            {
                if (!connection.IsOpen()) connection.Open();
                cmd.CommandTimeout = 600;
                cmd.Connection = connection;
                assignParametersOnCmd(cmd);
                cmd.ExecuteNonQuery();

                parameters = cmd.Parameters.Cast<IDataParameter>()
                    .Where(p => p.IsAnOutputParameter())
                    .Select(p => new KeyValuePair<string, object>(p.ParameterName, p.Value));
            }
        }

        public virtual void Execute<TQuery>(TQuery query) where TQuery : class
        {
            Action<IDbCommand> parameterizedCmd = c =>
            {
                c.CommandText = query.ToString();
                c.CommandType = CommandType.StoredProcedure;
                c.Parameters.Add(PropertyEvaluator, query);
            };

            IEnumerable<KeyValuePair<string, object>> outputParameters;

            Execute(parameterizedCmd, out outputParameters);

            var properties = PropertyEvaluator(typeof(TQuery));

            outputParameters.ForEach(p => properties.First(i => i.Name == p.Key).SetValue(query, p.Value, null));
        }

        private Func<Type, PropertyInfo[]> PropertyEvaluator
        {
            get { return type => GetPropertyInfo.For(type).ToArray(); }
        }
    }

    public class AdoSession<TEntity> : IAdoSession<TEntity> where TEntity : class, new()
    {
        private readonly Func<DbConnection> _createConnection;

        [Inject] private IDataReaderMapper<TEntity> EntityMapper { get; set; }
        [Inject] private GetPropertyInfo GetPropertyInfo { get; set; }
        
        public AdoSession(Func<DbConnection> createConnection)
        {
            _createConnection = createConnection;
        }

        public virtual IQueryable<TEntity> Execute(Action<IDbCommand> assignParametersOnCmd)
        {
            IEnumerable<KeyValuePair<string, object>> parameters;
            return Execute(assignParametersOnCmd, out parameters);
        }

        public virtual IQueryable<TEntity> Execute(Action<IDbCommand> assignParametersOnCmd, out IEnumerable<KeyValuePair<string, object>> parameters)
        {
            ICollection<TEntity> results;

            using (var connection = _createConnection())
            using (var cmd = connection.CreateCommand())
            {
                if (!connection.IsOpen()) connection.Open();
                cmd.CommandTimeout = 600;
                cmd.Connection = connection;
                assignParametersOnCmd(cmd);
                using (var reader = cmd.ExecuteReader())
                {
                    results = new List<TEntity>();
                    while(reader.Read())
                        results.Add(EntityMapper.MapFrom(reader));
                }

                parameters = cmd.Parameters.Cast<IDataParameter>()
                    .Where(p => p.IsAnOutputParameter())
                    .Select(p => new KeyValuePair<string, object>(p.ParameterName, p.Value));
            }

            return results.AsQueryable();
        }

        public virtual IQueryable<TEntity> Execute<TQuery>(TQuery query) where TQuery : class
        {
            Action<IDbCommand> parameterizedCmd = c =>
            {
                c.CommandText = query.ToString();
                c.CommandType = CommandType.StoredProcedure;
                c.Parameters.Add(PropertyEvaluator, query);
            };

            IEnumerable<KeyValuePair<string, object>> outputParameters;

            var entities = Execute(parameterizedCmd, out outputParameters);

            var properties = PropertyEvaluator(typeof(TQuery));

            outputParameters.ForEach(p => properties.First(i => i.Name == p.Key).SetValue(query, p.Value, null));

            return entities;
        }

        private Func<Type, PropertyInfo[]> PropertyEvaluator
        {
            get { return type => GetPropertyInfo.For(type).ToArray(); }
        }
    }
}
