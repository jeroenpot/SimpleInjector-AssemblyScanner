using System;
using System.Collections.Generic;

namespace JeroenPot.SimpleInjector.AssemblyScanner
{
    /// <summary>
    /// Container of validation errors.
    /// </summary>
    public class ValidationErrors
    {
        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public IList<ValidationError> Errors { get; } = new List<ValidationError>();

        /// <summary>
        /// Adds the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Add(string message)
        {
            Errors.Add(new ValidationError() {ErrorMessage = message});
        }

        /// <summary>
        /// Adds the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Add(string message, Exception exception)
        {
            Errors.Add(new ValidationError() { ErrorMessage = message, Exception = exception});
        }
    }
}