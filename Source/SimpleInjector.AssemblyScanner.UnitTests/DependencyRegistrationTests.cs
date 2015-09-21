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
                Assert.That(dependencyConfigurationException.ValidationErrors, Has.Count.EqualTo(4));
            }
        }

        [Test]
        public void ShouldNotRegisterAnyClassOfInterfaceIDoNotRegisterMe()
        {
            var container = new Container();

            var ignoreList = new List<Type>(_ignoreList);
            ignoreList.Add(typeof(IDoNotRegisterMe));
            DependencyRegistration.Register(container, typeof(DependencyRegistrationTests).Assembly, ignoreList.ToArray());

            Assert.That(() => container.GetInstance<IDoNotRegisterMe>(),
                Throws.Exception.TypeOf<ActivationException>()
                    .With.Message.EqualTo("No registration for type IDoNotRegisterMe could be found."));
        }

        [Test]
        public void ShouldNotRegisterInstanceOfDoNotRegisterMe()
        {
            var container = new Container();

            var ignoreList = new List<Type>(_ignoreList);
            ignoreList.Add(typeof(DoNotRegisterMe));

            DependencyRegistration.Register(container, typeof(DependencyRegistrationTests).Assembly, ignoreList.ToArray());

            Assert.That(() => container.GetInstance<IDoNotRegisterMe>(),
                Throws.Exception.TypeOf<ActivationException>()
                    .With.Message.EqualTo("No registration for type IDoNotRegisterMe could be found."));
        }

        [Test]
        public void ShouldRegisterOtherTypesOfInstance()
        {
            var container = new Container();

            var ignoreList = new List<Type>(_ignoreList);
            ignoreList.Remove(typeof(IHasMultipleImplementations));
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
                DependencyRegistration.Register(container, typeof(DependencyRegistrationTests).Assembly, typeof(DoNotRegisterMe), typeof(IDoNotRegisterMe), typeof(IHasMultipleImplementations), typeof(ClassOfInterfaceT));
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
            var container = new Container();

            Assert.That(() => DependencyRegistration.Register(container, null),
                Throws.Exception.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ShouldIgnoreAlreadyRegisteredInterface()
        {
            var ignoreList = new List<Type>(_ignoreList);
            ignoreList.Remove(typeof(IThatHasConcreteImplementationWithConstructorArgument));
            var container = new Container();

            container.Register<IThatHasConcreteImplementationWithConstructorArgument>(() => new ConstructorArgumentString("hello"));
            DependencyRegistration.Register(container, this.GetType().Assembly, ignoreList.ToArray());

            var instance = container.GetInstance<IThatHasConcreteImplementationWithConstructorArgument>();
            Assert.That(instance, Is.TypeOf<ConstructorArgumentString>());
        }

        [Test]
        public void ShouldIgnoreBaseInterfaceWithNoRegisteredTypes()
        {
            var ignoreList = new List<Type>(_ignoreList);
            ignoreList.Remove(typeof(IInterfaceOfT));
            var container = new Container();

            container.Register<IInterfaceOfT>(() => new ClassOfInterfaceT(1));
            DependencyRegistration.Register(container, GetType().Assembly, ignoreList.ToArray());
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
            var container = new Container();
            DependencyRegistration.Register(container, GetType().Assembly, _ignoreList.ToArray());

            var namingConventionClass = container.GetInstance<INamingConventionClass>();

            Assert.That(namingConventionClass, Is.TypeOf<NamingConventionClass>());
        }

        [Test]
        public void ShouldGetClassThatMatchesNamingConventionWhenOtherTypeIsRegisteredFirst()
        {
            var container = new Container();
            DependencyRegistration.Register(container, GetType().Assembly, _ignoreList.ToArray());

            var namingConventionClass = container.GetInstance<IOtherNamingConventionClass>();

            Assert.That(namingConventionClass, Is.TypeOf<OtherNamingConventionClass>());
        }

        [Test]
        public void ShouldGetClassThatMatchesNamingConventionWhenHasMultipleInterfaces()
        {
            var container = new Container();
            DependencyRegistration.Register(container, GetType().Assembly, _ignoreList.ToArray());

            var namingConventionClass = container.GetInstance<ICorrectNamingInterface>();

            Assert.That(namingConventionClass, Is.TypeOf<CorrectNamingInterface>());
        }
    }
}