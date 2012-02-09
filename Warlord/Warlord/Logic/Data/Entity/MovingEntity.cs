using Microsoft.Xna.Framework;

namespace Warlord.Logic.Data.Entity
{
    class MovingEntity : GameEntity
    {
        float mass;
        Vector3 velocity;
        Vector3 sumOfForces;

        private Vector3 previousPosition;

        public MovingEntity(Vector3 position) : base(position)
        {
            previousPosition = position;
            currentPosition = position;
        }
        public void Update(GameTime gameTime)
        {
            float seconds = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            currentPosition += velocity * seconds;
        }
        new public void Teleport(Vector3 location)
        {
            previousPosition = currentPosition;
            this.currentPosition = location;
        }
        public void UndoMove()
        {
            currentPosition = previousPosition;
        }
        public void Stop()
        {
            velocity = Vector3.Zero;
        }
        public void ResetForce()
        {
            sumOfForces = Vector3.Zero;
        }
        protected Vector3 PreviousPosition
        {
            get { return previousPosition; }
        }
        public float Mass
        {
            get { return mass; }
            set { mass = value; }
        }
        public Vector3 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        public Vector3 SumOfForces
        {
            get { return sumOfForces; }
            set { sumOfForces = value; }
        }
    }
}
