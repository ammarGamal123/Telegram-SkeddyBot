using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_SkeddyBot.Core.Contracts
{
    public interface ITelegramBotService
    {
        Task StartBotAsync(CancellationToken cancellationToken);
    }
}
