using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Warlord.Logic
{
    class ProcessManager
    {
        List<Process> runningProcesses;

        public ProcessManager()
        {
            runningProcesses = new List<Process>();            
        }

        public void Update(GameTime gameTime)
        {
            List<Process> toRemove = new List<Process>();
            foreach(Process process in runningProcesses)
            {
                if(process.Running || !process.Started)
                    process.Update(gameTime);

                if(process.Done)
                {
                    FinishProcess(process);
                    toRemove.Add(process);
                }
            }

            RemoveProcesses(toRemove);
        }

        public void AttachProcess(Process process)
        {
            runningProcesses.Add(process);
        }
        private void FinishProcess(Process process)
        {
            if(process.Next.Valid)
                runningProcesses.Add(process.Next.Data);
        }
        private void RemoveProcesses(List<Process> toRemove)
        {
            foreach(Process process in toRemove)
            {
                runningProcesses.Remove(process);
            }
        }

        internal void ShutDown()
        {
            foreach(Process process in runningProcesses)
            {
                process.KillProcess( );
            }
        }
    }
}
