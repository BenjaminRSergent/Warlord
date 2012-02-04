using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameTools.State
{
    abstract class State<OwnerType>
    {
        protected OwnerType owner;

        public State(OwnerType owner)
        {
            this.owner = owner;
        }

        abstract public void EnterState();
        abstract public void Update();
        abstract public void ExitState();
    }
}
