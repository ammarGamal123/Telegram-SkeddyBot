using System;
using System.Collections.Generic;
using System.Linq;

namespace Telegram_SkeddyBot.Core.Handlers
{
    /// <summary>
    /// Represents different states a user can be in during interactions with the bot.
    /// </summary>
    public enum UserState
    {
        /// <summary>
        /// The default state indicating no specific state.
        /// </summary>
        None,

        /// <summary>
        /// State indicating that the bot is expecting an event message from the user.
        /// </summary>
        ExpectingEventMessage,

        /// <summary>
        /// State indicating that the bot is expecting a schedule time from the user.
        /// </summary>
        ExpectingScheduleTime
    }

    /// <summary>
    /// Manages the state and events for users interacting with the bot.
    /// </summary>
    public class UserStateHandler
    {
        private readonly Dictionary<long, UserState> _userState = new();
        private readonly Dictionary<long, string> _eventMessages = new();
        private readonly Dictionary<long, List<(string eventMessage, DateTime scheduleTime)>> _userEvents = new();

        /// <summary>
        /// Gets the current state of the user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The current state of the user.</returns>
        public UserState GetUserState(long userId)
        {
            return _userState.TryGetValue(userId, out var state) ? state : UserState.None;
        }

        /// <summary>
        /// Sets the state of the user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="state">The state to set for the user.</param>
        public void SetUserState(long userId, UserState state)
        {
            _userState[userId] = state;
        }

        /// <summary>
        /// Clears the state of the user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        public void ClearUserState(long userId)
        {
            _userState.Remove(userId);
        }

        /// <summary>
        /// Stores the event message for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="eventMessage">The event message to store.</param>
        public void StoreEventMessage(long userId, string eventMessage)
        {
            _eventMessages[userId] = eventMessage;
        }

        /// <summary>
        /// Retrieves the stored event message for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The event message for the user, or null if not found.</returns>
        public string GetEventMessage(long userId)
        {
            return _eventMessages.TryGetValue(userId, out var message) ? message : null;
        }

        /// <summary>
        /// Stores an event with a specified schedule time for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="scheduleTime">The time the event is scheduled for.</param>
        public void StoreUserEvent(long userId, DateTime scheduleTime)
        {
            if (_eventMessages.TryGetValue(userId, out var message))
            {
                if (!_userEvents.ContainsKey(userId))
                {
                    _userEvents[userId] = new List<(string eventMessage, DateTime scheduleTime)>();
                }
                _userEvents[userId].Add((message, scheduleTime));
            }
        }

        /// <summary>
        /// Retrieves the list of events for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of events for the user.</returns>
        public List<(string eventMessage, DateTime scheduleTime)> GetUserEvents(long userId)
        {
            return _userEvents.TryGetValue(userId, out var events) ? events : new List<(string eventMessage, DateTime scheduleTime)>();
        }

        /// <summary>
        /// Deletes a specific event for a user based on the index in the list of events.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="index">The index of the event to delete.</param>
        public void DeleteUserEvent(long userId, int index)
        {
            if (_userEvents.ContainsKey(userId))
            {
                var events = _userEvents[userId];
                if (index >= 0 && index < events.Count)
                {
                    events.RemoveAt(index);
                    if (events.Count == 0)
                    {
                        _userEvents.Remove(userId);
                    }
                }
            }
        }
    }
}
