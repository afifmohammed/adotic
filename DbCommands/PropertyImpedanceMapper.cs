using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Adotic
{
    public abstract class PropertyImpedanceMapper<TClass> : IPropertyImpedanceMapper<TClass> where TClass : class
    {
        public abstract Expression<Func<TClass, object>> PropertyExpression { get; }
        
        public void HandleImpedance(PropertyInfo p, ref object value)
        {
            if (p.Name != PropertyExpression.Name())
                return;

            var propertyType = p.PropertyType;

            var valueType = value.GetType();
            if (valueType == propertyType) return;

            HandlePropertyImpedance(ref value);
        }

        protected abstract void HandlePropertyImpedance(ref object value);
    }
}