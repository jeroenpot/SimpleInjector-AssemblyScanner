using System;

namespace JeroenPot.SimpleInjector.AssemblyScanner
{
    /// <summary>
    /// Exception that is thrown when registration fails.
    /// </summary>
    public class DependencyConfigurationException : Exception
    {
        /// <summary>
        /// Gets the validation errors.
        /// </summary>
        /// <value>
        /// The validation errors.
        /// </value>
        public ValidationErrors ValidationErrors { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyConfigurationException"/> class.
        /// </summary>
        public DependencyConfigurationException()
        {
            ValidationErrors = new ValidationErrors();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyConfigurationException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DependencyConfigurationException(string message) : base(message)
        {
            ValidationErrors = new ValidationErrors();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyConfigurationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="validationErrors">The validation errors.</param>
        public DependencyConfigurationException(string message, ValidationErrors validationErrors)
            : base(message)
        {
            ValidationErrors = validationErrors;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyConfigurationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public DependencyConfigurationException(string message, Exception inner) : base(message, inner)
        {
            ValidationErrors = new ValidationErrors();
        }
    }
}