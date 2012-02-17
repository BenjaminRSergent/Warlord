using System;
using Warlord.GameTools;
using Warlord.Logic.Data.Entity;

namespace Warlord.Event.EventTypes
{
    class EntityCreatedEvent : BaseGameEvent
    {
        GameEntity newEntity;

        public EntityCreatedEvent(Optional<Object> sender, int delay, GameEntity newEntity)
            : base(sender, "entity_created", delay)
        {
            this.newEntity = newEntity;
        }

        public GameEntity NewEntity
        {
            get { return newEntity; }
        }
    }
}

