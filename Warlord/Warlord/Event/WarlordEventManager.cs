using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warlord.Event
{
    class WarlordEventManager : EventManager
    {
        private HashSet<String> validEvents;

        public WarlordEventManager( )
        {
            ValidEventInitializer.SetValidEvents( validEvents );
        }


        public void Subscribe(EventSubscriber listener, Event theEvent)
        {
            throw new NotImplementedException();
        }

        public void FireEvent(Event theEvent)
        {
            throw new NotImplementedException();
        }
    }
}
