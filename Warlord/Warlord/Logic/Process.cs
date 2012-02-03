using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Warlord.GameTools;

namespace Warlord.Logic
{
    abstract class Process : IDisposable
    {
        Thread processThread;
        int timeAllocated;
        bool started;
        Optional<Process> next;

        private EventWaitHandle waitHandle;
        private bool done;
        private bool kill;        

        public Process()
        {
            waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
            started = false;
            done = false;
            kill = false;

            this.timeAllocated = 0;
        }
        public Process(int timeAllocated)
        {
            waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
            started = false;
            done = false;

            this.timeAllocated = timeAllocated;
        }

        public void Update(GameTime gameTime)
        {            
            waitHandle.Set();
            if(processThread == null || (!started && !done))
            {
                processThread = new Thread(() => UpdateBehavior(gameTime));
                processThread.Start();
                started = true;
            }

            Thread.Sleep(timeAllocated);

            if(processThread.IsAlive)
                waitHandle.Reset();
            else if(started)
                done = true;
        }

        abstract protected void UpdateBehavior(GameTime gameTime);

        protected void SafePointCheckIn( )
        {
            if( Kill )
                Thread.CurrentThread.Abort( );

                WaitHandle.WaitOne();
        }

        public void KillProcess()
        {
            kill = true;
            waitHandle.Set( );
        }

        public bool Done
        {
            get
            {
                return done;
            }
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
        public void AttachNext(Process nextProcess)
        {
            next = new Optional<Process>(nextProcess);
        }
        public Optional<Process> Next
        {
            get { return next; }
        }
        protected EventWaitHandle WaitHandle
        {
            get { return waitHandle; }
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

        public void Dispose()
        {
            waitHandle.Dispose( );
        }
    }
}
