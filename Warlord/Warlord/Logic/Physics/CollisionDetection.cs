using System;
using Warlord.Logic.Data.Entity;
using Microsoft.Xna.Framework;
using Warlord.Logic.Data.World;
using Warlord.GameTools;

namespace Warlord.Logic.Physics
{
    class CollisionDetection
    {
        private WarlordPhysics owner;
        public CollisionDetection(WarlordPhysics owner)
        {
            this.owner = owner;
        }
        public void ResolveCollisions( GameTime gameTime, GameEntity entity)
        {
            EntityWorldCollisions(gameTime, entity );     
        }

        private void EntityWorldCollisions(GameTime gameTime, GameEntity entity )
        {
            const float step = 0.1f;
            Vector3 direction = new Vector3();
            CollisionData collisionData;

            direction = entity.Facing;
            collisionData = owner.CastShape(entity.CurrentBoundingBox,
                                            direction,
                                            (entity.GetFuturePosition(gameTime)-entity.CurrentPosition).Length(),
                                            step);

            if(collisionData.collisionType == CollisionType.Block)
            {
                Vector3 velocity = entity.Velocity;
                Vector3.Reflect(ref velocity, ref collisionData.collisionNormal, out velocity);


                // Do with force?
                entity.Velocity = velocity/4;
            }            
        }        

    }
}
