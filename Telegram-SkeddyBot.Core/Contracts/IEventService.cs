using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram_SkeddyBot.Data.Entities;

namespace Telegram_SkeddyBot.Core.Contracts
{
    public interface IEventService
    {
        Task ScheduleEventAsync(string eventDetails, DateTime scheduleTime);

        Task<IEnumerable<Event>> GetUpcomingEventsAsync();

    }
}
