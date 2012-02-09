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
        private List<MultiTickProcess> processes;

        public ProcessManager()
        {
            processes = new List<MultiTickProcess>();
        }

        public void Update(GameTime gameTime)
        {
            List<MultiTickProcess> toRemove = new List<MultiTickProcess>();
            foreach(MultiTickProcess process in processes)
            {
                process.Update(gameTime);

                if(process.Dead)
                    toRemove.Add(process);
            }

            RemoveTemporaryProcesses(toRemove);
        }

        public void AttachProcess(MultiTickProcess process)
        {
            processes.Add(process);
        }

        private void FinishProcess(MultiTickProcess process)
        {
            if(process.Next.Valid)
                processes.Add(process.Next.Data);
        }
        private void RemoveTemporaryProcesses(List<MultiTickProcess> toRemove)
        {
            foreach(MultiTickProcess process in toRemove)
            {
                processes.Remove(process);
            }
        }
        internal void ShutDown()
        {
            foreach(MultiTickProcess process in processes)
            {
                process.KillProcess();
            }            
        }
    }
}
