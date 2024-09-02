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
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTelegramBotServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Read the Token from the configuration
            var botToken = configuration.GetSection("BotConfiguration")
                                        .GetValue<string>("Token");

            if (string.IsNullOrEmpty(botToken))
            {
                throw new ArgumentNullException(nameof(botToken), "BotConfiguration or Token is not configured properly.");
            }

            // Register the TelegramBotClient with the Token from configuration
            services.AddSingleton<ITelegramBotClient>(sp =>
            {
                return new TelegramBotClient(botToken);
            });

            // Register the core services
            services.AddSingleton<ITelegramBotService, TelegramBotService>();


            // Register the CommandHandler from Core
            services.AddTransient<CommandHandler>();
            services.AddSingleton<UserStateHandler>();
            services.AddSingleton<ValidationHander>();
            services.AddSingleton<MessageHandler>();


            // Configure BotConfiguration settings
            services.Configure<BotConfiguration>(options =>
            {
                options.Token = botToken;
                options.WebHookUrl = configuration
                       .GetSection("BotConfiguration").GetValue<string>("WebHookUrl");
            });

            return services;
        }
    }
}
