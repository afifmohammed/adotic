using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;

namespace Adotic
{
    internal static class DataParameterCollectionExtensions
    {
        public static SqlParameter Add(this IDataParameterCollection parameters, string parameterName, Type parameterType, object parameterValue)
        {
            var param = new SqlParameter(parameterName, parameterValue ?? DBNull.Value) { DbType = TypeConverter.ToDbType(parameterType) };
            if(parameterValue != null)
            {
                if (parameterValue is DataTable)
                {
                    param.SqlDbType = SqlDbType.Structured;
                    param.TypeName = ((DataTable)parameterValue).TableName;
                }
            }
            
            if (parameterName.ToLower().StartsWith("output"))
                param.Direction = ParameterDirection.Output;

            parameters.Add(param);

            return param;
        }
        
        public static IDataParameterCollection Add<TCriteria>(this IDataParameterCollection parameters, Func<Type, PropertyInfo[]> getProperties, TCriteria criteria) 
            where TCriteria : class
        {
            var properties = getProperties(criteria.GetType());
            
            properties.ForEach(p => parameters.Add(p.Name, p.PropertyType, p.GetValue(criteria, null)));
            return parameters;
        }

        public static SqlParameter Add<TInterface, TLiteral>(this IDataParameterCollection parameters, Expression<Func<TInterface, object>> property, object value)
        {
            var parameterName = property.Name;
            var parameterType = typeof (TLiteral);
            
            return parameters.Add(parameterName, parameterType, value);
        }

        public static SqlParameter Add<TInterface>(this IDataParameterCollection parameters, Expression<Func<TInterface, object>> property, DataTable value)
        {
            return parameters.Add<TInterface, string>(property, value).Do(p => { p.SqlDbType = SqlDbType.Structured; p.TypeName = value.TableName; });
        }

        public static SqlParameter Add<TInterface>(this IDataParameterCollection parameters, Expression<Func<TInterface, object>> property, string value)
        {
            return parameters.Add<TInterface, string>(property, value.ToSqlValue()).Do(p => p.SqlDbType = SqlDbType.VarChar);
        }

        public static SqlParameter Add<TInterface>(this IDataParameterCollection parameters, Expression<Func<TInterface, object>> property, int value)
        {
            return parameters.Add<TInterface, int>(property, value.ToSqlValue());
        }

        public static SqlParameter Add<TInterface>(this IDataParameterCollection parameters, Expression<Func<TInterface, object>> property, DateTime value)
        {
            return parameters.Add<TInterface, DateTime>(property, value.ToSqlValue());
        }

        public static SqlParameter Add<TInterface>(this IDataParameterCollection parameters, Expression<Func<TInterface, object>> property, DateTime? value)
        {
            return parameters.Add<TInterface, DateTime?>(property, value.ToSqlValue());
        }

        public static SqlParameter Add<TInterface>(this IDataParameterCollection parameters, Expression<Func<TInterface, object>> property, bool value)
        {
            return parameters.Add<TInterface, bool>(property, value.ToSqlValue());
        }

        public static SqlParameter Add<TInterface>(this IDataParameterCollection parameters, Expression<Func<TInterface, object>> property, bool? value)
        {
            return parameters.Add<TInterface, bool?>(property, value.ToSqlValue());
        }

        public static SqlParameter Add<TInterface>(this IDataParameterCollection parameters, Expression<Func<TInterface, object>> property, int? value)
        {
            return parameters.Add<TInterface, int?>(property, value.ToSqlValue());
        }

        public static SqlParameter Add<TInterface>(this IDataParameterCollection parameters, Expression<Func<TInterface, object>> property, Enum value)
        {
            return parameters.Add<TInterface, Enum>(property, value);
        }

        public static SqlParameter Add<TInterface>(this IDataParameterCollection parameters, Expression<Func<TInterface, object>> property, decimal? value)
        {
            return parameters.Add<TInterface, decimal?>(property, value.ToSqlValue());
        }

        /// <returns>
        /// a paramter of direction as <see cref="ParameterDirection.Output"/> 
        /// and size of 8 if the <paramref name="value"/> is empty 
        /// else with direction as <see cref="ParameterDirection.InputOutput"/> 
        /// and size as the size of the <paramref name="value"/>
        /// </returns>
        public static SqlParameter Add<TInterface>(this IDataParameterCollection parameters, Expression<Func<TInterface, object>> property, byte[] value)
        {
            return parameters.Add<TInterface, byte[]>(property, value)
                .Do(p => p.Direction = value.Length == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput)
                .Do(p => p.Size = value.Length == 0 ? 8 : value.Length);
        }
    }
}