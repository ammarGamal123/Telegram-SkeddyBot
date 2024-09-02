using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram_SkeddyBot.Core.Handlers;
using Telegram_SkeddyBot.Core.Contracts;

namespace Telegram_SkeddyBot.Core.Helpers
{
    public class CommandHandler
    {
        private readonly UserStateHandler _userStateHandler;
        private readonly MessageHandler _messageHandler;
        private readonly ValidationHander _validationHandler;

        public CommandHandler(UserStateHandler userStateHandler, MessageHandler messageHandler, ValidationHander validationHandler)
        {
            _userStateHandler = userStateHandler;
            _messageHandler = messageHandler;
            _validationHandler = validationHandler;
        }

        public async Task HandleStartCommandAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
        {
            string welcomeMessage = "Welcome to the bot! Use the following commands to interact:\n" +
                                    "/help - to help you with bot and commands\n" +
                                    "/add - add new reminder\n" +
                                    "/list - get a list of your reminders\n";

            await _messageHandler.SendTextMessageAsync(botClient, chatId, welcomeMessage, null, cancellationToken);
        }

        public async Task HandleHelpCommandAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
        {
            string helpMessage = GetHelpMessage();
            await _messageHandler.SendTextMessageAsync(botClient, chatId, helpMessage, null, cancellationToken);
        }

        public async Task HandleAddCommandAsync(ITelegramBotClient botClient, long chatId, long userId, string message, CancellationToken cancellationToken)
        {
            var state = _userStateHandler.GetUserState(userId);

            if (state == UserState.None)
            {
                _userStateHandler.SetUserState(userId, UserState.ExpectingEventMessage);
                await _messageHandler.SendTextMessageAsync(botClient, chatId, "Please enter the event message.", null, cancellationToken);
            }
            else if (state == UserState.ExpectingEventMessage)
            {
                if (_validationHandler.ValidateEventMessage(message))
                {
                    var inlineKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Edit", "edit_event"),
                            InlineKeyboardButton.WithCallbackData("Continue", "continue_event")
                        }
                    });

                    _userStateHandler.StoreEventMessage(userId, message);
                    _userStateHandler.SetUserState(userId, UserState.ExpectingScheduleTime);
                    await _messageHandler.SendTextMessageAsync(botClient, chatId, "Please review the event message and choose an option:", inlineKeyboard, cancellationToken);
                }
                else
                {
                    await _messageHandler.SendTextMessageAsync(botClient, chatId, "Invalid event message. Please enter a valid event message.", null, cancellationToken);
                }
            }
            else if (state == UserState.ExpectingScheduleTime)
            {
                if (_validationHandler.ValidateScheduleTime(message, out var scheduleTime))
                {
                    var inlineKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Edit", "edit_schedule"),
                            InlineKeyboardButton.WithCallbackData("Continue", "continue_schedule")
                        }
                    });

                    _userStateHandler.StoreUserEvent(userId, scheduleTime);
                    await _messageHandler.SendTextMessageAsync(botClient, chatId, "Please review the schedule time and choose an option:", inlineKeyboard, cancellationToken);
                }
                else
                {
                    await _messageHandler.SendTextMessageAsync(botClient, chatId, "Invalid time format. Please enter the schedule time again.", null, cancellationToken);
                }
            }
        }

        private string GetHelpMessage()
        {
            return @"
                I can help you create and manage your reminders, which I will then send to you via Telegram at the specified time.
                Here is the list of my key features:

                - You can create simple reminders using natural language; just send me messages like: ""check your email in 20 minutes"", etc.
                - I have a convenient set of predefined inline buttons in sent reminders, which you can use to reschedule or disable reminders.
                - You can create notes without specifying a time; just send me some text and click ""📝Save as note"".

                And here is the full list of commands:

                /start - start using the bot/go to the main menu
                /help - open help
                /add - add new reminder
                /list - get a list of your reminders";
        }

        public async Task HandleListCommandAsync(ITelegramBotClient botClient, long chatId, long userId, CancellationToken cancellationToken)
        {
            var userEvents = _userStateHandler.GetUserEvents(userId);
            if (userEvents.Any())
            {
                var message = "Your upcoming events:\n";
                foreach (var userEvent in userEvents)
                {
                    message += $"- Event: {userEvent.eventMessage}\n" +
                               $"- Scheduled Time: {userEvent.scheduleTime.ToString("yyyy-MM-dd HH:mm")}\n\n";
                }
                await _messageHandler.SendTextMessageAsync(botClient, chatId, message, null, cancellationToken);
            }
            else
            {
                await _messageHandler.SendTextMessageAsync(botClient, chatId, "You have no upcoming events.", null, cancellationToken);
            }
        }

        public async Task HandleCallbackQueryAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            var userId = callbackQuery.From.Id;
            var chatId = callbackQuery.Message.Chat.Id;
            var data = callbackQuery.Data;

            switch (data)
            {
                case "edit_event":
                    _userStateHandler.SetUserState(userId, UserState.ExpectingEventMessage);
                    await _messageHandler.SendTextMessageAsync(botClient, chatId, "Please enter the event message again.", null, cancellationToken);
                    break;

                case "continue_event":
                    _userStateHandler.SetUserState(userId, UserState.ExpectingScheduleTime);
                    await _messageHandler.SendTextMessageAsync(botClient, chatId, "Please enter the schedule time in the future (e.g., 2024-09-01 14:00).", null, cancellationToken);
                    break;

                case "edit_schedule":
                    _userStateHandler.SetUserState(userId, UserState.ExpectingScheduleTime);
                    await _messageHandler.SendTextMessageAsync(botClient, chatId, "Please enter the schedule time again.", null, cancellationToken);
                    break;

                case "continue_schedule":
                    await _messageHandler.SendTextMessageAsync(botClient, chatId, "Event scheduled successfully!", null, cancellationToken);
                    _userStateHandler.ClearUserState(userId);
                    break;

                case var deleteData when deleteData.StartsWith("delete "):
                    var eventIndex = int.Parse(deleteData.Substring(7));
                    _userStateHandler.DeleteUserEvent(userId, eventIndex);
                    await _messageHandler.SendTextMessageAsync(botClient, chatId, "Event deleted successfully!", null, cancellationToken);
                    _userStateHandler.ClearUserState(userId);
                    break;

                default:
                    await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Unknown action", cancellationToken: cancellationToken);
                    break;
            }

            // Ensure the callback query is answered
            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, cancellationToken: cancellationToken);
        }


        public async Task HandleDeleteCommandAsync(ITelegramBotClient botClient, long chatId, long userId, string message, CancellationToken cancellationToken)
        {
            // Step 1: If the list is empty, notify the user
            var userEvents = _userStateHandler.GetUserEvents(userId);

            if (!userEvents.Any())
            {
                await _messageHandler.SendTextMessageAsync
                    (botClient, chatId, "You have no Events yet", null, cancellationToken);
                return;
            }

            // Step 2: If there are events, display them with inline keyboard options
            var inlineKeyboardsButtons = userEvents.Select((userEvent, index) =>
            InlineKeyboardButton.WithCallbackData($"{userEvent.eventMessage} at {userEvent.scheduleTime} ", $"delete {index}"))
                .ToArray();

            var inlineKeyboard = new InlineKeyboardMarkup(inlineKeyboardsButtons);

            await _messageHandler.SendTextMessageAsync(botClient, chatId, "Please select event to delete", inlineKeyboard, cancellationToken);
        }

    }
}
