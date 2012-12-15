using GameTools.Graph;
using GameTools.State;
using Microsoft.Xna.Framework;
using Warlord.Application;
using Warlord.Logic.Data.World;
using Warlord.Logic.Physics;
using Warlord.Logic.Data.Entity;

namespace Warlord.Logic.States
{
    class DebugPlayingState : State<WarlordLogic>
    {       
        WarlordPhysics physics;
        public DebugPlayingState(WarlordLogic owner, int drawDistance, Vector3 regionSize)
            : base(owner)
        {
            owner.regionUpdater = new RegionController(drawDistance, 27, regionSize);

            physics = new WarlordPhysics();
        }
        public override void EnterState()
        {
            owner.EntityManager.AddPlayer(Vector3.Zero);
            owner.EntityManager.AddEntity(new Warlord.Logic.Data.Entity.GameEntity(new Vector3(10, 40, 10), EntityType.bear, 400, 0.75f));
            GlobalSystems.ThreadManager.AttachThread(owner.regionUpdater);
            physics.InitializeBasicForces( );
        }
        public override void Update()
        {
            physics.Update(owner.MostRecentGameTime);
            owner.ProcessManager.Update(owner.MostRecentGameTime);
        }
        public override void ExitState()
        {
            owner.ProcessManager.ShutDown();
            owner.EntityManager.ShutDown();
        }
    }
}
