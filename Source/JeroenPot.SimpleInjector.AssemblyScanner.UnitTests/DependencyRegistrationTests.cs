using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SimpleInjector;

namespace JeroenPot.SimpleInjector.AssemblyScanner.UnitTests
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

            aClass.Should().BeOfType<AClass>();
        }

        [Test]
        public void ShouldThrowExceptionWhenMultipleInterfaceImplementations()
        {
            Action action = () => DependencyRegistration.Register(Container, typeof(DependencyRegistrationTests).Assembly);

            var dependencyConfigurationException = action.ShouldThrow<DependencyConfigurationException>().Which;

            dependencyConfigurationException.ValidationErrors.Errors.Should().HaveCount(5);
            dependencyConfigurationException.Message.Should().Contain("Multiple Implementations found for [JeroenPot.SimpleInjector.AssemblyScanner.UnitTests.IBClass]");
        }

        [Test]
        public void ShouldNotRegisterAnyClassOfInterfaceIDoNotRegisterMe()
        {
            var ignoreList = new List<Type>(_ignoreList) { typeof(IDoNotRegisterMe) };
            DependencyRegistration.Register(Container, typeof(DependencyRegistrationTests).Assembly, ignoreList.ToArray());

            Action action = () => Container.GetInstance<IDoNotRegisterMe>();

            action.ShouldThrow<ActivationException>()
                .WithMessage("No registration for type IDoNotRegisterMe could be found.");
        }

        [Test]
        public void ShouldNotRegisterInstanceOfDoNotRegisterMe()
        {
            var ignoreList = new List<Type>(_ignoreList) { typeof(DoNotRegisterMe) };
            DependencyRegistration.Register(Container, typeof(DependencyRegistrationTests).Assembly, ignoreList.ToArray());

            Action action = () => Container.GetInstance<IDoNotRegisterMe>();

            action.ShouldThrow<ActivationException>()
                .WithMessage("No registration for type IDoNotRegisterMe could be found.");
        }

        [Test]
        public void ShouldRegisterOtherTypesOfInstance()
        {
            var ignoreList = new List<Type>(_ignoreList);
            ignoreList.Remove(typeof(IHasMultipleImplementations));
            ignoreList.Add(typeof(FirstImplementation));

            DependencyRegistration.Register(Container, typeof(DependencyRegistrationTests).Assembly, ignoreList.ToArray());

            IHasMultipleImplementations instance = Container.GetInstance<IHasMultipleImplementations>();
            instance.Should().BeOfType<SecondImplementation>();
        }

        [Test]
        public void ShouldHandleGivingBothImplementationAndInterfaceAsIgnoreParameters()
        {
            Action action =
                () =>
                    DependencyRegistration.Register(Container, typeof(DependencyRegistrationTests).Assembly,
                        typeof(DoNotRegisterMe), typeof(IDoNotRegisterMe), typeof(IHasMultipleImplementations),
                        typeof(ClassOfInterfaceT), typeof(IThatHasConcreteImplementationWithConstructorArgument));

            action.ShouldThrow<DependencyConfigurationException>().And.ValidationErrors.Errors.Should().HaveCount(2);
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionForContainer()
        {
            Action action = () => DependencyRegistration.Register(null, typeof(DependencyRegistrationTests).Assembly, _ignoreList.ToArray());

            action.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void ShouldThrowExceptionWhenAssemblyIsNull()
        {
            Action action = () => DependencyRegistration.Register(Container, null);

            action.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void ShouldIgnoreAlreadyRegisteredInterface()
        {
            var ignoreList = new List<Type>(_ignoreList);
            ignoreList.Remove(typeof(IThatHasConcreteImplementationWithConstructorArgument));

            Container.Register<IThatHasConcreteImplementationWithConstructorArgument>(() => new ConstructorArgumentString("hello"));
            DependencyRegistration.Register(Container, GetType().Assembly, ignoreList.ToArray());

            var instance = Container.GetInstance<IThatHasConcreteImplementationWithConstructorArgument>();

            instance.Should().BeOfType<ConstructorArgumentString>();
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

            abc.Should().BeOfType<AnotherClass>();
        }

        [Test]
        public void ShouldGetClassThatMatchesNamingConvention()
        {
            DependencyRegistration.Register(Container, GetType().Assembly, _ignoreList.ToArray());

            var namingConventionClass = Container.GetInstance<INamingConventionClass>();

            namingConventionClass.Should().BeOfType<NamingConventionClass>();
        }

        [Test]
        public void ShouldGetClassThatMatchesNamingConventionWhenOtherTypeIsRegisteredFirst()
        {
            DependencyRegistration.Register(Container, GetType().Assembly, _ignoreList.ToArray());

            var namingConventionClass = Container.GetInstance<IOtherNamingConventionClass>();

            namingConventionClass.Should().BeOfType<OtherNamingConventionClass>();
        }

        [Test]
        public void ShouldGetClassThatMatchesNamingConventionWhenHasMultipleInterfaces()
        {
            DependencyRegistration.Register(Container, GetType().Assembly, _ignoreList.ToArray());

            var namingConventionClass = Container.GetInstance<ICorrectNamingInterface>();

            namingConventionClass.Should().BeOfType<CorrectNamingInterface>();
        }

        [Test]
        public void ShouldNotRegisterAbstractClass()
        {
            DependencyRegistration.Register(Container, typeof(DependencyRegistrationTests).Assembly, _ignoreList.ToArray());

            Action action = () => Container.GetInstance<IAbstractClass>();

            action.ShouldThrow<ActivationException>()
                .WithMessage("No registration for type IAbstractClass could be found.");
        }
    }
}