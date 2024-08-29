using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_SkeddyBot.Core.Handlers
{
    public class ValidationHander
    {
        public bool ValidateEventMessage(string message)
        {
            // Add logic to validate event messages
            return !string.IsNullOrWhiteSpace(message);
        }
        public bool ValidateScheduleTime(string input, out DateTime scheduleTime)
        {
            // First, try to parse the input into a DateTime
            bool isValid = DateTime.TryParse(input, out scheduleTime);

            // Check if the parsed DateTime is in the future
            if (isValid && scheduleTime > DateTime.Now)
            {
                return true;
            }

            // If parsing fails or the date is in the past, return false
            return false;
        }

    }
}
