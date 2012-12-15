using System;
using GameTools;
using Warlord.Logic.Data;

namespace Warlord.Event.EventTypes
{
    class RegionRemovedEvent : BaseGameEvent
    {
        private Region deadRegion;

        public RegionRemovedEvent(Optional<Object> sender, int delay, Region deadRegion)
            : base(sender, "region_removed", delay)
        {
            this.deadRegion = deadRegion;
        }

        public Region DeadRegion
        {
            get { return deadRegion; }
        }
    }
}
