using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Warlord.Logic.Data.Entity;
using Warlord.GameTools;
using Warlord.Logic.Data.World;
using Warlord.Logic.Data;

namespace Warlord.Event.EventTypes
{
    class RegionRemovedData
    {
        public Region deadRegion;

        public RegionRemovedData(Region deadRegion)
        {
            this.deadRegion = deadRegion;
        }        
    }

    class RegionRemovedEvent : BaseGameEvent
    {
        RegionRemovedData data;        

        public RegionRemovedEvent( Optional<Object> sender, int delay, RegionRemovedData data )
            : base(sender, "region_removed", delay)
        {
            this.data = data;
        }

        public RegionRemovedData Data
        {
            get { return data; }
        }
    }
}
