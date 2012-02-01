using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warlord.Interfaces.Subsystems;
using Warlord.Logic.Data.World;
using GameTools.Graph;
using Warlord.Logic.Data;
using Microsoft.Xna.Framework;

namespace Warlord.Logic
{
    class WarlordLogic : GameLogic
    {
        ProcessManager processManager;
        RegionUpdater regionUpdater;
        WarlordEntityManager entityManager;

        public WarlordLogic()
        {
            processManager = new ProcessManager();
            regionUpdater = new RegionUpdater(5, 3, new Vector3i(16, 128, 16));
            entityManager = new WarlordEntityManager();

            entityManager.AddPlayer(new Vector3f(0, 80, 0));

            GlobalApplication.Application.GameEventManager.Subscribe( Update, "update");
        }
        public void BeginGame( )
        {
            entityManager.AddPlayer(new Vector3f(0, 80, 0));
            processManager.AttachProcess(regionUpdater);
        }
        public void Update( object sender, object data )
        {
            GameTime gameTime = data as GameTime;

            processManager.Update( gameTime );
        }
        public WarlordEntityManager EntityManager
        {
            get { return entityManager; }
        }
        public ProcessManager ProcessManager
        {
            get { return processManager; }
        }

        
    }
}
