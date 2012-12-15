using System;
using GameTools;
using Warlord.Logic.Data.Entity;

namespace Warlord.Event.EventTypes
{
    class RefreshRegionGraphicsEvent : BaseGameEvent
    {
        public RefreshRegionGraphicsEvent(Optional<Object> sender, int delay)
            : base(sender, "refresh_region_graphics", delay)
        {
        }
    }
}

