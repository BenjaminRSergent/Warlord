using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warlord.GameTools;
using Microsoft.Xna.Framework;

namespace Warlord.Event.EventTypes
{
    class ActorCreatedData
    {
        public uint actorID;
        public Vector3 location;

        public ActorCreatedData(uint actorID, Vector3 location)
        {
            this.actorID = actorID;
            this.location = location;
        }        
    }

    class ActorCreatedEvent : BaseGameEvent
    {
        ActorCreatedData data;        

        public ActorCreatedEvent( Optional<Object> sender, int delay, ActorCreatedData data )
            : base(sender, "actor_created", delay)
        {
            this.data = data;
        }

        public ActorCreatedData Data
        {
            get { return data; }
        }
    }
}

