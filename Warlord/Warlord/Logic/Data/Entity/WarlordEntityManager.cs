using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameTools.Graph;
using Warlord.GameTools;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Warlord.Event.EventTypes;

namespace Warlord.Logic.Data.Entity
{
    class WarlordEntityManager : EntityManager
    {
        Player player;
        Dictionary<uint, GameEntity> entityMap;
        uint nextEntityID;

        public WarlordEntityManager()
        {
            entityMap = new Dictionary<uint, GameEntity>();
            nextEntityID = 0;
        }
        public GameEntity GetEntity(uint id)
        {
            Debug.Assert(entityMap.ContainsKey(id));

            return entityMap[id];
        }
        public void AddEntity(GameEntity newEntity)
        {
            newEntity.InitalizeID(NextEntityID);
            entityMap.Add(newEntity.actorID, newEntity);
            GlobalSystems.EventManager.SendEvent(new ActorCreatedEvent(new Optional<object>(),
                                                                         0,
                                                                         newEntity));
        }
        public void AddPlayer(Vector3 position)
        {
            player = new Player(position);
            AddEntity(player);
        }
        public Player Player
        {
            get
            {
                return player;
            }
        }
        public void ShutDown()
        {
            entityMap.Clear();
        }

        private uint NextEntityID
        {
            get
            {
                uint id = nextEntityID;
                nextEntityID++;

                return id;
            }
        }
    }
}
