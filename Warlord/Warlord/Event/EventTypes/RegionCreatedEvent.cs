using System;
using GameTools;
using Warlord.Logic.Data;

namespace Warlord.Event.EventTypes
{
    class RegionCreatedData
    {
        public Region newRegion;

        public RegionCreatedData(Region newRegion)
        {
            this.newRegion = newRegion;
        }
    }

    class RegionCreatedEvent : BaseGameEvent
    {
        RegionCreatedData data;

        public RegionCreatedEvent(Optional<Object> sender, int delay, RegionCreatedData data)
            : base(sender, "region_created", delay)
        {
            this.data = data;
        }

        public RegionCreatedData Data
        {
            get { return data; }
        }
    }
}
