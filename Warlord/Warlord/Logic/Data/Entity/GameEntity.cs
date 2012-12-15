using System.Diagnostics;
using Microsoft.Xna.Framework;
using GameTools;

namespace Warlord.Logic.Data.Entity
{
    class GameEntity
    {
        private Optional<uint> id;

        private float mass;
        private float scale;
        private EntityType type;
        private Vector3 velocity;
        private Vector3 sumOfForces;
        private Vector3 previousPosition;
        private Vector3 currentPosition;
        private Vector3 rotation;        

        private BoundingBox originalboundingBox;
        private BoundingBox currentBoundingBox;
        private Vector3 facing;

        public GameEntity(Vector3 position, EntityType type, float mass, float scale)
        {
            Debug.Assert(mass > 0);

            this.previousPosition = position;
            this.currentPosition = position;
            this.mass = mass;
            this.scale = scale;
            this.type = type;

            this.originalboundingBox = EntityBoundingBoxes.GetBox(type);

            UpdateCurrentBoundingBox( );

            id = new Optional<uint>();
        }
        public void Update(GameTime gameTime)
        {            
            float seconds = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;

            if( seconds == 0 )
                return;

            velocity += sumOfForces/mass*seconds;
            currentPosition += velocity * seconds;

            facing = velocity;
            facing.Normalize();

            UpdateCurrentBoundingBox();
        }
        public Vector3 GetFuturePosition(GameTime gameTime)
        {
            Vector3 futurePosition;
            Vector3 futureVelocity;
            float seconds = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;

            if( seconds == 0 )
                return currentPosition;

            futureVelocity = velocity + sumOfForces/mass*seconds;

            futurePosition = currentPosition + velocity * seconds;

            return futurePosition;
        }
        public void Teleport(Vector3 location)
        {
            previousPosition = currentPosition;
            this.currentPosition = location;

            UpdateCurrentBoundingBox();
        }
        public void Rotate( Vector3 rotation )
        {
            this.rotation *= rotation;
        }
        public void SetAbsoluteRotate( Vector3 rotation )
        {
            this.rotation = rotation;
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
        private void UpdateCurrentBoundingBox()
        {
            Vector3 min = originalboundingBox.Min;
            Vector3 max = originalboundingBox.Max;


            Matrix transform = Matrix.Identity;
            transform *= Matrix.CreateRotationX(rotation.Z);
            transform *= Matrix.CreateRotationY(rotation.X);
            transform *= Matrix.CreateRotationZ(rotation.Y);
            transform *= Matrix.CreateScale(scale);
            transform *= Matrix.CreateTranslation(currentPosition);

            min = Vector3.Transform(min, transform);
            max = Vector3.Transform(max, transform);

            currentBoundingBox = new BoundingBox(min, max);
        }
        public uint entityID { get { Debug.Assert(id.Valid); return id.Data; } }
        public float Mass { get { return mass; } set { mass = value; } }
        public Vector3 Velocity { get { return velocity; } set { velocity = value; } }
        public Vector3 SumOfForces { get { return sumOfForces; } set { sumOfForces = value; } }
        public Vector3 PreviousPosition { get { return previousPosition; } }
        public Vector3 CurrentPosition { get { return currentPosition; } }
        protected Vector3 Rotation { get { return rotation; } }
        public BoundingBox CurrentBoundingBox { get { return currentBoundingBox; } }
        public float Scale { get{ return scale;} }

        public Vector3 Facing { get {return facing;} }
    }
}