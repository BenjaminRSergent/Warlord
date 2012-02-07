using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Warlord.Logic.Data.Entity;
using Warlord.GameTools;

namespace Warlord.Event.EventTypes
{
    class ActorRemovedData
    {
        public uint actorID;
        public Entity actor;
        public Vector3 location;

        public ActorRemovedData(uint actorID, Entity actor, Vector3 location)
        {
            this.actorID = actorID;
            this.actor = actor;
            this.location = location;
        }        
    }

    class ActorRemovedEvent 
        : BaseGameEvent
    {
        ActorRemovedData data;        

        public ActorRemovedEvent( Optional<Object> sender, int delay, ActorRemovedData data )
            : base(sender, "actor_removed", delay)
        {
            this.data = data;
        }

        public ActorRemovedData Data
        {
            get { return data; }
        }
    }
}
