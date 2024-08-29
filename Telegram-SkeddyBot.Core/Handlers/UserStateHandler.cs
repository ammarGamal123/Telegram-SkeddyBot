using System;
using System.Collections.Generic;
using System.Linq;

namespace Telegram_SkeddyBot.Core.Handlers
{
    public enum UserState
    {
        None,
        ExpectingEventMessage,
        ExpectingScheduleTime
    }

    public class UserStateHandler
    {
        private readonly Dictionary<long, UserState> _userState = new();
        private readonly Dictionary<long, string> _eventMessages = new();
        private readonly Dictionary<long, List<(string eventMessage, DateTime scheduleTime)>> _userEvents = new();

        public UserState GetUserState(long userId)
        {
            return _userState.TryGetValue(userId, out var state) ? state : UserState.None;
        }

        public void SetUserState(long userId, UserState state)
        {
            _userState[userId] = state;
        }

        public void ClearUserState(long userId)
        {
            _userState.Remove(userId);
        }

        public void StoreEventMessage(long userId, string eventMessage)
        {
            _eventMessages[userId] = eventMessage;
        }

        public string GetEventMessage(long userId)
        {
            return _eventMessages.TryGetValue(userId, out var message) ? message : null;
        }

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

        public List<(string eventMessage, DateTime scheduleTime)> GetUserEvents(long userId)
        {
            return _userEvents.TryGetValue(userId, out var events) ? events : new List<(string eventMessage, DateTime scheduleTime)>();
        }
    }
}
