using Rrs.Types;
using System;
using System.Linq;
using Xunit;

namespace Tests.Rrs.Types
{
    // naming needs some attention
    public class ConcreteTypeTests
    {
        [Fact]
        public void TestConcreteTypeGeneratorClass()
        {
            DerivedClass c = ConcreteTypeGenerator.Get<DerivedClass>();
            Assert.NotNull(c);
        }

        [Fact]
        public void TestIgnoreAbstract()
        {
            var isConcreteImplementation = typeof(BaseAbstractClass).IsConcreteImplementation(typeof(BaseAbstractClass));

            Assert.False(isConcreteImplementation);
        }

        [Fact]
        public void TestConcreteDerivedClass()
        {
            var isConcreteImplementation = typeof(DerivedClass).IsConcreteImplementation(typeof(BaseAbstractClass));

            Assert.True(isConcreteImplementation);
        }

        [Fact]
        public void TestIgnoreInterface()
        {
            var isConcreteImplementation = typeof(IInterface).IsConcreteImplementation(typeof(IInterface));

            Assert.False(isConcreteImplementation);
        }

        [Fact]
        public void TestInterfaceImplementation()
        {
            var isConcreteImplementation = typeof(InterfaceImplementation).IsConcreteImplementation(typeof(IInterface));

            Assert.True(isConcreteImplementation);
        }

        [Fact]
        public void TestIgnoreGenericInterface()
        {
            var isConcreteImplementation = typeof(IGenericInterface<>).IsConcreteImplementation(typeof(IGenericInterface<int>));

            Assert.False(isConcreteImplementation);

            isConcreteImplementation = typeof(IGenericInterface<int>).IsConcreteImplementation(typeof(IGenericInterface<int>));

            Assert.False(isConcreteImplementation);
        }

        [Fact]
        public void TestGenericImplementation()
        {
            var isConcreteImplementation = typeof(IntImplementationOfGenericInterface).IsConcreteImplementation(typeof(IGenericInterface<>));

            Assert.True(isConcreteImplementation);

            isConcreteImplementation = typeof(IntImplementationOfGenericInterface).IsConcreteImplementation(typeof(IGenericInterface<int>));

            Assert.True(isConcreteImplementation);
        }

        [Fact]
        public void TestGenericInterfaceImplementation()
        {
            var isConcreteImplementation = typeof(InterfaceImplementationOfGenericInterface).IsConcreteImplementation(typeof(IGenericInterface<InterfaceImplementation>));

            Assert.False(isConcreteImplementation);
        }

        [Fact]
        public void TestGenericInInterfaceImplementation()
        {
            var isConcreteImplementation = typeof(GenericInInterfaceImplementation).IsConcreteImplementation(typeof(IGenericInInterface<InterfaceImplementation>));

            Assert.True(isConcreteImplementation);
        }

        [Fact]
        public void TestGenericOutInterfaceImplementation()
        {
            var isConcreteImplementation = typeof(GenericOutInterfaceImplementation).IsConcreteImplementation(typeof(IGenericOutInterface<InterfaceImplementation>));

            Assert.False(isConcreteImplementation);
        }

        [Fact]
        public void TestDualGenericInterfaceWithGenericImplementation()
        {
            var isConcreteImplementation = typeof(DualGenericImplementation<>).IsConcreteImplementation(typeof(IDualInterface<DerivedClass,DerivedClass>));

            //Assert.True(isConcreteImplementation);

            var cls = typeof(DualGenericImplementation<>);
            var clsGeneric = cls.GetInterfaces()[0];
            var clsGenTypeDef = clsGeneric.GetGenericTypeDefinition();
            var clsParams = clsGeneric.GetGenericArguments();

            var args = typeof(IDualInterface<,>).GetGenericArguments();

            var inter = typeof(IDualInterface<IGenericInterface<int>, int>);
            var generic = inter.GetGenericTypeDefinition();
            var parameters = inter.GetGenericArguments();


        }

        //[Fact]
        //public void TestGenericImplementationOfGenericInterface()
        //{
        //    var isConcreteImplementation = typeof(GenericImplementationOfGenericInterface<>).IsConcreteImplementation(typeof(IGenericInInterface<int>));

        //    Assert.True(isConcreteImplementation);
        //}

        //[Fact]
        //public void TestFindConcreteImplementationsOfDualInterface()
        //{
        //    var types = typeof(IDualInterface<IGenericInterface<int>, int>).ConcreteImplementationsOf();

        //    Assert.Equal(typeof(DualGenericImplementation<int>), types.Single());
        //}


        //[Fact]
        //public void TestConcreteTypeGeneratorGenericType()
        //{
        //    DualGenericImplementation<DerivedClass> c = ConcreteTypeGenerator.Get<DualGenericImplementation<DerivedClass>>();
        //    Assert.NotNull(c);
        //}
    }
}
