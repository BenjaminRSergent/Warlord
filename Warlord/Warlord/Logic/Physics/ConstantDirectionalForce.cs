using Microsoft.Xna.Framework;
using Warlord.Logic.Data.Entity;

namespace Warlord.Logic.Physics
{
    class ConstantDirectionalForce : ForceGenerator
    {
        private float force;

        private Vector3 direction;

        public ConstantDirectionalForce(float force, Vector3 direction)
        {
            this.force = force;
            Direction = direction;
        }
        public void ApplyForceTo(GameTime gameTime, MovingEntity entity)
        {
            entity.SumOfForces += force * Direction;
        }

        public float Force
        {
            get { return force; }
            set { force = value; }
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
