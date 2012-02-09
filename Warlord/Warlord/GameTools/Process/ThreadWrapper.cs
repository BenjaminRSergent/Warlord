using System.Threading;

namespace GameTools.Process
{
    abstract class ThreadWrapper
    {
        private Thread thread;

        private bool started;
        private bool kill;

        EventWaitHandle waitHandle;
        private bool waiting;
        private string name;

        public ThreadWrapper(string threadName)
        {
            name = threadName;

            started = false;
            kill = false;
            waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
        }

        public void Start()
        {
            Unpause();
            thread = new Thread(() => ThreadBehavior());
            thread.Name = name;
            thread.Start();
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
        abstract protected void ThreadBehavior();

        protected void SafePointCheckIn()
        {
            if(Kill)
                Thread.CurrentThread.Abort();

            waiting = true;
            waitHandle.WaitOne();
            waiting = false;
        }
        public void KillThread()
        {
            kill = true;
            waitHandle.Set();
        }
        public bool Running
        {
            get
            {
                if(thread == null)
                    return false;

                return thread.IsAlive;
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

        public string Name { get { return name; } }
    }
}
