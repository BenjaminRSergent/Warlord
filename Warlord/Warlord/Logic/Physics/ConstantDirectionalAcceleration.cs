using Microsoft.Xna.Framework;
using Warlord.Logic.Data.Entity;

namespace Warlord.Logic.Physics
{
    class ConstantDirectionalAcceleration : ForceGenerator
    {
        private float acceleration;

        private Vector3 direction;

        public ConstantDirectionalAcceleration(float acceleration, Vector3 direction)
        {
            this.acceleration = acceleration;
            Direction = direction;
        }
        public void ApplyForceTo(GameTime gameTime, GameEntity entity)
        {
            entity.SumOfForces += acceleration * entity.Mass * Direction;
        }

        public float Force
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
