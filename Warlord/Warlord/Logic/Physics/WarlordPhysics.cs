using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Warlord.Application;
using Warlord.Event;
using Warlord.Event.EventTypes;
using Warlord.Logic.Data.Entity;

namespace Warlord.Logic.Physics
{
    class WarlordPhysics
    {
        Dictionary<uint, MovingEntity> movingEntityMap;
        List<ForceGenerator> globalForces;
        CollisionDetection collisionDetection;

        public WarlordPhysics()
        {
            movingEntityMap = new Dictionary<uint, MovingEntity>();
            globalForces = new List<ForceGenerator>();
            collisionDetection = new CollisionDetection();

            GlobalSystems.EventManager.Subscribe(AddEntity, "actor_created_event");
            GlobalSystems.EventManager.Subscribe(RemoveEntity, "actor_removed_event");
        }
        public void InitializeBasicForces()
        {
            ConstantDirectionalForce gravity = new ConstantDirectionalForce(9.81f, Vector3.Down);
            SimpleDrag airResistance = new SimpleDrag(0.2f);
        }
        public void AttachGlobalForce(ForceGenerator force)
        {
            globalForces.Add(force);
        }
        public void Update(GameTime gameTime)
        {
            foreach( MovingEntity entity in movingEntityMap.Values)                
            {
                entity.ResetForce();

                foreach(ForceGenerator force in globalForces)
                    force.ApplyForceTo(gameTime, entity);

                 UpdateEntity(entity, gameTime);

                collisionDetection.ResolveCollisions(gameTime, entity);
            }
        }

        private void UpdateEntity(MovingEntity entity, GameTime gameTime)
        {
            entity.Velocity += entity.SumOfForces / entity.Mass;
            entity.Update(gameTime);            
        }
        private void AddEntity(BaseGameEvent theEvent)
        {
            ActorCreatedEvent creationEvent = theEvent as ActorCreatedEvent;
            GameEntity entity = creationEvent.NewEntity;

            if(entity is MovingEntity)
                movingEntityMap.Add(entity.actorID, entity as MovingEntity);
        }
        private void RemoveEntity(BaseGameEvent theEvent)
        {
            ActorRemovedEvent removalEvent = theEvent as ActorRemovedEvent;
            GameEntity entity = removalEvent.DeadEntity;

            if(entity is MovingEntity)
                movingEntityMap.Remove(entity.actorID);
        }
    }
}
