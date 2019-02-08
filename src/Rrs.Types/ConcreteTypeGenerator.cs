using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Rrs.Types
{
    public static class ConcreteTypeGenerator
    {
        public static T Get<T>()
        {
            return ConcreteTypeGenerator<T>.Get().Single();
        }

        public static IEnumerable<T> GetMany<T>()
        {
            return ConcreteTypeGenerator<T>.Get();
        }
    }

    public static class ConcreteTypeGenerator<T>
    {
        private static readonly IEnumerable<Func<T>> _newFuncs;

        static ConcreteTypeGenerator()
        {
            var concreteTypes = typeof(T).ConcreteImplementationsOf();
            _newFuncs = concreteTypes.Select(t => Expression.Lambda<Func<T>>(Expression.New(t)).Compile());
        }

        public static IEnumerable<T> Get()
        {
            return _newFuncs.Select(o => o());
        }
    }
}
