using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram_SkeddyBot.Core.Contracts;
using Telegram_SkeddyBot.Core.Handlers;
using Telegram_SkeddyBot.Core.Helpers;
using Telegram_SkeddyBot.Core.Services;
using Telegram_SkeddyBot.Infrastructure.Configurations;

namespace Telegram_SkeddyBot.Api.Extensions
{
    /// <summary>
    /// Provides extension methods for IServiceCollection to register Telegram bot-related services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the necessary Telegram bot services and configurations to the IServiceCollection.
        /// </summary>
        /// <param name="services">The service collection to which the services will be added.</param>
        /// <param name="configuration">The application configuration from which settings are read.</param>
        /// <returns>The modified IServiceCollection with the registered services.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the bot token is not found in the configuration.</exception>
        public static IServiceCollection AddTelegramBotServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Read the Bot Token from the configuration file
            var botToken = configuration.GetSection("BotConfiguration")
                                        .GetValue<string>("Token");

            // Validate that the bot token is not null or empty
            if (string.IsNullOrEmpty(botToken))
            {
                throw new ArgumentNullException(nameof(botToken), "BotConfiguration or Token is not configured properly.");
            }

            // Register the TelegramBotClient as a singleton using the retrieved token
            services.AddSingleton<ITelegramBotClient>(sp =>
            {
                return new TelegramBotClient(botToken);
            });

            // Register the Telegram bot service for handling bot interactions
            services.AddSingleton<ITelegramBotService, TelegramBotService>();

            // Register command and message handlers for processing bot commands and messages
            services.AddTransient<CommandHandler>();
            services.AddSingleton<UserStateHandler>();
            services.AddSingleton<ValidationHander>();
            services.AddSingleton<MessageHandler>();

            // Configure BotConfiguration settings with the token and webhook URL
            services.Configure<BotConfiguration>(options =>
            {
                options.Token = botToken;
                options.WebHookUrl = configuration
                       .GetSection("BotConfiguration").GetValue<string>("WebHookUrl");
            });

            // Return the modified service collection
            return services;
        }
    }
}
