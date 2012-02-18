using Microsoft.Xna.Framework;
using Warlord.Logic.Data.Entity;

namespace Warlord.Logic.Physics
{
    class ConstantDirectionalAcceleration : ForceGenerator
    {
        private WarlordPhysics owner;
        private float acceleration;
        private Vector3 direction;

        public ConstantDirectionalAcceleration(WarlordPhysics owner, float acceleration, Vector3 direction)
        {
            this.owner = owner;
            this.acceleration = acceleration;
            Direction = direction;
        }
        public void ApplyForceTo(GameTime gameTime, GameEntity entity)
        {
            CollisionData collision = owner.CastRay(entity.CurrentPosition, direction, 1f, 0.1f);

            if( collision.collisionType == CollisionType.None )
                entity.SumOfForces += acceleration * entity.Mass * Direction;
        }

        public float Acceleration
        {
            get { return acceleration; }
            set { acceleration = value; }
        }
        public Vector3 Direction
        {
            get
            {
                return direction;
            }
            set
            {
                direction = value;
                direction.Normalize();
            }
        }
    }
}
