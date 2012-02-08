using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Threading;

namespace GameTools.Process
{
    class ProcessManager
    {
        private List<MultiTickProcess> temporaryProcesses;
        private List<ThreadProcess> continuousProcesses;

        public ProcessManager()
        {
            temporaryProcesses = new List<MultiTickProcess>();
            continuousProcesses = new List<ThreadProcess>();
        }

        public void Update(GameTime gameTime)
        {
            List<MultiTickProcess> toRemove = new List<MultiTickProcess>();
            foreach(MultiTickProcess process in temporaryProcesses)
            {
                process.Update(gameTime);

                if(process.Dead)
                    toRemove.Add(process);
            }

            RemoveTemporaryProcesses(toRemove);
        }

        public void AttachProcess(MultiTickProcess process)
        {
            temporaryProcesses.Add(process);
        }
        public void AttachProcess(ThreadProcess process)
        {
            continuousProcesses.Add(process);
            process.Start();
        }
        public void KillThread(ThreadProcess process)
        {
            process.KillProcess();
            continuousProcesses.Remove(process);
        }
        private void FinishProcess(MultiTickProcess process)
        {
            if(process.Next.Valid)
                temporaryProcesses.Add(process.Next.Data);
        }
        private void RemoveTemporaryProcesses(List<MultiTickProcess> toRemove)
        {
            foreach(MultiTickProcess process in toRemove)
            {
                temporaryProcesses.Remove(process);
            }
        }
        internal void ShutDown()
        {
            foreach(MultiTickProcess process in temporaryProcesses)
            {
                process.KillProcess();
            }
            foreach(ThreadProcess process in continuousProcesses)
            {
                process.KillProcess();
            }
        }
    }
}
