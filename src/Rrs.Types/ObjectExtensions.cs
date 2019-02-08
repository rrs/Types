using System.Collections.Generic;
using System.Reflection;

namespace Rrs.Types
{
    public static class ObjectExtensions
    {
        public static PropertyInfo GetProperty<T>(this T _, string propertyName)
        {
            return PropertyCache.Get<T>(propertyName);
        }

        public static IEnumerable<PropertyInfo> GetProperties<T>(this T _)
        {
            return PropertyCache.Get<T>();
        }
    }
}
