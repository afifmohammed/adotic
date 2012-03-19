using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Adotic
{
    internal interface IDataReaderMapper<out TClass> : IMapper<IDataReader, TClass> {}

    internal class DataReaderMapper<TClass> : IDataReaderMapper<TClass>
        where TClass : class, new()
    {
        private readonly IDictionary<string, IDictionary<string, PropertyInfo>> _propertyMap;
        private readonly IEnumerable<IPropertyImpedanceMapper<TClass>> _propertyImpedanceMappers;

        public DataReaderMapper(
            IDictionary<string, IDictionary<string, PropertyInfo>> propertyMap, 
            IEnumerable<IPropertyImpedanceMapper<TClass>> propertyImpedanceMappers)
        {
            _propertyMap = propertyMap;
            _propertyImpedanceMappers = propertyImpedanceMappers;
        }

        public TClass MapFrom(IDataReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            var destination = Activator.CreateInstance<TClass>();

            var destinationName = typeof (TClass).Name;

            if (!_propertyMap.ContainsKey(destinationName))
                _propertyMap.Add(destinationName, typeof(TClass).GetProperties().ToDictionary(pi => pi.Name.ToLower(), pi => pi));

            var destinationPropertyMap = _propertyMap[destinationName];

            for (var i = 0; i < reader.FieldCount; i++)
            {
                PropertyInfo propertyInfo;
                var columnName = reader.GetName(i).ToLower();

                if (!destinationPropertyMap.TryGetValue(columnName, out propertyInfo)) continue;

                var value = DBNull.Value.Equals(reader[i]) ? null : reader[i];
                if (value == null)
                    continue;
                
                var propType = propertyInfo.PropertyType;

                _propertyImpedanceMappers.ForEach(x => x.HandleImpedance(propertyInfo, ref value));
                
                if (value.GetType() != propType)
                {
                    var matching = Convert.ChangeType(value, Nullable.GetUnderlyingType(propType) ?? propType);
                    propertyInfo.SetValue(destination, matching, null);
                }
                else
                    propertyInfo.SetValue(destination, value, null);
            }

            return destination;
        }
    }
}