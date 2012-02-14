using Microsoft.Xna.Framework;
using Warlord.Logic.Data.Entity;

namespace Warlord.Logic.Physics
{
    class SimpleDrag : ForceGenerator
    {
        private float dragCoefficant;

        public SimpleDrag(float dragCoefficant)
        {
            this.dragCoefficant = dragCoefficant;
        }
        public void ApplyForceTo(GameTime gameTime, GameEntity entity)
        {
            Vector3 opposingForce = dragCoefficant * entity.Velocity;
            entity.SumOfForces += opposingForce;
        }
        public float DragCoefficant
        {
            get { return dragCoefficant; }
            set { dragCoefficant = value; }
        }
    }
}
