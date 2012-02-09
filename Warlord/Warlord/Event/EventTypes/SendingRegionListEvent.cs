using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warlord.Logic.Data;
using Warlord.GameTools;

namespace Warlord.Event.EventTypes
{
    class SendingRegionListEvent : BaseGameEvent
    {
        private List<Region> currentRegionList;

        public SendingRegionListEvent(Optional<Object> sender, int delay, List<Region> currentRegionList)
            : base(sender, "sending_region_list", delay)
        {
            this.currentRegionList = currentRegionList;
        }

        public List<Region> CurrentRegionList
        {
            get { return currentRegionList; }
        }
    }
}
