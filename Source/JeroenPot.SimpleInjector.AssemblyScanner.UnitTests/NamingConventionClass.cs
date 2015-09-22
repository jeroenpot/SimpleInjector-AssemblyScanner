namespace JeroenPot.SimpleInjector.AssemblyScanner.UnitTests
{
    public class NamingConventionClass : INamingConventionClass
    {
    }

    public class NotConformingToNamingConvention : INamingConventionClass
    {
    }

    public interface INamingConventionClass
    {
    }

    public interface IOtherNamingConventionClass
    {
    }

    public class ANotMatchingConvention : IOtherNamingConventionClass
    {
    }

    public class OtherNamingConventionClass : IOtherNamingConventionClass
    {
    }

    public interface ICorrectNamingInterface
    {
    }

    public interface IThisDoesNotMatchNamingConvention
    {
    }

    public class CorrectNamingInterface : ICorrectNamingInterface
    {
    }
}


