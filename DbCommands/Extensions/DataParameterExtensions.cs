using System.Data;

namespace Adotic
{
    internal static class DataParameterExtensions
    {
        public static bool IsAnOutputParameter(this IDataParameter parameter)
        {
            return parameter.Direction == ParameterDirection.InputOutput ||
                   parameter.Direction == ParameterDirection.Output;
        }
    }
}