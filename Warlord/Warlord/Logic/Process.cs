using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;

namespace Warlord.Logic
{
    abstract class Process
    {
        Thread processThread;
        int timeAllocated;
        bool started;

        private EventWaitHandle waitHandle;

        Process( )
        {
            waitHandle = new EventWaitHandle( false, EventResetMode.ManualReset );
            started = false;
        }

        public void Update( GameTime gameTime )
        {
            waitHandle.Set( );   
            if(!started)
            {
                processThread = new Thread(() => UpdateBehavior(gameTime));
                processThread.Start();
            }

            Thread.Sleep( timeAllocated );

            if( processThread.IsAlive)
                waitHandle.Reset( );            
        }

        abstract public void UpdateBehavior( GameTime gameTime );
        
        public bool Running
        {
            get
            {
                return processThread.IsAlive;
            }
        }
    }
}
