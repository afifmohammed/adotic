using System;
using System.Data.Linq;
using System.Data.SqlTypes;

namespace Adotic
{
    internal static class LiteralExtensions
    {
        public static SqlString ToSqlValue(this string input)
        {
            return string.IsNullOrEmpty(input) ? SqlString.Null : new SqlString(input);
        }

        public static SqlDateTime ToSqlValue(this DateTime input)
        {
            return input == DateTime.MinValue ? SqlDateTime.Null : new SqlDateTime(input);
        }

        public static SqlDateTime ToSqlValue(this DateTime? input)
        {
            return input.HasValue ? new SqlDateTime(input.Value) : SqlDateTime.Null;
        }

        public static SqlInt32 ToSqlValue(this int? input)
        {
            return input.HasValue ? new SqlInt32(input.Value) : SqlInt32.Null;
        }

        public static SqlInt32 ToSqlValue(this int input)
        {
            return input == int.MinValue || input == 0 ? SqlInt32.Null : new SqlInt32(input);
        }

        public static SqlInt64 ToSqlValue(this long? input)
        {
            return input.HasValue ? new SqlInt64(input.Value) : SqlInt64.Null;
        }

        public static SqlInt64 ToSqlValue(this long input)
        {
            return input == int.MinValue || input == 0 ? SqlInt64.Null : new SqlInt64(input);
        }

        public static SqlBoolean ToSqlValue(this bool input)
        {
            return new SqlBoolean(input);
        }

        public static SqlBoolean ToSqlValue(this bool? input)
        {
            return input.HasValue ? new SqlBoolean(input.Value) : SqlBoolean.Null;
        }

        public static SqlBinary ToSqlValue(this Binary input)
        {
            return input == null ? SqlBinary.Null : new SqlBinary(input.ToArray());
        }

        public static SqlBinary ToSqlValue(this Byte[] input)
        {
            return input == null ? SqlBinary.Null : new SqlBinary(input);
        }

        public static SqlDecimal ToSqlValue(this decimal input)
        {
            return input == decimal.MinValue ? SqlDecimal.Null : new SqlDecimal(input);
        }

        public static SqlDecimal ToSqlValue(this decimal? input)
        {
            return !input.HasValue ? SqlDecimal.Null : new SqlDecimal(input.Value);
        }

        public static SqlDouble ToSqlValue(this double input)
        {
            return input == double.MinValue ? SqlDouble.Null : new SqlDouble(input);
        }

        public static SqlDouble ToSqlValue(this double? input)
        {
            return !input.HasValue ? SqlDouble.Null : new SqlDouble(input.Value);
        }

        public static SqlSingle ToSqlValue(this float input)
        {
            return input == float.MinValue ? SqlSingle.Null : new SqlSingle(input);
        }

        public static SqlSingle ToSqlValue(this float? input)
        {
            return !input.HasValue ? SqlSingle.Null : new SqlSingle(input.Value);
        }
    }
}