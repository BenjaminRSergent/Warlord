using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using Warlord.GameTools;

namespace GameTools.Process
{
    abstract class ThreadProcess
    {
        private Thread processThread;

        private bool started;
        private bool kill;

        EventWaitHandle waitHandle;
        private bool waiting;

        public ThreadProcess()
        {
            started = false;
            kill = false;
            waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
        }

        public void Start()
        {
            Unpause();
            processThread = new Thread(() => ProcessBehavior());
            processThread.Start();
            started = true;
        }
        public void Pause()
        {
            waitHandle.Reset();
        }
        public void Unpause()
        {
            waitHandle.Set();
        }
        abstract protected void ProcessBehavior();

        protected void SafePointCheckIn()
        {
            if(Kill)
                Thread.CurrentThread.Abort();

            waiting = true;
            waitHandle.WaitOne();
            waiting = false;
        }
        public void KillProcess()
        {
            kill = true;
            waitHandle.Set();
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
        public bool Waiting
        {
            get
            {
                return waiting;
            }
        }
        public bool Kill
        {
            get { return kill; }
        }
    }
}
