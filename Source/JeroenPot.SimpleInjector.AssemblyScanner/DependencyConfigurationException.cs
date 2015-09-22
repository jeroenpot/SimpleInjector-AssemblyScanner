using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace JeroenPot.SimpleInjector.AssemblyScanner
{
    /// <summary>
    /// Exception that is thrown when registration fails.
    /// </summary>
    [Serializable]
    public class DependencyConfigurationException : Exception
    {
        /// <summary>
        /// Gets the validation errors.
        /// </summary>
        /// <value>
        /// The validation errors.
        /// </value>
        public IList<string> ValidationErrors { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyConfigurationException"/> class.
        /// </summary>
        public DependencyConfigurationException()
        {
            ValidationErrors = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyConfigurationException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DependencyConfigurationException(string message) : base(message)
        {
            ValidationErrors = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyConfigurationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="validationErrors">The validation errors.</param>
        public DependencyConfigurationException(string message, IList<string> validationErrors)
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
            ValidationErrors = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyConfigurationException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected DependencyConfigurationException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
            ValidationErrors = new List<string>();
        }

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown. </param><param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination. </param><exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is a null reference (Nothing in Visual Basic). </exception>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("ValidationErrors", ValidationErrors);
        }
    }
}