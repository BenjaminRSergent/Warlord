using System;
using Warlord.GameTools;
using Warlord.Logic.Data.Entity;

namespace Warlord.Event.EventTypes
{
    class ActorCreatedEvent : BaseGameEvent
    {
        GameEntity newEntity;

        public ActorCreatedEvent(Optional<Object> sender, int delay, GameEntity newEntity)
            : base(sender, "actor_created", delay)
        {
            this.newEntity = newEntity;
        }

        public GameEntity NewEntity
        {
            get { return newEntity; }
        }
    }
}

