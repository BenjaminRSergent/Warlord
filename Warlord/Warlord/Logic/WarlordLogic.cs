using GameTools.Graph;
using GameTools.Process;
using GameTools.State;
using Microsoft.Xna.Framework;
using Warlord.Logic.Data.Entity;
using Warlord.Logic.States;

namespace Warlord.Logic
{
    class WarlordLogic
    {
        ProcessManager processManager;
        WarlordEntityManager entityManager;

        StateMachine<WarlordLogic> stateMachine;
        GameTime mostRecentGameTime;

        public WarlordLogic()
        {
            Initialize();
        }
        private void Initialize()
        {
            processManager = new ProcessManager();
            entityManager = new WarlordEntityManager(16);

            stateMachine = new StateMachine<WarlordLogic>(this);
        }
        public void BeginGame()
        {
            stateMachine.ChangeState(new DebugPlayingState(this, 10, new Vector3i(16, 128, 16)));
        }
        public void EndGame()
        {
            stateMachine.ChangeState(new NullState<WarlordLogic>(this));
        }
        public void Update(GameTime gameTime)
        {
            mostRecentGameTime = gameTime;

            stateMachine.Update();
        }
        public WarlordEntityManager EntityManager
        {
            get { return entityManager; }
        }
        public ProcessManager ProcessManager
        {
            get { return processManager; }
        }
        public StateMachine<WarlordLogic> StateMachine
        {
            get { return stateMachine; }
        }
        public GameTime MostRecentGameTime
        {
            get { return mostRecentGameTime; }
        }
    }
}
