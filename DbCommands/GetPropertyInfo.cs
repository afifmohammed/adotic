using System;
using System.Collections.Generic;
using System.Reflection;

namespace Adotic
{
    internal class GetPropertyInfo
    {
        private readonly IDictionary<Type, PropertyInfo[]> _propertyInfoForTypes;

        public GetPropertyInfo()
        {
            _propertyInfoForTypes = new Dictionary<Type, PropertyInfo[]>();
        }

        public IEnumerable<PropertyInfo> For(Type type)
        {
            if (!_propertyInfoForTypes.ContainsKey(type))
                _propertyInfoForTypes.Add(type, type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            return _propertyInfoForTypes[type];
        }
    }
}