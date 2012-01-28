using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warlord.GameTools;

namespace Warlord.Event
{
    interface EventSubscriber
    {       
        void HandleEvent( GameEvent theEvent );
    }
}
