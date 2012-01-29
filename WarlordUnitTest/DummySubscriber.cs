using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warlord.Event;

namespace WarlordUnitTest
{
    class DummySubscriber
    {
        public bool RecievedEvent{ get; private set; }

        public DummySubscriber( EventManager theManager )
        {
            theManager.Subscribe(RecieveEvent, "actor_moved");
        }

        public void RecieveEvent(object theEvent, object data)
        {
            RecievedEvent = true;
        }
    }
}
