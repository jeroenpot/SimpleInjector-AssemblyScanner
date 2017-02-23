using System;
using System.Collections.Generic;

namespace JeroenPot.SimpleInjector.AssemblyScanner
{
    public class ValidationErrors
    {
        public IList<ValidationError> Errors { get; } = new List<ValidationError>();

        public void Add(string message)
        {
            Errors.Add(new ValidationError() {ErrorMessage = message});
        }

        public void Add(string message, Exception exception)
        {
            Errors.Add(new ValidationError() { ErrorMessage = message, Exception = exception});
        }
    }
}