using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using Warlord.GameTools;

namespace Warlord.Logic
{
    abstract class ContinuousProcess
    {
        private Thread processThread;        
        
        private bool started;   
        private bool kill;

        public ContinuousProcess()
        {
            started = false;
            kill = false;

        }

        public void Start( )
        {            
            processThread = new Thread(() => ProcessBehavior());
            processThread.Start();
            started = true;
        }

        abstract protected void ProcessBehavior();

        protected void SafePointCheckIn( )
        {
            if( Kill )
                Thread.CurrentThread.Abort( );
        }
        public void KillProcess()
        {
            kill = true;
        }
        public bool Running
        {
            get
            {
                if(processThread == null)
                    return false;

                return processThread.IsAlive;
            }
        }                
        public bool Started
        {
            get
            {
                return started;
            }
        }

        public bool Kill
        {
            get { return kill; }
            set { kill = value; }
        }
    }
}
