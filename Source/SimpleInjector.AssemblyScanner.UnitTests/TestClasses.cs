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
}