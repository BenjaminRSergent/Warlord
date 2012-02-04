using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Warlord.GameTools;
using System.Diagnostics;

namespace Warlord.Logic
{
    abstract class TemporaryProcess : IDisposable
    {
        private Thread processThread;
        private int timeAllocated;               
        
        private bool started;
        private bool loopExecuted;        
        private bool readyToDie;
        private bool kill;
        private EventWaitHandle waitHandle;
        private Stopwatch stopwatch;
        private Optional<TemporaryProcess> next;


        public TemporaryProcess()
        {
             stopwatch = new Stopwatch( );

            waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
            started = false;
            readyToDie = false;
            kill = false;

            this.timeAllocated = 0;
        }
        public TemporaryProcess(int timeAllocated)
        {
            stopwatch = new Stopwatch( );
            waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
            started = false;
            readyToDie = false;

            this.timeAllocated = timeAllocated;
        }

        public void Update(GameTime gameTime)
        {            
            waitHandle.Set();
            if(processThread == null || (!started && !readyToDie))
            {
                processThread = new Thread(() => UpdateBehavior(gameTime));
                processThread.Start();
                started = true;
            }

            stopwatch.Restart( );
            stopwatch.Start( );

            while(!loopExecuted && stopwatch.ElapsedMilliseconds < timeAllocated)
            {
                Thread.Sleep(1);
            }

            if(processThread.IsAlive)
                waitHandle.Reset();
            else if(started)
                readyToDie = true;
        }

        abstract protected void UpdateBehavior(GameTime gameTime);

        protected void SafePointCheckIn( )
        {
            if( Kill )
                Thread.CurrentThread.Abort( );

                WaitHandle.WaitOne();
        }
        public void AttachNext(TemporaryProcess nextProcess)
        {
            next = new Optional<TemporaryProcess>(nextProcess);
        }
        public void KillProcess()
        {
            kill = true;
            waitHandle.Set( );
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
        public bool ReadyToDie
        {
            get
            {
                return readyToDie;
            }
            protected set
            {
                loopExecuted = readyToDie;
            }
        }
        public bool LoopExcuted
        {
            get 
            {
                return loopExecuted; 
            }
            protected set
            {
                loopExecuted = value;
            }
        }        
        
        public Optional<TemporaryProcess> Next
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
