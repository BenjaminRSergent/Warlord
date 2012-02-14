using System.Diagnostics;
using Microsoft.Xna.Framework;
using Warlord.GameTools;

namespace Warlord.Logic.Data.Entity
{
    class GameEntity
    {
        private Optional<uint> id;
        protected Vector3 currentPosition;
        private float mass;
        private Vector3 velocity;
        private Vector3 sumOfForces;
        private Vector3 previousPosition;

        public GameEntity(Vector3 position)
        {
            this.previousPosition = position;
            this.currentPosition = position;
            id = new Optional<uint>();
        }
        public void Update(GameTime gameTime)
        {
            float seconds = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            currentPosition += velocity * seconds;
        }
        public void Teleport(Vector3 location)
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
        public void InitalizeID(uint id)
        {
            Debug.Assert(!this.id.Valid);

            this.id = new Optional<uint>(id);
        }
        public override int GetHashCode()
        {
            return (int)id.Data;
        }
        public uint entityID { get { Debug.Assert(id.Valid); return id.Data; } }
        public float Mass { get { return mass; } set { mass = value; } }
        public Vector3 Velocity { get { return velocity; } set { velocity = value; } }
        public Vector3 SumOfForces { get { return sumOfForces; } set { sumOfForces = value; } }
        public Vector3 PreviousPosition { get { return previousPosition; } }
        public Vector3 CurrentPosition { get { return currentPosition; } }
    }
}
