using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram_SkeddyBot.Core.Handlers
{
    public class MessageHandler
    {
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
