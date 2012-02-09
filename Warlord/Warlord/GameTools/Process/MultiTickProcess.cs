using Microsoft.Xna.Framework;
using Warlord.GameTools;

namespace GameTools.Process
{
    abstract class MultiTickProcess
    {
        private Optional<MultiTickProcess> next;

        public MultiTickProcess()
        {
            next = new Optional<MultiTickProcess>();
        }
        public MultiTickProcess(int timeToLive, MultiTickProcess nextProcess)
        {
            next = new Optional<MultiTickProcess>(nextProcess);
        }

        public void Update(GameTime gameTime)
        {
            bool done = UpdateBehavior(gameTime);

            if(done)
            {
                Dead = true;
            }
        }

        abstract protected bool UpdateBehavior(GameTime gameTime);

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
