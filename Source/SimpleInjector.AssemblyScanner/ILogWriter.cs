namespace SimpleInjector.AssemblyScanner
{
    /// <summary>
    /// Interface for writing logmessage in this library.
    /// </summary>
    public interface ILogWriter
    {
        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="line">The line.</param>
        void WriteLine(string line);
    }
}