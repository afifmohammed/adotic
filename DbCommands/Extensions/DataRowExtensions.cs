using System;
using System.Data;
using System.Linq.Expressions;

namespace Adotic
{
    internal static class DataRowExtensions
    {
        public static TLiteral GetValueAs<TLiteral>(this DataRow row, string column)
        {
            return (row.Table.Columns.Contains(column) && !row.IsNull(column))
                       ? (TLiteral)row[column]
                       : default(TLiteral);
        }

        public static object GetValueFor(this DataRow row, string column)
        {
            return (row.Table.Columns.Contains(column) && !row.IsNull(column))
                       ? row[column]
                       : null;
        }

        public static TLiteral GetValueAs<TLiteral, TInterface>(this DataRow row, Expression<Func<TInterface, object>> property)
        {
            return row.GetValueAs<TLiteral>(property.Name);
        }

        public static bool GetValueFor<TInterface>(this DataRow row, Expression<Func<TInterface, bool>> property)
        {
            var column = property.Name;
            return (row.Table.Columns.Contains(column) && !row.IsNull(column))
                       ? Convert.ToBoolean(row[column])
                       : default(bool);
        }

        public static bool? GetValueFor<TInterface>(this DataRow row, Expression<Func<TInterface, bool?>> property)
        {
            var column = property.Name;
            return (row.Table.Columns.Contains(column) && !row.IsNull(column))
                       ? Convert.ToBoolean(row[column])
                       : default(bool);
        }

        public static string GetValueFor<TInterface>(this DataRow row, Expression<Func<TInterface, string>> property)
        {
            var column = property.Name;
            return (row.Table.Columns.Contains(column) && !row.IsNull(column))
                       ? Convert.ToString(row[column])
                       : default(string);
        }

        public static int GetValueFor<TInterface>(this DataRow row, Expression<Func<TInterface, int>> property)
        {
            var column = property.Name;
            var columnReturnedWithValue = row.Table.Columns.Contains(column) && !row.IsNull(column);

            if (columnReturnedWithValue)
                return Convert.ToInt32(row[column]);

            return default(int);
        }

        public static int? GetValueFor<TInterface>(this DataRow row, Expression<Func<TInterface, int?>> property)
        {
            var column = property.Name;
            return (row.Table.Columns.Contains(column) && !row.IsNull(column))
                       ? Convert.ToInt32(row[column])
                       : default(int?);
        }

        public static decimal GetValueFor<TInterface>(this DataRow row, Expression<Func<TInterface, decimal>> property)
        {
            var column = property.Name;
            var columnReturnedWithValue = row.Table.Columns.Contains(column) && !row.IsNull(column);

            if (columnReturnedWithValue)
                return Convert.ToDecimal(row[column]);

            return default(decimal);
        }

        public static decimal? GetValueFor<TInterface>(this DataRow row, Expression<Func<TInterface, decimal?>> property)
        {
            var column = property.Name;
            return (row.Table.Columns.Contains(column) && !row.IsNull(column))
                       ? Convert.ToDecimal(row[column])
                       : default(decimal?);
        }


        public static DateTime GetValueFor<TInterface>(this DataRow row, Expression<Func<TInterface, DateTime>> property)
        {
            var column = property.Name;
            return (row.Table.Columns.Contains(column) && !row.IsNull(column))
                       ? Convert.ToDateTime(row[column])
                       : default(DateTime);
        }

        public static DateTime? GetValueFor<TInterface>(this DataRow row, Expression<Func<TInterface, DateTime?>> property)
        {
            var column = property.Name;
            return (row.Table.Columns.Contains(column) && !row.IsNull(column))
                       ? Convert.ToDateTime(row[column])
                       : default(DateTime?);
        }

        public static ulong GetValueFor<TInterface>(this DataRow row, Expression<Func<TInterface, ulong>> property)
        {
            var column = property.Name;

            return (row.Table.Columns.Contains(column) && !row.IsNull(column))
                       ? BitConverter.ToUInt64((byte[])row[column], 0) : default(ulong);
        }
    }
}