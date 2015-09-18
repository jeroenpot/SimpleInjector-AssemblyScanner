using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace SimpleInjector.AssemblyScanner.UnitTests
{
    [TestFixture]
    public class DependencyRegistrationTests
    {
        private readonly IList<Type> _ignoreList = new List<Type> { typeof(CDCLass), typeof(IHasMultipleImplementations), typeof(IBClass) };

        [Test]
        public void ShouldRegisterClasses()
        {
            var container = new Container();
            DependencyRegistration.Register(container, typeof(DependencyRegistrationTests).Assembly, _ignoreList.ToArray());

            IAClass aClass = container.GetInstance<IAClass>();

            Assert.That(aClass, Is.InstanceOf<AClass>());
        }

        [Test]
        public void ShouldThrowExceptionWhenMultipleInterfaceImplementations()
        {
            var container = new Container();

            try
            {
                DependencyRegistration.Register(container, typeof(DependencyRegistrationTests).Assembly);
                Assert.Fail();
            }
            catch (DependencyConfigurationException dependencyConfigurationException)
            {
                Assert.That(dependencyConfigurationException.ValidationErrors, Has.Count.EqualTo(3));
            }
        }

        [Test]
        public void ShouldNotRegisterAnyClassOfInterfaceIDontRegisterMe()
        {
            var container = new Container();

            var ignoreList = new List<Type>(_ignoreList);
            ignoreList.Add(typeof(IDontRegisterMe));
            DependencyRegistration.Register(container, typeof(DependencyRegistrationTests).Assembly, ignoreList.ToArray());

            Assert.That(() => container.GetInstance<IDontRegisterMe>(),
                Throws.Exception.TypeOf<ActivationException>()
                    .With.Message.EqualTo("No registration for type IDontRegisterMe could be found."));
        }

        [Test]
        public void ShouldNotRegisterInstanceOfDontRegisterMe()
        {
            var container = new Container();

            var ignoreList = new List<Type>(_ignoreList);
            ignoreList.Add(typeof(DontRegisterMe));

            DependencyRegistration.Register(container, typeof(DependencyRegistrationTests).Assembly, ignoreList.ToArray());

            Assert.That(() => container.GetInstance<IDontRegisterMe>(),
                Throws.Exception.TypeOf<ActivationException>()
                    .With.Message.EqualTo("No registration for type IDontRegisterMe could be found."));
        }

        [Test]
        public void ShouldRegisterOtherTypesOfInstance()
        {
            var container = new Container();

            var ignoreList = new List<Type>(_ignoreList);
            ignoreList.Remove(typeof (IHasMultipleImplementations));
            ignoreList.Add(typeof(FirstImplementation));

            DependencyRegistration.Register(container, typeof(DependencyRegistrationTests).Assembly, ignoreList.ToArray());

            IHasMultipleImplementations instance = container.GetInstance<IHasMultipleImplementations>();
            Assert.That(instance, Is.TypeOf<SecondImplementation>());
        }

        [Test]
        public void ShouldHandleGivingBothImplementationAndInterfaceAsIgnoreParameters()
        {
            var container = new Container();

            try
            {
                DependencyRegistration.Register(container, typeof(DependencyRegistrationTests).Assembly, typeof(DontRegisterMe), typeof(IDontRegisterMe), typeof(IHasMultipleImplementations));
            }
            catch (DependencyConfigurationException dependencyConfigurationException)
            {
                Assert.That(dependencyConfigurationException.ValidationErrors, Has.Count.EqualTo(2));
            }
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionForContainer()
        {
            Assert.That(() =>
                DependencyRegistration.Register(null, typeof(DependencyRegistrationTests).Assembly,
                    _ignoreList.ToArray()), Throws.Exception.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ShouldThrowExceptionWhenAssemblyIsNull()
        {
            var container = new Container();

            Assert.That(() => DependencyRegistration.Register(container, null),
                Throws.Exception.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ShouldIgnoreAlreadyRegisterdInterface()
        {
            var ignoreList = new List<Type>(_ignoreList);
            ignoreList.Remove(typeof(IThatHasConcreteImplementationWithConstructorArgument));
            var container = new Container();
            DependencyRegistration.Register(container, this.GetType().Assembly, ignoreList.ToArray());

            var instance = container.GetInstance<IThatHasConcreteImplementationWithConstructorArgument>();
        }
    }
}