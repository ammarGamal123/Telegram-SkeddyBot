using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_SkeddyBot.Infrastructure.Logging
{
    public class ConsoleLogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
