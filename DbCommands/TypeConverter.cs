using System;
using System.Collections.Generic;
using System.Data;

namespace Adotic
{
    internal static class TypeConverter
    {
        private static readonly Dictionary<Type, DbType> TypeToDbType = new Dictionary<Type, DbType>
                                                                            {
                                                                                { typeof(string), DbType.String },
                                                                                { typeof(DateTime), DbType.DateTime },
                                                                                { typeof(DateTime?), DbType.DateTime },
                                                                                { typeof(int), DbType.Int32 },
                                                                                { typeof(int?), DbType.Int32 },
                                                                                { typeof(long), DbType.Int64 },
                                                                                { typeof(long?), DbType.Int64 },
                                                                                { typeof(bool), DbType.Boolean },
                                                                                { typeof(bool?), DbType.Boolean },
                                                                                { typeof(byte[]), DbType.Binary },
                                                                                { typeof(decimal), DbType.Decimal },
                                                                                { typeof(decimal?), DbType.Decimal },
                                                                                { typeof(double), DbType.Double },
                                                                                { typeof(double?), DbType.Double },
                                                                                { typeof(float), DbType.Single },
                                                                                { typeof(float?), DbType.Single },
                                                                                { typeof(Guid), DbType.Guid },
                                                                                { typeof(Guid?), DbType.Guid }, 
                                                                                { typeof(Enum), DbType.Int32 },
                                                                                { typeof(DataTable), DbType.Object},
                                                                                { typeof(DateTimeOffset), DbType.DateTimeOffset},
                                                                                { typeof(DateTimeOffset?), DbType.DateTimeOffset}
                                                                            };

        public static DbType ToDbType(Type type)
        {
            if (!TypeToDbType.ContainsKey(type))
            {
                throw new InvalidOperationException("Type {0} doesn't have a matching DbType configured".For(type.FullName));
            }

            return TypeToDbType[type];
        }
    }
}