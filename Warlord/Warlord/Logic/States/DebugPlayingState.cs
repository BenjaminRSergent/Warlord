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
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Threading;
using Warlord.Application;

namespace Warlord.Logic.States
{
    class DebugPlayingState : State<WarlordLogic>
    {
        RegionUpdater regionUpdater;

        public DebugPlayingState(WarlordLogic owner, int drawDistance, Vector3i regionSize)
            : base(owner)
        {
            regionUpdater = new RegionUpdater(drawDistance, 27, regionSize);
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
