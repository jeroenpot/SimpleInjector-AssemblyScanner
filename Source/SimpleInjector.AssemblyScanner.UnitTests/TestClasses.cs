﻿namespace SimpleInjector.AssemblyScanner.UnitTests
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

    public class DontRegisterMe : IDontRegisterMe
    {
    }

    public interface IDontRegisterMe
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
        public ConstructorArgumentString(string someString)
        {
            SomeString = someString;
        }
    }

    public class ClassOfInterfaceT : IInterfaceOfT
    {
        public ClassOfInterfaceT(int i)
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
}