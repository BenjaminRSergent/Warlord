using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warlord.GameTools;

namespace Warlord.Event
{
    class Event
    {       
        Event( Optional<Object> sender )
        {
            Sender = sender;
        }

        public String EventType{ get; protected set; }
        public Optional<Object> Sender{ get; protected set; }

        public Optional<List<Event>> NextEvents{ get; protected set; }
    }
}
