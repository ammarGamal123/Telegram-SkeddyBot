using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram_SkeddyBot.Core.Contracts;

namespace Telegram_SkeddyBot.Core.Handlers
{
    public class CommandHanlder
    {
        private readonly ITelegramBotClient _botClient;

        public CommandHanlder(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        public async Task HandleCommandAsync(Message message)
        {
            if (message.Type != MessageType.Text)
            {
                return;
            }

            var text = message.Text;

            if (text == null) {
                return;
            }

            switch (text.Split(' ')[0])
            {
                case "/start":
                    await _botClient.SendTextMessageAsync(message.Chat.Id, "Welcome to Skeddybot!");
                    break;
                case "/help":
                    string helpMessage = "I can help you create and manage your reminders, which I then send to you via Telegram at specified times.\n\n" +
                                         "Here is the list of my key features:\n\n" +
                                         "- You can create simple reminders using natural language, just sending me messages like: \"check your email in 20 minutes\", \"congratulate John🎂 tomorrow at 10am\", \"check the pie every 10 minutes\", etc.\n\n" +
                                         "- Also, I have a very convenient set of predefined inline buttons in sent reminders, which you can use to tell me to remind you again at another time. Also, you can enter this time manually, if you want.\n\n" +
                                         "- You can disable any reminder if you don't need it for the time. When you need it again, just enable it.\n\n" +
                                         "- You can create not only reminders but also just notes without specifying sending time. For example, you can have your shopping list saved as a note in me. To create a note, send me some text and then click \"📝Save as note\".\n\n" +
                                         "- You can convert passed reminders to notes, or you can reschedule them. And you can convert notes to reminders also.\n\n" +
                                         "And here is the full list of commands:\n" +
                                         "/start - start using bot/go to main menu\n" +
                                         "/help - open help\n" +
                                         "/add - add new reminder\n" +
                                         "/list - get a list of your reminders\n" +
                                         "/formats - show information about supported date and time formats\n" +
                                         "/settings - change bot settings\n" +
                                         "/web - generate link to bot Web-interface\n" +
                                         "/cancel - cancel the current operation\n\n" +
                                         "If you like ❤️ how I do my job, please hit the Like button here: https://telegramic.org/bot/skeddybot. This is very important to me. Thanks!\n\n" +
                                         "Feedback: @SkeddySupport\n\n" +
                                         "And do not forget to subscribe to my channel :) https://t.me/skeddy_news";
                    await _botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: helpMessage,
                            parseMode: ParseMode.Html
                        );
                    break;

                default:
                    await _botClient.SendTextMessageAsync(message.Chat.Id, "Unknown command.");
                    break;
            }
        }
    }
}
