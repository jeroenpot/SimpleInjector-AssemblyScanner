using SimpleInjector;

namespace $DefaultNamespace$.Configuration
{
    public static class SimpleInjectorBootstrapper
    {
        public static void Bootstrap(Container container)
        {
            JeroenPot.SimpleInjector.AssemblyScanner.DependencyRegistration.Register(container, typeof(SimpleInjectorBootstrapper).Assembly);
        }
    }
}
