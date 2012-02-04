using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameTools.State
{
    class StateMachine<OwnerType>
    {
        OwnerType owner;

        State<OwnerType> globalState;
        State<OwnerType> previousState;
        State<OwnerType> currentState;

        public StateMachine(OwnerType owner)
        {
            this.owner = owner;
            this.globalState = new NullState<OwnerType>(owner);
            this.previousState = new NullState<OwnerType>(owner);
            this.currentState = new NullState<OwnerType>(owner);
        }
        public StateMachine(OwnerType owner, State<OwnerType> initalState)
        {
            this.owner = owner;
            this.globalState = new NullState<OwnerType>(owner);
            this.previousState = new NullState<OwnerType>(owner);
            this.currentState = initalState;
        }
        public StateMachine(OwnerType owner, State<OwnerType> initalState, State<OwnerType> globalState)
        {
            this.owner = owner;
            this.globalState = globalState;
            this.previousState = new NullState<OwnerType>(owner);
            this.currentState = initalState;
        }
        public void ChangeState(State<OwnerType> newState)
        {
            previousState = currentState;
            currentState = newState;

            previousState.ExitState();
            currentState.EnterState();
        }

        public void Update()
        {
            globalState.Update();
            currentState.Update();
        }
    }
}
