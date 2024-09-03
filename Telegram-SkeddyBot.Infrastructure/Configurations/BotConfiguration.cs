using System;

namespace Telegram_SkeddyBot.Infrastructure.Configurations
{
    /// <summary>
    /// Represents the configuration settings for the Telegram bot.
    /// </summary>
    public class BotConfiguration
    {
        /// <summary>
        /// Gets or sets the token used for authenticating the bot with the Telegram API.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the URL for the webhook used to receive updates from Telegram.
        /// </summary>
        public string WebHookUrl { get; set; }
    }
}
