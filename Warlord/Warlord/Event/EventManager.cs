using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warlord.Event
{
    interface EventManager
    {
        void Subscribe( EventSubscriber listener, String eventType );
        void SendEvent( GameEvent theEvent );        
        void SendDelayedEvents( int currentTime );
    }
}
