using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Warlord.GameTools;
using System.Diagnostics;

namespace GameTools.Process
{
    abstract class MultiTickProcess
    {
        private int timeLeft;
        private Optional<MultiTickProcess> next;

        public MultiTickProcess(int timeToLive)
        {
            this.timeLeft = timeToLive;
            next = new Optional<MultiTickProcess>();
        }
        public MultiTickProcess(int timeToLive, MultiTickProcess nextProcess)
        {
            this.timeLeft = timeToLive;
            next = new Optional<MultiTickProcess>(nextProcess);
        }

        public void Update(GameTime gameTime)
        {
            timeLeft -= gameTime.ElapsedGameTime.Milliseconds;

            UpdateBehavior(gameTime);

            if(timeLeft < 1)
            {
                Dead = true;
            }
        }

        abstract protected void UpdateBehavior(GameTime gameTime);

        public void AttachNext(MultiTickProcess nextProcess)
        {
            next = new Optional<MultiTickProcess>(nextProcess);
        }
        public void KillProcess()
        {
        }

        public bool Dead
        {
            get;
            protected set;
        }
        public Optional<MultiTickProcess> Next
        {
            get { return next; }
        }
    }
}
