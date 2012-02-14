using System;
using Warlord.GameTools;
using Warlord.Logic.Data.Entity;

namespace Warlord.Event.EventTypes
{
    class EntityMovedEvent : BaseGameEvent
    {
        GameEntity movedEntity;

        public EntityMovedEvent(Optional<Object> sender, int delay, GameEntity movedEntity)
            : base(sender, "entity_moved", delay)
        {
            this.movedEntity = movedEntity;
        }

        public GameEntity MovedEntity
        {
            get { return movedEntity; }
            set { movedEntity = value; }
        }
    }
}
