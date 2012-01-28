using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warlord.Event;

namespace WarlordUnitTest
{
    class DummySubscriber : EventSubscriber
    {
        public bool RecievedEvent{ get; private set; }
        public GameEvent TheEvent{ get; private set; }

        public DummySubscriber( EventManager theManager )
        {
            theManager.Subscribe(this, "actor_moved");
        }

        public void HandleEvent(GameEvent theEvent)
        {
            RecievedEvent = true;
            TheEvent = theEvent;
        }
    }
}
