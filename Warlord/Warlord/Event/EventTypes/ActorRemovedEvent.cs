using System;
using Warlord.GameTools;
using Warlord.Logic.Data.Entity;

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
