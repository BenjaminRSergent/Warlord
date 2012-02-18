using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Warlord.Application;
using Warlord.Event;
using Warlord.Event.EventTypes;
using Warlord.Logic.Data.Entity;
using Warlord.GameTools;
using Warlord.Logic.Data.World;
using System;

namespace Warlord.Logic.Physics
{
    class WarlordPhysics
    {
        List<ForceGenerator> globalForces;
        CollisionDetection collisionDetection;

        public WarlordPhysics()
        {
            globalForces = new List<ForceGenerator>();
            collisionDetection = new CollisionDetection(this);
        }
        public void InitializeBasicForces()
        {
            ConstantDirectionalAcceleration gravity = new ConstantDirectionalAcceleration(this, 9.81f, Vector3.Down);
            SimpleDrag airResistance = new SimpleDrag(16f);

            globalForces.Add(gravity);
            globalForces.Add(airResistance);
        }
        public void AttachGlobalForce(ForceGenerator force)
        {
            globalForces.Add(force);
        }
        public void Update(GameTime gameTime)
        {
            List<GameEntity> entities = GlobalSystems.EntityManager.GetAllEntity( );

            foreach(GameEntity entity in entities)
            {
                foreach(ForceGenerator force in globalForces)
                    force.ApplyForceTo(gameTime, entity);

                collisionDetection.ResolveCollisions( gameTime, entity);
                entity.Update(gameTime);
                entity.ResetForce( );
            }
        }
        public CollisionData CastRay(Vector3 start, Vector3 direction, float length, float step)
        {
            Vector3 currentPosition;
            CollisionData collisionData = new CollisionData( );
            Optional<Block> block = new Optional<Block>();

            List<GameEntity> entitiesInRange = GlobalSystems.EntityManager.GetEntitiesWithin(start + length / 2 * direction, (int)(length / 2) );
                         
            for(float onRay = step; onRay < length; onRay += step)
            {
                currentPosition = start + onRay*direction;

                block = GlobalSystems.Blocks.GetBlockAt(currentPosition);
                if(block.Valid && block.Data.Type != BlockType.Air)
                {
                    collisionData.collisionType = CollisionType.Block;
                    collisionData.collisionLocation = currentPosition;
                    collisionData.hitObject = block.Data;

                    return collisionData;
                }

                foreach(GameEntity entity in entitiesInRange)
                {
                    currentPosition = start + onRay*direction;
                    
                    if(entity.CurrentBoundingBox.Contains(currentPosition) != ContainmentType.Disjoint)
                    {
                        collisionData.collisionType = CollisionType.Entity;
                        collisionData.collisionLocation = currentPosition;
                        collisionData.hitObject = entity;
                    }
                }
            }            

            collisionData.collisionType = CollisionType.None;
            return collisionData;
        }
        public CollisionData CastShape(BoundingBox shape, Vector3 direction, float length, float step)
        {            
            CollisionData collisionData = new CollisionData( );
            List<Block> blocks = new List<Block>( );
            List<Block> nonAir = new List<Block>( );            

            Vector3 center = shape.Min + (shape.Max - shape.Min)/2;

            List<GameEntity> entitiesInRange = GlobalSystems.EntityManager.GetEntitiesWithin(center + length * direction, (int)(length / 2) );

            float distance;
            float closestDistance;
            Block closestBlock = null;

            BoundingBox currentBox = shape;
            Matrix transformStep = Matrix.CreateTranslation(step*direction);
            
            for(float onRay = 0; onRay < length; onRay += step)
            {
                currentBox.Min = Vector3.Transform(currentBox.Min, transformStep);
                currentBox.Max = Vector3.Transform(currentBox.Max, transformStep);

                center = shape.Min + (shape.Max - shape.Min)/2;

                blocks = GlobalSystems.Blocks.GetBlockWithin(currentBox);
                
                nonAir.Clear( );

                foreach(Block block in blocks)
                { 
                    if(block.Type != BlockType.Air)
                    {
                        nonAir.Add(block);                        
                    }

                    closestDistance = float.MaxValue;
                    foreach(Block solidBlock in nonAir)
                    {
                        distance = (solidBlock.Center - center).Length( );
                        if(distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestBlock = solidBlock;
                        }
                    }

                    if(closestDistance != float.MaxValue)
                    {
                        collisionData.collisionType = CollisionType.Block;
                        collisionData.collisionLocation = closestBlock.BackLeftbottomPosition;
                        collisionData.collisionNormal = CalcBlockCollisionNormal(block, currentBox, block.BoundingBox);
                        collisionData.hitObject = closestBlock;

                        return collisionData;
                    }
                }

                foreach(GameEntity entity in entitiesInRange)
                {                      
                    if(shape.Contains(entity.CurrentBoundingBox) != ContainmentType.Disjoint)
                    {
                        collisionData.collisionType = CollisionType.Entity;
                        collisionData.collisionLocation = entity.CurrentPosition;
                        collisionData.hitObject = entity;

                        return collisionData;
                    }
                }
            }            

            collisionData.collisionType = CollisionType.None;
            return collisionData;
        }
        private Vector3 CalcBlockCollisionNormal( Block hitBlock, BoundingBox boxA, BoundingBox boxB )
        {
            Vector3 normal = Vector3.Zero;

            Vector3 minLessMax = boxA.Min - boxB.Max;
            Vector3 maxLessMin = boxB.Min - boxA.Max;
                                    
            normal = Vector3.Up;

            return normal;
        }
        private bool IsInBounds( float number, float min, float max )
        {
            return number > min && number < max;
        }
    }
}
