using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameTools.State;
using Warlord.Logic.Data.World;
using GameTools.Graph;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Warlord.Logic.States
{
    class DebugPlayingState : State<WarlordLogic>
    {
        RegionUpdater regionUpdater;
        Stopwatch stopWatch;

        public DebugPlayingState(WarlordLogic owner, int drawDistance, Vector3i regionSize)
            : base(owner)
        {
            regionUpdater = new RegionUpdater(drawDistance, 27, regionSize);            

            stopWatch = new Stopwatch( );
            stopWatch.Start( );
        }
        public override void EnterState()
        {
            owner.EntityManager.AddPlayer(new Vector3f(0, 80, 0));
            owner.ProcessManager.AttachProcess( regionUpdater );
        }
        public override void Update()
        {
            stopWatch.Restart( );
            owner.ProcessManager.Update( owner.MostRecentGameTime );
        }
        public override void ExitState()
        {
            owner.ProcessManager.ShutDown( );
            owner.EntityManager.ShutDown( );
        }
    }
}
