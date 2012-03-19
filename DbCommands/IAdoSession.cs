using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Adotic
{
    public interface IAdoSession
    {
        void Execute(Action<IDbCommand> assignParametersOnCmd);
        void Execute(Action<IDbCommand> assignParametersOnCmd, out IEnumerable<KeyValuePair<string, object>> parameters);
        void Execute<TQuery>(TQuery query) where TQuery : class;
    }

    public interface IAdoSession<out TEntity> where TEntity : class, new()
    {
        IQueryable<TEntity> Execute(Action<IDbCommand> assignParametersOnCmd);
        IQueryable<TEntity> Execute(Action<IDbCommand> assignParametersOnCmd, out IEnumerable<KeyValuePair<string, object>> parameters);
        IQueryable<TEntity> Execute<TQuery>(TQuery query) where TQuery : class;
    }
}
