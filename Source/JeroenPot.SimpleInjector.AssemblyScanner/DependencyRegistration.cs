using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using SimpleInjector;

namespace JeroenPot.SimpleInjector.AssemblyScanner
{
    /// <summary>
    /// Static helper class for scanning of assembly and automatic registration of classes and interfaces in the SimpleInjector container.
    /// </summary>
    public static class DependencyRegistration
    {
        /// <summary>
        /// Gets the registered interfaces and their types.
        /// </summary>
        /// <value>
        /// The registered interfaces with types.
        /// </value>
        public static IDictionary<Type, Type> RegisteredInterfacesWithTypes { get; private set; }

        /// <summary>
        /// The log writer to display information of the registered classes. 
        /// By default the implementation <see cref="DebugWriter"/> is used.
        /// </summary>
        public static ILogWriter LogWriter;

        /// <summary>
        /// Initializes the <see cref="DependencyRegistration"/> class.
        /// </summary>
        static DependencyRegistration()
        {
            LogWriter = new DebugWriter();
        }

#pragma warning disable CS3001 // Argument type is not CLS-compliant
        /// <summary>
        /// Registers the the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="assembly">The assembly.</param>
        /// <param name="typesToIgnore">The types you want this method to ignore (class and/or interface).</param>
        /// <exception cref="System.ArgumentNullException">assembly</exception>
        /// <exception cref="DependencyConfigurationException"></exception>
        /// <exception cref="ArgumentNullException">assembly</exception>
        public static void Register(Container container, Assembly assembly, params Type[] typesToIgnore)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            RegisteredInterfacesWithTypes = new Dictionary<Type, Type>();

            var registrations = GetTypes(container, assembly, typesToIgnore);

            ValidationErrors validationErrors = new ValidationErrors();

            foreach (Type type in registrations)
            {
                var interfaces = type.GetTypeInfo().ImplementedInterfaces.ToList();

                Type interfaceToUse;

                if (interfaces.Count > 1)
                {
                    interfaceToUse = InterfaceToUse(interfaces, type, validationErrors);
                    if (interfaceToUse != null)
                    {
                        Register(container, interfaceToUse, type, validationErrors);
                    }
                }
                else
                {
                    interfaceToUse = interfaces.First();
                    if (RegisteredInterfacesWithTypes.ContainsKey(interfaceToUse))
                    {
                        // Does this match the naming convention?
                        if (interfaceToUse.Name.Substring(1).Equals(type.Name))
                        {
                            bool currentOverrideOption = container.Options.AllowOverridingRegistrations;
                            container.Options.AllowOverridingRegistrations = true;
                            Register(container, interfaceToUse, type, validationErrors);
                            container.Options.AllowOverridingRegistrations = currentOverrideOption;
                        }
                        else
                        {
                            KeyValuePair<Type, Type> registeredInterface = RegisteredInterfacesWithTypes.First(x => x.Key == interfaceToUse);
                            if (!registeredInterface.Key.Name.Substring(1).Equals(registeredInterface.Value.Name))
                            {
                                // Neither of the interfaces matches naming convention.
                                validationErrors.Add($"Multiple Implementations found for [{interfaceToUse}]");
                            }
                        }
                    }
                    else
                    {
                        Register(container, interfaceToUse, type, validationErrors);
                    }
                }
            }

            if (validationErrors.Errors.Any())
            {
                throw new DependencyConfigurationException(string.Join(Environment.NewLine, validationErrors.Errors.Select(error => error.ErrorMessage)), validationErrors);
            }
        }

        private static IList<Type> GetTypes(Container container, Assembly assembly, Type[] typesToIgnore)
        {
            string assemblyName = assembly.GetName().Name;

            var existingRegistrationsServiceTypes =
                container.GetCurrentRegistrations().Select(instanceProducer => instanceProducer.ServiceType).ToList();

            IList<Type> registrations =
                assembly.ExportedTypes
                    .Where(type => !typesToIgnore.Contains(type))
                    .Where(type => !existingRegistrationsServiceTypes.Contains(type))
                    .Where(type => type.Namespace != null)
                    .Where(type => type.Namespace.StartsWith(assemblyName, StringComparison.OrdinalIgnoreCase))
                    .Where(type => (type.GetTypeInfo().Attributes & TypeAttributes.Abstract) != TypeAttributes.Abstract)
                    .Where(type => type.GetTypeInfo().ImplementedInterfaces.Any())
                    .Where(type => type.GetTypeInfo().ImplementedInterfaces.Any(
                        inter =>
                            !typesToIgnore.Contains(inter) && inter.Namespace != null &&
                            inter.Namespace.StartsWith(assemblyName, StringComparison.OrdinalIgnoreCase)))
                    .ToList();


            // Ignore already registerd interfaces:
            for (int i = registrations.Count - 1; i >= 0; i--)
            {
                foreach (var registrationInterface in registrations[i].GetTypeInfo().ImplementedInterfaces)
                {
                    if (existingRegistrationsServiceTypes.Contains(registrationInterface))
                    {
                        registrations.RemoveAt(i);
                        break;
                    }
                }
            }
            return registrations;
        }

        private static void Register(Container container, Type interfaceToUse, Type type, ValidationErrors validationErrors)
        {
            try
            {
                string message = $"Registering interface [{interfaceToUse}] with concrete implementation [{type}]";

                LogWriter.WriteLine(message);
                container.Register(interfaceToUse, type, Lifestyle.Transient);

                if (RegisteredInterfacesWithTypes.ContainsKey(interfaceToUse))
                {
                    RegisteredInterfacesWithTypes[interfaceToUse] = type;
                }
                else
                {
                    RegisteredInterfacesWithTypes.Add(interfaceToUse, type);
                }
            }
            catch (Exception exception)
            {

                validationErrors.Add($"Failed to register interface {interfaceToUse} with concrete type {type}. Exception that was caught said: [{exception.Message}]", exception);
            }
        }

        private static Type InterfaceToUse(IEnumerable<Type> interfaces, Type type, ValidationErrors validationErrors)
        {
            string foundInterfaces = string.Join(", ", interfaces.Select(i => i.ToString()));

            IList<Type> interfacesOfSameNamespace =
                interfaces.Where(inter => inter.Namespace != null && inter.Namespace.Equals(type.Namespace))
                    .ToList();

            if (interfacesOfSameNamespace.Count == 1)
            {
                var interfaceToUse = interfacesOfSameNamespace.First();

                LogWriter.WriteLine($"Registering interface [{interfaceToUse}] for type [{type}], because the namespace matches. but found multiple interfaces: {foundInterfaces}");
                return interfaceToUse;
            }

            validationErrors.Add($"Multiple interfaces found for [{type}] found: {foundInterfaces}");

            return null;
        }
    }
}