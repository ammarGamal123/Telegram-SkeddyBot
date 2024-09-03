using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram_SkeddyBot.Core.Handlers
{
    /// <summary>
    /// Provides functionality to send text messages using the Telegram bot client.
    /// </summary>
    public class MessageHandler
    {
        /// <summary>
        /// Sends a text message to a specified chat using the Telegram bot client.
        /// </summary>
        /// <param name="botClient">The Telegram bot client instance.</param>
        /// <param name="chatId">The ID of the chat where the message will be sent.</param>
        /// <param name="text">The content of the message to be sent.</param>
        /// <param name="replyMarkup">Optional inline keyboard markup to be included with the message.</param>
        /// <param name="cancellationToken">Cancellation token for async operations.</param>
        /// <returns>A task representing the asynchronous send operation.</returns>
        public async Task SendTextMessageAsync(ITelegramBotClient botClient, long chatId, string text, InlineKeyboardMarkup replyMarkup, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: text,
                replyMarkup: replyMarkup,
                cancellationToken: cancellationToken
            );
        }
    }
}
