using System;

namespace Telegram_SkeddyBot.Infrastructure.Logging
{
    /// <summary>
    /// Provides a simple implementation of a logger that writes log messages to the console.
    /// </summary>
    public class ConsoleLogger
    {
        /// <summary>
        /// Logs a message to the console.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
