using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warlord.GameTools;

namespace Warlord.Event
{
    class BaseGameEvent
    {
        private int hash;

        public BaseGameEvent(Optional<Object> sender, String eventType, int delay)
        {
            Sender = sender;
            EventType = eventType;
            Delay = delay;


            hash = eventType.GetHashCode();
        }

        public int Delay { get; private set; }
        public String EventType { get; private set; }
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
        static public bool operator ==(BaseGameEvent lhs, BaseGameEvent rhs)
        {
            return lhs.hash == rhs.hash;
        }
        static public bool operator !=(BaseGameEvent lhs, BaseGameEvent rhs)
        {
            return lhs.hash != rhs.hash;
        }
    }
}
