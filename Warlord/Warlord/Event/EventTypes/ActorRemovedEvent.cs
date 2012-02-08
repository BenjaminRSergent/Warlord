using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Warlord.Logic.Data.Entity;
using Warlord.GameTools;

namespace Warlord.Event.EventTypes
{
    class ActorRemovedEvent
        : BaseGameEvent
    {
        GameEntity deadEntity;

        public ActorRemovedEvent(Optional<Object> sender, int delay, GameEntity deadEntity)
            : base(sender, "actor_removed", delay)
        {
            this.deadEntity = deadEntity;
        }

        public GameEntity DeadEntity
        {
            get { return deadEntity; }
        }
    }
}
