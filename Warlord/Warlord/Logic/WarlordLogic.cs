using GameTools.Graph;
using GameTools.State;
using Microsoft.Xna.Framework;
using Warlord.Interfaces.Subsystems;
using Warlord.Logic.Data.Entity;
using Warlord.Logic.States;

namespace Warlord.Logic
{
    class WarlordLogic : GameLogic
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
            entityManager = new WarlordEntityManager();

            entityManager.AddPlayer(new Vector3f(0, 80, 0));

            GlobalApplication.Application.GameEventManager.Subscribe(Update, "update");

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
        public void Update(object sender, object data)
        {
            mostRecentGameTime = data as GameTime;

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
