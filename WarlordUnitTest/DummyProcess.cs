using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warlord.Logic;

namespace WarlordUnitTest
{
    class DummyProcess : TemporaryProcess
    {
        
        public DummyProcess( int timeAllocated ) : base(timeAllocated)
        {
            
        }

        public override void UpdateBehavior(Microsoft.Xna.Framework.GameTime gameTime)
        {
            x += 1;    
        }

        public int x { get; protected set; }
    }
}
