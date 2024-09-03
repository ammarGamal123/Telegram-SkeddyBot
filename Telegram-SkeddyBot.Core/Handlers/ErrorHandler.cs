using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot;

namespace Telegram_SkeddyBot.Core.Handlers
{
    /// <summary>
    /// Provides a static method for handling errors that occur during bot operations.
    /// </summary>
    public static class ErrorHandler
    {
        /// <summary>
        /// Handles exceptions thrown by the Telegram bot client and logs the error messages.
        /// </summary>
        /// <param name="botClient">The Telegram bot client instance where the error occurred.</param>
        /// <param name="exception">The exception that was thrown.</param>
        /// <param name="cancellationToken">Cancellation token for async operations.</param>
        /// <returns>A completed task, indicating the error has been handled.</returns>
        public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Determine the error message based on the type of exception.
            var errorMessage = exception switch
            {
                // Handle Telegram API-specific errors.
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",

                // Handle other types of exceptions.
                _ => exception.ToString()
            };

            // Log the error message to the console.
            Console.WriteLine(errorMessage);

            // Return a completed task.
            return Task.CompletedTask;
        }
    }
}
