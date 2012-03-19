using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Adotic
{
    public interface IPropertyImpedanceMapper<TClass> where TClass : class
    {
        Expression<Func<TClass, object>> PropertyExpression { get; }
        void HandleImpedance(PropertyInfo p, ref object value);
    }
}