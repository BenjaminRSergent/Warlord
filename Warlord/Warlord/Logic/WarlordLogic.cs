using GameTools.Graph;
using GameTools.Process;
using GameTools.State;
using Microsoft.Xna.Framework;
using Warlord.Logic.Data.Entity;
using Warlord.Logic.States;
using Warlord.Logic.Data.World;
using Warlord.Interfaces;
using System.Collections.Generic;

namespace Warlord.Logic
{
    class WarlordLogic : BlockAccess
    {
        private ProcessManager processManager;
        private WarlordEntityManager entityManager;        
        private GameTime mostRecentGameTime;

        public StateMachine<WarlordLogic> stateMachine;
        public RegionController regionUpdater;        

        Vector3 regionSize;
        int entityCellSize;

        public WarlordLogic()
        {
            Initialize();
        }
        private void Initialize()
        {
            processManager = new ProcessManager();            

            stateMachine = new StateMachine<WarlordLogic>(this);
        }
        public void BeginGame(Vector3 regionSize, int entityCellSize, int drawDistance)
        {
            this.regionSize = regionSize;
            this.entityCellSize = entityCellSize;

            entityManager = new WarlordEntityManager(entityCellSize);            
            stateMachine.ChangeState(new DebugPlayingState(this, drawDistance, regionSize));
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

        public GameTools.Optional<Block> GetBlockAt(Vector3 location)
        {
            location.X = (int)location.X;
            location.Y = (int)location.Y;
            location.Z = (int)location.Z;

            return regionUpdater.GetBlock(location);
        }
        public List<Block> GetBlockWithin(BoundingBox currentBox)
        {
            return regionUpdater.GetBlocksWithin(currentBox);
        }
    }
}
