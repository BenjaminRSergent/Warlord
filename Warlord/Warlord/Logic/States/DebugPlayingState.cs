using GameTools.Graph;
using GameTools.State;
using Microsoft.Xna.Framework;
using Warlord.Application;
using Warlord.Logic.Data.World;

namespace Warlord.Logic.States
{
    class DebugPlayingState : State<WarlordLogic>
    {
        RegionController regionUpdater;

        public DebugPlayingState(WarlordLogic owner, int drawDistance, Vector3 regionSize)
            : base(owner)
        {
            regionUpdater = new RegionController(drawDistance, 27, regionSize);
        }
        public override void EnterState()
        {
            owner.EntityManager.AddPlayer(new Vector3(0, 80, 0));
            GlobalSystems.ThreadManager.AttachThread(regionUpdater);
        }
        public override void Update()
        {
            owner.ProcessManager.Update(owner.MostRecentGameTime);
        }
        public override void ExitState()
        {
            owner.ProcessManager.ShutDown();
            owner.EntityManager.ShutDown();
        }
    }
}
