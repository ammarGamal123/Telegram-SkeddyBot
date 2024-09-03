using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Telegram_SkeddyBot.Core.Contracts
{
    /// <summary>
    /// Represents a contract for a service that manages the operations of a Telegram bot.
    /// </summary>
    public interface ITelegramBotService
    {
        /// <summary>
        /// Starts the Telegram bot asynchronously and begins processing incoming messages.
        /// </summary>
        /// <param name="cancellationToken">A token that can be used to request cancellation of the bot's operation.</param>
        /// <returns>A task that represents the asynchronous operation of starting the bot.</returns>
        Task StartBotAsync(CancellationToken cancellationToken);
    }
}
