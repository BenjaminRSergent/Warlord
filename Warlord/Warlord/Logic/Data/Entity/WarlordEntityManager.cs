using System.Collections.Generic;
using System.Diagnostics;
using GameTools.Graph;
using Microsoft.Xna.Framework;
using Warlord.Application;
using Warlord.Event.EventTypes;
using Warlord.GameTools;

namespace Warlord.Logic.Data.Entity
{
    class WarlordEntityManager : EntityManager
    {
        Player player;
        Dictionary<uint, GameEntity> entityMap;
        Dictionary<Vector3i, HashSet<GameEntity>> entityCells;

        int cellSideLength;
        uint nextEntityID;

        public WarlordEntityManager(int cellSideLength)
        {
            entityCells = new Dictionary<Vector3i, HashSet<GameEntity>>();
            entityMap = new Dictionary<uint, GameEntity>();
            nextEntityID = 0;

            this.cellSideLength = cellSideLength;
        }
        public GameEntity GetEntity(uint id)
        {
            Debug.Assert(entityMap.ContainsKey(id));

            return entityMap[id];
        }
        public void AddPlayer(Vector3 position)
        {
            Debug.Assert(player == null);

            player = new Player(position);
            AddEntity(player);
        }
        private void AddEntity(GameEntity newEntity)
        {
            Vector3i cell = GetCellFromPosition(newEntity.CurrentPosition);

            newEntity.InitalizeID(NextEntityID);
            entityMap.Add(newEntity.actorID, newEntity);

            GlobalSystems.EventManager.SendEvent(new ActorCreatedEvent(new Optional<object>(),
                                                                        0,
                                                                        newEntity));
        }
        private void RemoveEntity(GameEntity deadEntity)
        {
            Vector3i cell = GetCellFromPosition(deadEntity.CurrentPosition);

            entityMap.Remove(deadEntity.actorID);
            RemoveFromCell(cell, deadEntity);

            GlobalSystems.EventManager.SendEvent(new ActorRemovedEvent(new Optional<object>(),
                                                                        0,
                                                                        deadEntity));
        }
        private void AddToCell(Vector3i cell, GameEntity entity)
        {
            HashSet<GameEntity> theCell;

            entityCells.TryGetValue(cell, out theCell);

            if(theCell == null)
                entityCells.Add(cell, new HashSet<GameEntity>());

            entityCells[cell].Add(entity);
        }
        private void RemoveFromCell(Vector3i cell, GameEntity entity)
        {
            HashSet<GameEntity> theCell;

            entityCells.TryGetValue(cell, out theCell);

            Debug.Assert(theCell != null);
            Debug.Assert(entityCells[cell].Contains(entity));

            entityCells[cell].Remove(entity);
        }
        public List<GameEntity> GetEntitiesWithin(Vector3 position, int radius)
        {
            List<GameEntity> nearbyEntities = new List<GameEntity>();
            HashSet<GameEntity> nearbyCell;

            int radiusInCells = cellSideLength / radius + 1;

            int radiusSq = radius * radius;

            Vector3i centerCell = GetCellFromPosition(position);
            Vector3i adjacentCell;

            for(int x = -radiusInCells; x < radiusInCells + 1; x++)
            {
                for(int y = -radiusInCells; y < radiusInCells + 1; y++)
                {
                    for(int z = -radiusInCells; z < radiusInCells + 1; z++)
                    {
                        adjacentCell = centerCell + new Vector3i(x, y, z);

                        entityCells.TryGetValue(adjacentCell, out nearbyCell);

                        if(nearbyCell != null)
                        {
                            foreach(GameEntity entity in nearbyCell)
                            {
                                int distanceSq = (int)((entity.CurrentPosition - position).LengthSquared());

                                if(distanceSq < radiusSq)
                                    nearbyEntities.Add(entity);
                            }
                        }
                    }
                }
            }

            return nearbyEntities;
        }
        private Vector3i GetCellFromPosition(Vector3 position)
        {
            return Transformation.ChangeVectorScale(position,
                                                    new Vector3i(cellSideLength, cellSideLength, cellSideLength));
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
