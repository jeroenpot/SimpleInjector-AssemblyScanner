using System;

namespace JeroenPot.SimpleInjector.AssemblyScanner
{
    /// <summary>
    /// Container for validation errors.
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public Exception Exception { get; set; }
    }
}