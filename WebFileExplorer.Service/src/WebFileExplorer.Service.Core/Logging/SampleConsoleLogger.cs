using System;
using WebFileExplorer.Service.Core.Logging.Interfaces;

namespace WebFileExplorer.Service.Core.Logging
{
    /// <summary>
    /// I'm too lazy to find and configure normal logger. I think it will be enought for this project. Very hardcoded
    /// </summary>
    public class SampleConsoleLogger : ILogger
    {
        private static readonly object SynchronizationContext = new object();

        public void LogDebug(string message) => WriteMessage("DEBUG", message, ConsoleColor.Yellow);
        public void LogError(string message) => WriteMessage("ERROR", message, ConsoleColor.Red);
        public void LogOk(string message) => WriteMessage("OK", message, ConsoleColor.Green);

        private static void WriteMessage(string type, string message, ConsoleColor consoleColor)
        {
            lock (SynchronizationContext)
            {
                Console.Write("[");
                Console.ForegroundColor = consoleColor;
                Console.Write(type);
                Console.ResetColor();
                Console.WriteLine($"] {DateTime.Now} {message}");
            }
        }
    }
}
