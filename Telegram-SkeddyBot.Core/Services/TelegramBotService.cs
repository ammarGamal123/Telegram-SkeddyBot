﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram_SkeddyBot.Core.Contracts;
using Telegram_SkeddyBot.Core.Handlers;
using Telegram_SkeddyBot.Core.Helpers;

namespace Telegram_SkeddyBot.Core.Services
{
    /// <summary>
    /// Provides services for managing and running the Telegram bot.
    /// </summary>
    public class TelegramBotService : ITelegramBotService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly CommandHandler _commandHandler;
        private readonly UserStateHandler _userStateHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="TelegramBotService"/> class.
        /// </summary>
        /// <param name="botClient">The bot client used to interact with the Telegram API.</param>
        /// <param name="commandHandler">The handler for processing bot commands.</param>
        /// <param name="userStateHandler">The handler for managing user states.</param>
        public TelegramBotService(ITelegramBotClient botClient, CommandHandler commandHandler, UserStateHandler userStateHandler)
        {
            _botClient = botClient;
            _commandHandler = commandHandler;
            _userStateHandler = userStateHandler;
        }

        /// <summary>
        /// Starts the bot and begins receiving updates from Telegram.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        public async Task StartBotAsync(CancellationToken cancellationToken)
        {
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { } // Receive all update types
            };

            _botClient.StartReceiving(
                HandleUpdateAsync,
                ErrorHandler.HandleErrorAsync, // Correct method signature
                receiverOptions
            );

            var botDetails = await _botClient.GetMeAsync();
            Console.WriteLine($"Bot is running...\nName: {botDetails.FirstName}\nUsername: {botDetails.Username}\nID: {botDetails.Id}");

            await Task.Delay(Timeout.Infinite, cancellationToken);
        }

        /// <summary>
        /// Handles incoming updates from Telegram.
        /// </summary>
        /// <param name="botClient">The bot client used to interact with the Telegram API.</param>
        /// <param name="update">The update received from Telegram.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update.Message?.From != null)
            {
                var message = update.Message.Text;
                var user = update.Message.From;
                var userId = user.Id;

                Console.WriteLine($"Command: {message}\nUser: {user}\nUser ID: {userId}");

                if (_userStateHandler.GetUserState(userId) != UserState.None)
                {
                    await _commandHandler.HandleAddCommandAsync(botClient, update.Message.Chat.Id, userId, message, cancellationToken);
                }
                else if (message != null)
                {
                    switch (message)
                    {
                        case "/start":
                            await _commandHandler.HandleStartCommandAsync(botClient, update.Message.Chat.Id, cancellationToken);
                            break;
                        case "/help":
                            await _commandHandler.HandleHelpCommandAsync(botClient, update.Message.Chat.Id, cancellationToken);
                            break;
                        case "/add":
                            await _commandHandler.HandleAddCommandAsync(botClient, update.Message.Chat.Id, userId, message, cancellationToken);
                            break;
                        case "/list":
                            await _commandHandler.HandleListCommandAsync(botClient, update.Message.Chat.Id, userId, cancellationToken);
                            break;
                        case "/delete":
                            await _commandHandler.HandleDeleteCommandAsync(botClient, update.Message.Chat.Id, userId, message, cancellationToken);
                            break;

                        default:
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Unknown command. Type /help to see available commands.", cancellationToken: cancellationToken);
                            break;
                    }
                }
            }
            else if (update.Type == UpdateType.CallbackQuery)
            {
                await _commandHandler.HandleCallbackQueryAsync(botClient, update.CallbackQuery, cancellationToken);
            }
        }
    }
}
