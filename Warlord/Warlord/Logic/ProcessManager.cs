using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Threading;

namespace Warlord.Logic
{
    class ProcessManager
    {
        List<TemporaryProcess> temporaryProcesses;
        List<ContinuousProcess> continuousProcesses;

        public ProcessManager()
        {
            temporaryProcesses = new List<TemporaryProcess>();   
            continuousProcesses = new List<ContinuousProcess>( );
        }

        public void Update(GameTime gameTime)
        {
            List<TemporaryProcess> toRemove = new List<TemporaryProcess>();
            foreach(TemporaryProcess process in temporaryProcesses)
            {
                if(process.Running || !process.Started)
                    process.Update(gameTime);

                if(process.ReadyToDie)
                {
                    FinishProcess(process);
                    toRemove.Add(process);
                }
            }

            RemoveTemporaryProcesses(toRemove);
        }

        public void AttachProcess(TemporaryProcess process)
        {
            temporaryProcesses.Add(process);
        }
        public void AttachProcess(ContinuousProcess process)
        {
            continuousProcesses.Add(process);
            process.Start( );
        }
        private void FinishProcess(TemporaryProcess process)
        {
            if(process.Next.Valid)
                temporaryProcesses.Add(process.Next.Data);
        }
        private void RemoveTemporaryProcesses(List<TemporaryProcess> toRemove)
        {
            foreach(TemporaryProcess process in toRemove)
            {
                temporaryProcesses.Remove(process);
            }
        }
        internal void ShutDown()
        {
            foreach(TemporaryProcess process in temporaryProcesses)
            {
                process.KillProcess( );
            }
            foreach(ContinuousProcess process in continuousProcesses)
            {
                process.KillProcess( );
            }
        }
    }
}
