using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Warlord.GameTools;

namespace Warlord.Event.EventTypes
{
    class ActorMovedData
    {
        public uint actorID;
        public Vector3 oldLocation;
        public Vector3 newLocation;

        public ActorMovedData(uint actorID, Vector3 oldLocation, Vector3 newLocation)
        {
            this.actorID = actorID;
            this.oldLocation = oldLocation;
            this.newLocation = newLocation;
        }        
    }

    class ActorMovedEvent : BaseGameEvent
    {
        ActorMovedData data;        

        public ActorMovedEvent( Optional<Object> sender, int delay, ActorMovedData data )
            : base(sender, "actor_moved", delay)
        {
            this.data = data;
        }

        public ActorMovedData Data
        {
            get { return data; }
        }
    }
}
