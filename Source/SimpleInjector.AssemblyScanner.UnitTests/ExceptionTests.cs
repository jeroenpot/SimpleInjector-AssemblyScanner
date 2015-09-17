using AutoTest.Exceptions;
using NUnit.Framework;

namespace SimpleInjector.AssemblyScanner.UnitTests
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
