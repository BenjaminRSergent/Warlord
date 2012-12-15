using System;
using GameTools;
using Warlord.Logic.Data.Entity;

namespace Warlord.Event.EventTypes
{
    class EntityRemovedEvent
        : BaseGameEvent
    {
        GameEntity deadEntity;

        public EntityRemovedEvent(Optional<Object> sender, int delay, GameEntity deadEntity)
            : base(sender, "entity_removed", delay)
        {
            this.deadEntity = deadEntity;
        }

        public GameEntity DeadEntity
        {
            get { return deadEntity; }
        }
    }
}
