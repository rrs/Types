using System;
using System.Collections.Generic;
using System.Linq;

namespace Rrs.Types
{
    public static class TypeCache
    {
        public static IEnumerable<Type> Types { get; private set;  }

        static TypeCache()
        {
            CacheLoadedTypes();
        }

        /// <summary>
        /// Use in case all types available where not loaded
        /// </summary>
        public static void CacheLoadedTypes()
        {
            Types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).ToList();
        }
    }
}
