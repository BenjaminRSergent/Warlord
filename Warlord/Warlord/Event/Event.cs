using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warlord.GameTools;

namespace Warlord.Event
{
    class Event
    {       
        public Event( Optional<Object> sender, String eventType, int delay )
        {
            EventType = eventType;
            Sender = sender;
            Delay = delay;
        }

        public int Delay{ get; protected set; }

        public String EventType{ get; protected set; }
        public Optional<Object> Sender{ get; protected set; }

        public Optional<List<Event>> NextEvents{ get; protected set; }
    }
}
