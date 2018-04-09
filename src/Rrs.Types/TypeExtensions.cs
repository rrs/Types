using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rrs.Types
{
    public static class TypeExtensions
    {
        public static bool InheritsOrImplements(this Type child, Type parent)
        {
            parent = ResolveGenericTypeDefinition(parent);

            var currentChild = child.IsGenericType
                ? child.GetGenericTypeDefinition()
                : child;

            while (currentChild != typeof(object))
            {
                if (parent == currentChild || HasAnyInterfaces(parent, currentChild))
                    return true;

                currentChild = currentChild.BaseType != null
                               && currentChild.BaseType.IsGenericType
                    ? currentChild.BaseType.GetGenericTypeDefinition()
                    : currentChild.BaseType;

                if (currentChild == null)
                    return false;
            }
            return false;
        }

        public static bool IsConcreteImplementation(this Type child, Type parent)
        {
            return child.InheritsOrImplements(parent) && !child.IsAbstract;
        }

        public static IEnumerable<Type> ConcreteImplementationsOf(this Type type, Assembly assembly = null)
        {
            assembly = assembly ?? Assembly.GetExecutingAssembly();
            return assembly.GetReferencedAssemblies()
                            .Select(Assembly.Load)
                            .SelectMany(a => a.GetTypes().Where(t => t.IsConcreteImplementation(type)))
                            .ToList();
        }

        private static bool HasAnyInterfaces(Type parent, Type child)
        {
            return child.GetInterfaces()
                .Any(childInterface =>
                {
                    if (childInterface == parent) return true;

                    var currentInterface = childInterface.IsGenericType
                        ? childInterface.GetGenericTypeDefinition()
                        : childInterface;

                    return currentInterface == parent;
                });
        }

        private static Type ResolveGenericTypeDefinition(Type parent)
        {
            bool shouldUseGenericType = !(parent.IsGenericType && parent.GetGenericTypeDefinition() != parent);

            if (parent.IsGenericType && shouldUseGenericType) parent = parent.GetGenericTypeDefinition();
            return parent;
        }

        public static IEnumerable<PropertyInfo> GetFlattenedProperties(this Type type, BindingFlags bindingFlags = BindingFlags.Default)
        {
            if (!type.IsInterface)
                return type.GetProperties(bindingFlags);

            return (new Type[] { type })
                   .Concat(type.GetInterfaces())
                   .SelectMany(i => i.GetProperties(bindingFlags));
        }

        public static bool ImplementsInterfaceWithGenericTypeOf(this Type typeToCheck, Type interfaceType, Type genericType)
        {
            return typeToCheck.GetInterfaces().Any(o => o.IsGenericType &&
                                                        o.GetGenericTypeDefinition() == interfaceType &&
                                                        o.GetGenericArguments()[0] == genericType);
        }

        public static Type GetEnumerableItemType(this Type type)
        {
            // Type is Array
            // short-circuit if you expect lots of arrays 
            if (type.IsArray)
                return type.GetElementType();

            // type is IEnumerable<T>;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return type.GetGenericArguments()[0];

            // type implements/extends IEnumerable<T>;
            return type.GetInterfaces()
                                    .Where(t => t.IsGenericType &&
                                           t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                                    .Select(t => t.GetGenericArguments()[0]).FirstOrDefault();
        }

        private static readonly char[] SystemTypeChars = { '<', '>', '+' };

        public static bool IsSystemType(this Type type)
        {
            return type.Namespace == null
                || type.Namespace.StartsWith("System", StringComparison.Ordinal)
                || type.Name.IndexOfAny(SystemTypeChars) >= 0;
        }

        public static bool HasInterface(this Type type, Type interfaceType)
        {
            foreach (var t in type.GetInterfaces())
            {
                if (t == interfaceType)
                    return true;
            }
            return false;
        }

        public static MethodInfo GetGenericMethod(this Type type, string name, params Type[] types)
        {
            return type.GetMethods().FirstOrDefault(m => m.Name == name && m.GetParameters().Select(p => p.ParameterType).SequenceEqual(types, new GenericParameterComparer()));
        }

        // not a good general solution, it serves the purpose here for now
        class GenericParameterComparer : IEqualityComparer<Type>
        {
            public bool Equals(Type x, Type y)
            {
                if (x == y) return true;
                if (x.IsGenericType && y.IsGenericType && x.GetGenericTypeDefinition() == y.GetGenericTypeDefinition()) return true;
                if (x.BaseType.IsAssignableFrom(y)) return true;
                return false;
            }

            public int GetHashCode(Type obj)
            {
                throw new NotImplementedException();
            }
        }
    }
}
