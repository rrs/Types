using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Rrs.Types;

public static class RuntimePropertyGetter
{
    private static readonly Dictionary<Type, Dictionary<string, Func<object, object>>> _getters = new();

    public static object Get(object source, string propertyName)
    {
        if (source == null) return null;
        var sourceType = source.GetType();
        if (!_getters.ContainsKey(sourceType))
        {
            var properties = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var getters = new Dictionary<string, Func<object, object>>();
            foreach (var prop in properties)
            {
                var parameterExpression = Expression.Parameter(typeof(object));
                var body = Expression.Convert(Expression.Property(Expression.Convert(parameterExpression, sourceType), propertyName), typeof(object));
                var lambda = Expression.Lambda<Func<object, object>>(body, parameterExpression);
                getters.Add(prop.Name, lambda.Compile());
            }
            _getters[sourceType] = getters;
        }

        return _getters[sourceType][propertyName](source);
    }
}
