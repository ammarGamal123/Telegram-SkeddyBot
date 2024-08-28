using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram_SkeddyBot.Core.Contracts;

namespace Telegram_SkeddyBot.Core.Services
{
    public class TelegramBotService : ITelegramBotService
    {
        private readonly ITelegramBotClient _botClient;

        public TelegramBotService(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        public async Task StartBotAsync(CancellationToken cancellationToken)
        {
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { } // Receive all update types
            };

            _botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions
            );

            var botDetails = await _botClient.GetMeAsync();
            Console.WriteLine($"Bot is running...\nName: {botDetails.FirstName}\nUsername: {botDetails.Username}\nID: {botDetails.Id}");

            // Keep the bot running until cancellation is requested
            await Task.Delay(Timeout.Infinite, cancellationToken);
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update.Message?.From != null)
            {
                var message = update.Message.Text;
                var user = update.Message.From;

                // Log user details
                Console.WriteLine($"User Details:");
                Console.WriteLine($"User ID: {user.Id}");
                Console.WriteLine($"Username: {user.Username}");
                Console.WriteLine($"Command {message}");
                switch (message.ToLower())
                {
                    case "/start":
                        await HandleStartCommandAsync(botClient, update.Message.Chat.Id, cancellationToken);
                        break;

                    case "/help":
                        await HandleHelpCommandAsync(botClient, update.Message.Chat.Id, cancellationToken);
                        break;

                    // Add other commands here

                    default:
                        Console.WriteLine($"Received unknown command: {message}");
                        break;
                }
            }
        }


        private async Task HandleHelpCommandAsync(ITelegramBotClient botClient,
                                                  long chatId,
                                                  CancellationToken cancellationToken)
        {
            string helpMessage = GetHelpMessage();
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: helpMessage,
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken
            );
        }
        private async Task HandleStartCommandAsync(ITelegramBotClient botClient,
                                                   long chatId,
                                                   CancellationToken cancellationToken)
        {
            string welcomeMessage = "Welcome to the bot! Use the following commands to interact:\n" +
        "/add - add new reminder\n" +
        "/list - get a list of your reminders\n" +
        "/formats - show information about supported date and time formats\n" +
        "/settings - change bot settings\n" +
        "/web - generate link to bot Web-interface\n" +
        "/cancel - cancel the current operation\n\n";


            _botClient.SendTextMessageAsync(
                    chatId:chatId,
                    text:welcomeMessage,
                    parseMode: ParseMode.Html,
                    cancellationToken: cancellationToken
                );
        }



        private string GetHelpMessage()
        {
            return @"
I can help you create and manage your reminders, which then I send to you via Telegram at the specified time.

Here is the list of my key features:

- You can create simple reminders using natural language, just send me messages like: ""check your email in 20 minutes"", etc.
- I have a convenient set of predefined inline buttons in sent reminders, which you can use to reschedule or disable reminders.
- You can create notes without specifying a time, just send me some text and click ""📝Save as note"".

And here is the full list of commands:

/start - start using the bot/go to the main menu
/help - open help
/add - add new reminder
/list - get a list of your reminders
/formats - show information about supported date and time formats
/settings - change bot settings
/web - generate a link to the bot Web-interface
/cancel - cancel the current operation";
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }
    }
}
