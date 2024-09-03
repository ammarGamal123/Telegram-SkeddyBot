using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_SkeddyBot.Core.Handlers
{
    /// <summary>
    /// Provides methods to validate event messages and schedule times.
    /// </summary>
    public class ValidationHander
    {
        /// <summary>
        /// Validates an event message to ensure it is not null or whitespace.
        /// </summary>
        /// <param name="message">The event message to validate.</param>
        /// <returns>True if the message is valid; otherwise, false.</returns>
        public bool ValidateEventMessage(string message)
        {
            // Add logic to validate event messages
            return !string.IsNullOrWhiteSpace(message);
        }

        /// <summary>
        /// Validates a schedule time by parsing the input and checking if it is a future date.
        /// </summary>
        /// <param name="input">The input string to validate as a schedule time.</param>
        /// <param name="scheduleTime">The parsed DateTime if validation succeeds; otherwise, default(DateTime).</param>
        /// <returns>True if the input is a valid future DateTime; otherwise, false.</returns>
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
