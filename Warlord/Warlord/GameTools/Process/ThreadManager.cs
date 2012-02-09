using System;
using System.Collections.Generic;

namespace GameTools.Process
{
    class ThreadManager
    {
        private Dictionary<string, ThreadWrapper> nameThreadMap;

        public ThreadManager()
        {
            nameThreadMap = new Dictionary<string, ThreadWrapper>();
        }

        public void AttachThread(ThreadWrapper process)
        {
            nameThreadMap.Add(process.Name, process);
            process.Start();
        }
        public void KillThread(string threadName)
        {
            ThreadWrapper thread = null;
            nameThreadMap.TryGetValue(threadName, out thread);

            if(thread != null)
            {
                thread.KillThread();
                nameThreadMap.Remove(threadName);
            }
        }
        public void ShutDown()
        {
            foreach(ThreadWrapper thread in nameThreadMap.Values)
            {
                thread.KillThread();
            }

            nameThreadMap.Clear();
        }
    }
}
