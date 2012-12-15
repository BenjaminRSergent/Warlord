using System;
using System.Collections.Generic;
using GameTools;

namespace Warlord.Event
{
    class BaseGameEvent
    {
        private int hash;

        protected BaseGameEvent(Optional<Object> sender, string eventType, int delay)
        {
            Sender = sender;
            EventType = eventType;
            Delay = delay;

            hash = eventType.GetHashCode();
        }

        public int Delay { get; private set; }
        public string EventType { get; private set; }
        public Optional<Object> Sender { get; private set; }
        public Optional<List<BaseGameEvent>> NextEvents { get; private set; }

        public override int GetHashCode()
        {
            return hash;
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public static bool operator ==(BaseGameEvent lhs, BaseGameEvent rhs)
        {
            return lhs.hash == rhs.hash;
        }
        public static bool operator !=(BaseGameEvent lhs, BaseGameEvent rhs)
        {
            return lhs.hash != rhs.hash;
        }
    }
}
