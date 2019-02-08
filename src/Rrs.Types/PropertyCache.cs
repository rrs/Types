using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rrs.Types
{
    public static class PropertyCache
    {
        public static IEnumerable<PropertyInfo> Get<T>()
        {
            return PropertyCache<T>.Get().Values;
        }

        public static PropertyInfo Get<T>(string propertyName)
        {
            return PropertyCache<T>.Get()[propertyName];
        }
    }

    public static class PropertyCache<T>
    {
        private static readonly IDictionary<string, PropertyInfo> _properties;

        static PropertyCache()
        {
            _properties = typeof(T).GetFlattenedProperties().ToDictionary(o => o.Name);
        }

        public static IDictionary<string, PropertyInfo> Get()
        {
            return _properties;
        }
    }
}
