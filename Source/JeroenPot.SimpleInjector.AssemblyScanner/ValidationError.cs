using System;

namespace JeroenPot.SimpleInjector.AssemblyScanner
{
    public class ValidationError
    {
        public string ErrorMessage { get; set; }
        public Exception Exception { get; set; }
    }
}