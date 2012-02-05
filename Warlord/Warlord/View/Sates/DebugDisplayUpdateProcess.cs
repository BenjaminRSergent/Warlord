using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warlord.Logic;
using Warlord.Logic.Data;
using Warlord.View.Human.Display;

namespace Warlord.View.Sates
{
    class DebugDisplayUpdateProcess : ContinuousProcess
    {
        private volatile Dictionary<Region, RegionGraphics> regionGraphics;

        public DebugDisplayUpdateProcess( Dictionary<Region, RegionGraphics> regionGraphics )
        {
            this.regionGraphics = regionGraphics;
        }

        protected override void ProcessBehavior()
        {           
            while(true)
            {
                RegionGraphics[] threadSafeGraphics;
                lock(regionGraphics)
                { 
                    threadSafeGraphics = regionGraphics.Values.ToArray( );
                }

                foreach(RegionGraphics region in threadSafeGraphics)
                {                    
                    if( region != null )
                        region.Update( );

                    SafePointCheckIn( );
                }
            }
        }
    }
}
