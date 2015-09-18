using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SimpleInjector.AssemblyScanner
{
    /// <summary>
    /// Static helper class for scanning of assembly and automatic registration of classes and interfaces in the SimpleInjector container.
    /// </summary>
    public static class DependencyRegistration
    {
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
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            string assemblyName = assembly.GetName().Name;
            IList<Type> registeredInterfaces = new List<Type>();
            var existingRegistrationsServiceTypes = container.GetCurrentRegistrations().Select(instanceProducer => instanceProducer.ServiceType).ToList();

            IList<Type> registrations =
                assembly.GetExportedTypes()
                    .Where(type => !typesToIgnore.Contains(type))
                    .Where(type => !existingRegistrationsServiceTypes.Contains(type))
                    .Where(type => type.Namespace != null)
                    .Where(type => type.Namespace.StartsWith(assemblyName, StringComparison.OrdinalIgnoreCase))
                    .Where(type => type.GetInterfaces().Any())
                    .Where(type => type.GetInterfaces().Any(inter => !typesToIgnore.Contains(inter) && inter.Namespace != null && inter.Namespace.StartsWith(assemblyName)))
                    .ToList();


            // Ignore already registerd interfaces:
            for (int i = registrations.Count() - 1; i >= 0; i--)
            {
                foreach (var registrationInterface in registrations[i].GetInterfaces())
                {
                    if (existingRegistrationsServiceTypes.Contains(registrationInterface))
                    {
                        registrations.RemoveAt(i);
                        break;
                    }
                }
            }

            IList<string> validationErrors = new List<string>();

            foreach (Type type in registrations)
            {
                Type[] interfaces = type.GetInterfaces();
                if (interfaces.Length > 1)
                {
                    string foundInterfaces = string.Join(", ", interfaces.Select(i => i.ToString()));
                    validationErrors.Add(string.Format("Multiple interfaces found for [{0}] found: {1}", type, foundInterfaces));
                }
                else
                {
                    Type interfaceType = type.GetInterfaces().Single();

                    if (registeredInterfaces.Contains(interfaceType))
                    {
                        validationErrors.Add(string.Format("Multiple Implementations found for [{0}]", interfaceType));
                    }
                    else
                    {
                        string message = string.Format("Registering interface [{0}] with concrete implementation [{1}]", interfaceType, type);

                        LogWriter.WriteLine(message);
                        container.Register(interfaceType, type, Lifestyle.Transient);
                        registeredInterfaces.Add(interfaceType);
                    }
                }
            }

            if (validationErrors.Any())
            {
                throw new DependencyConfigurationException(string.Join(Environment.NewLine, validationErrors), validationErrors);
            }
        }
    }
}