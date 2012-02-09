using System;
using Warlord.GameTools;
using Warlord.Logic.Data.Entity;

namespace Warlord.Event.EventTypes
{
    class ActorMovedEvent : BaseGameEvent
    {
        MovingEntity movedEntity;

        public ActorMovedEvent(Optional<Object> sender, int delay, MovingEntity movedEntity)
            : base(sender, "actor_moved", delay)
        {
            this.movedEntity = movedEntity;
        }

        public MovingEntity MovedEntity
        {
            get { return movedEntity; }
            set { movedEntity = value; }
        }
    }
}
