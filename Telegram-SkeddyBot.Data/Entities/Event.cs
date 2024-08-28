using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_SkeddyBot.Data.Entities
{
    public class Event
    {
        public string EventDetails { get;set; }

        public DateTime ScheduleTime { get; set; }
    }
}
