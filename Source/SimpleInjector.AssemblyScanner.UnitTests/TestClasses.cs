using System;

namespace SimpleInjector.AssemblyScanner.UnitTests
{
    public class AClass : IAClass
    {
    }

    public interface IAClass
    {
    }

    public class B1Class : IBClass
    {
    }

    public class B2Class : IBClass
    {
    }

    public interface IBClass
    {
    }

    public class CDCLass : ICClass, IDClass
    {
    }

    public interface ICClass
    {
    }

    public interface IDClass
    {
    }

    public class DoNotRegisterMe : IDoNotRegisterMe
    {
    }

    public interface IDoNotRegisterMe
    {
    }

    public class FirstImplementation : IHasMultipleImplementations
    {
    }

    public class SecondImplementation : IHasMultipleImplementations
    {
    }

    public interface IHasMultipleImplementations
    {
    }

    public interface IThatHasConcreteImplementationWithConstructorArgument
    {
        string SomeString { get; set; }
    }

    public class ConstructorArgumentString : IThatHasConcreteImplementationWithConstructorArgument
    {
        public string SomeString { get; set; }
        // ReSharper disable once UnusedParameter.Local
        public ConstructorArgumentString(string value)
        {
            SomeString = value;
        }
    }

    public class ClassOfInterfaceT : IInterfaceOfT
    {
        public ClassOfInterfaceT(int value)
        {
            
        }
    }

    public interface IInterfaceOfT : IBaseInterface<SomeObject>
    {
    }

    public interface IBaseInterface<T>
    {
    }

    public class SomeObject
    {
    }

    public interface IIAmAnInterface
    {
    }

    public class AnotherClass : IIAmAnInterface, IComparable
    {
        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }
}