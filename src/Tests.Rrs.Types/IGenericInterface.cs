namespace Tests.Rrs.Types
{
    interface IGenericInterface<T>
    {
    }

    class IntImplementationOfGenericInterface : IGenericInterface<int>
    {
    }

    class InterfaceImplementationOfGenericInterface : IGenericInterface<IInterface>
    {
    }

    interface IGenericInInterface<in T>
    {
    }

    class GenericInImplementation : IGenericInInterface<int>
    {
    }

    class GenericInInterfaceImplementation : IGenericInInterface<IInterface>
    {
    }

    interface IGenericOutInterface<out T>
    {
    }

    class GenericOutImplementation : IGenericInInterface<int>
    {
    }

    class GenericOutInterfaceImplementation : IGenericInInterface<IInterface>
    {
    }

    class GenericImplementationOfGenericInterface<T> : IGenericInInterface<T>
    {
    }
}
