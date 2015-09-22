using AutoTest.Exceptions;
using NUnit.Framework;

namespace JeroenPot.SimpleInjector.AssemblyScanner.UnitTests
{
    [TestFixture]
    public class ExceptionTests
    {
        [Test]
        public void ShouldValidateExceptions()
        {
            ExceptionTester.TestAllExceptions(typeof(DependencyConfigurationException).Assembly);
        }
    }
}
