using System.Diagnostics;

namespace JeroenPot.SimpleInjector.AssemblyScanner
{
    /// <summary>
    /// Log implementation that uses <see cref="Debug"/> to write lines.
    /// </summary>
    public class DebugWriter : ILogWriter
    {
        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="line">The line.</param>
        public void WriteLine(string line)
        {
            Debug.WriteLine(line);
        }
    }
}