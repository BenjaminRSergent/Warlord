using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warlord.Event
{
    delegate void EventReaction( object eventObject );
    interface EventManager
    {
        void Subscribe( EventReaction eventReaction, String eventType );
        void SendEvent( GameEvent theEvent );        
        void SendDelayedEvents( int currentTime );
    }
}
