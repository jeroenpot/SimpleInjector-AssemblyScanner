using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace SimpleInjector.AssemblyScanner.UnitTests
{
    [TestFixture]
    public class DependencyRegistrationTests
    {
        private readonly IList<Type> _ignoreList = new List<Type>
        {
            typeof (CDCLass),
            typeof (IHasMultipleImplementations),
            typeof (IBClass),
            typeof (IThatHasConcreteImplementationWithConstructorArgument),
            typeof (IInterfaceOfT),
            typeof (ClassOfInterfaceT)
        };

        protected Container Container;

        [SetUp]
        public void Setup()
        {
            Container = new Container();
        }

        [Test]
        public void ShouldRegisterClasses()
        {
            DependencyRegistration.Register(Container, typeof(DependencyRegistrationTests).Assembly, _ignoreList.ToArray());

            IAClass aClass = Container.GetInstance<IAClass>();

            Assert.That(aClass, Is.InstanceOf<AClass>());
        }

        [Test]
        public void ShouldThrowExceptionWhenMultipleInterfaceImplementations()
        {
            try
            {
                DependencyRegistration.Register(Container, typeof(DependencyRegistrationTests).Assembly);
                Assert.Fail();
            }
            catch (DependencyConfigurationException dependencyConfigurationException)
            {
                Assert.That(dependencyConfigurationException.ValidationErrors, Has.Count.EqualTo(4));
            }
        }

        [Test]
        public void ShouldNotRegisterAnyClassOfInterfaceIDoNotRegisterMe()
        {
            var ignoreList = new List<Type>(_ignoreList);
            ignoreList.Add(typeof(IDoNotRegisterMe));
            DependencyRegistration.Register(Container, typeof(DependencyRegistrationTests).Assembly, ignoreList.ToArray());

            Assert.That(() => Container.GetInstance<IDoNotRegisterMe>(),
                Throws.Exception.TypeOf<ActivationException>()
                    .With.Message.EqualTo("No registration for type IDoNotRegisterMe could be found."));
        }

        [Test]
        public void ShouldNotRegisterInstanceOfDoNotRegisterMe()
        {
            var ignoreList = new List<Type>(_ignoreList);
            ignoreList.Add(typeof(DoNotRegisterMe));

            DependencyRegistration.Register(Container, typeof(DependencyRegistrationTests).Assembly, ignoreList.ToArray());

            Assert.That(() => Container.GetInstance<IDoNotRegisterMe>(),
                Throws.Exception.TypeOf<ActivationException>()
                    .With.Message.EqualTo("No registration for type IDoNotRegisterMe could be found."));
        }

        [Test]
        public void ShouldRegisterOtherTypesOfInstance()
        {
            var ignoreList = new List<Type>(_ignoreList);
            ignoreList.Remove(typeof(IHasMultipleImplementations));
            ignoreList.Add(typeof(FirstImplementation));

            DependencyRegistration.Register(Container, typeof(DependencyRegistrationTests).Assembly, ignoreList.ToArray());

            IHasMultipleImplementations instance = Container.GetInstance<IHasMultipleImplementations>();
            Assert.That(instance, Is.TypeOf<SecondImplementation>());
        }

        [Test]
        public void ShouldHandleGivingBothImplementationAndInterfaceAsIgnoreParameters()
        {
            try
            {
                DependencyRegistration.Register(Container, typeof(DependencyRegistrationTests).Assembly, typeof(DoNotRegisterMe), typeof(IDoNotRegisterMe), typeof(IHasMultipleImplementations), typeof(ClassOfInterfaceT));
            }
            catch (DependencyConfigurationException dependencyConfigurationException)
            {
                Assert.That(dependencyConfigurationException.ValidationErrors, Has.Count.EqualTo(2), dependencyConfigurationException.ToString());
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
            Assert.That(() => DependencyRegistration.Register(Container, null),
                Throws.Exception.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ShouldIgnoreAlreadyRegisteredInterface()
        {
            var ignoreList = new List<Type>(_ignoreList);
            ignoreList.Remove(typeof(IThatHasConcreteImplementationWithConstructorArgument));

            Container.Register<IThatHasConcreteImplementationWithConstructorArgument>(() => new ConstructorArgumentString("hello"));
            DependencyRegistration.Register(Container, this.GetType().Assembly, ignoreList.ToArray());

            var instance = Container.GetInstance<IThatHasConcreteImplementationWithConstructorArgument>();
            Assert.That(instance, Is.TypeOf<ConstructorArgumentString>());
        }

        [Test]
        public void ShouldIgnoreBaseInterfaceWithNoRegisteredTypes()
        {
            var ignoreList = new List<Type>(_ignoreList);
            ignoreList.Remove(typeof(IInterfaceOfT));

            Container.Register<IInterfaceOfT>(() => new ClassOfInterfaceT(1));
            DependencyRegistration.Register(Container, GetType().Assembly, ignoreList.ToArray());
        }

        [Test]
        public void ShouldGetClassWithMultipleInterfaces()
        {
            var container = new Container();
            DependencyRegistration.Register(container, GetType().Assembly, _ignoreList.ToArray());

            IIAmAnInterface abc = container.GetInstance<IIAmAnInterface>();

            Assert.That(abc, Is.TypeOf<AnotherClass>());
        }

        [Test]
        public void ShouldGetClassThatMatchesNamingConvention()
        {
            DependencyRegistration.Register(Container, GetType().Assembly, _ignoreList.ToArray());

            var namingConventionClass = Container.GetInstance<INamingConventionClass>();

            Assert.That(namingConventionClass, Is.TypeOf<NamingConventionClass>());
        }

        [Test]
        public void ShouldGetClassThatMatchesNamingConventionWhenOtherTypeIsRegisteredFirst()
        {
            DependencyRegistration.Register(Container, GetType().Assembly, _ignoreList.ToArray());

            var namingConventionClass = Container.GetInstance<IOtherNamingConventionClass>();

            Assert.That(namingConventionClass, Is.TypeOf<OtherNamingConventionClass>());
        }

        [Test]
        public void ShouldGetClassThatMatchesNamingConventionWhenHasMultipleInterfaces()
        {
            DependencyRegistration.Register(Container, GetType().Assembly, _ignoreList.ToArray());

            var namingConventionClass = Container.GetInstance<ICorrectNamingInterface>();

            Assert.That(namingConventionClass, Is.TypeOf<CorrectNamingInterface>());
        }
    }
}