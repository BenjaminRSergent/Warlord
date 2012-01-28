using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warlord.GameTools;

namespace Warlord.Event
{
    class GameEvent
    {
        public GameEvent( Optional<Object> sender, String eventType, object additionalData, int delay )
        {
            Sender = sender;
            EventType = eventType;            
            AdditionalData = additionalData;
            Delay = delay;
        }

        public int Delay{ get; protected set; }

        public String EventType{ get; protected set; }
        public Optional<Object> Sender{ get; protected set; }

        public object AdditionalData{ get; protected set; }

        public Optional<List<GameEvent>> NextEvents{ get; protected set; }
    }
}
