using Microsoft.Xna.Framework;

namespace GameTools
{
    public class Camera3D
    {
        private Vector3 position;
        private Vector3 up;

        private Vector2 rotation;

        private float aspectRatio;
        private float fov;
        private Vector2 drawDistance;

        public Camera3D(Rectangle clientBounds, Vector3 position, Vector2 initalFacingRotation, Vector3 up)
        {
            this.position = position;
            this.rotation = initalFacingRotation;
            this.up = up;

            aspectRatio = (float)clientBounds.Width / (float)clientBounds.Height;
            fov = 45;
            drawDistance = new Vector2(1, 300);

            BuildProjection();
        }
        public Camera3D(Rectangle clientBounds, Vector3 position, Vector2 initalFacingRotation, Vector3 up, Matrix projection)
        {
            this.position = position;
            this.rotation = initalFacingRotation;
            this.up = up;

            Projection = projection;
        }
        private void BuildProjection()
        {
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(fov),
                                                             aspectRatio,
                                                             drawDistance.X,
                                                             drawDistance.Y);
        }
        public void ForceMoveNoFly(Vector3 movement)
        {
            Matrix frontToSize = Matrix.CreateRotationY(-MathHelper.PiOver2);

            Vector3 effectiveFacing = Facing;
            effectiveFacing.Y = 0;

            Vector3 forwardMove = movement.Z * effectiveFacing;
            Vector3 sidewaysdMove = movement.X * Vector3.Transform(effectiveFacing, frontToSize);

            position += forwardMove;
            position += sidewaysdMove;

            position.Y += movement.Y;
        }
        public void ForceMoveFly(Vector3 movement)
        {
            Matrix frontToSide = Matrix.CreateRotationY(-MathHelper.PiOver2);

            Vector3 effectiveFacing = Facing;

            Vector3 forwardMove = movement.Z * effectiveFacing;
            Vector3 sidewaysdMove = movement.X * Vector3.Transform(effectiveFacing, frontToSide);

            position += forwardMove;
            position += sidewaysdMove;

            position.Y += movement.Y;
        }
        public void ForceChangeRotation(Vector2 facingChange)
        {
            const float cameraPadding = 0.2f;

            rotation += facingChange;

            if(rotation.Y > MathHelper.PiOver2 - cameraPadding)
                rotation.Y = MathHelper.PiOver2 - cameraPadding;

            if(rotation.Y < -MathHelper.PiOver2 + cameraPadding)
                rotation.Y = -MathHelper.PiOver2 + cameraPadding;
        }

        public void Teleport(Vector3 position)
        {
            this.position = position;
        }
        public void ResetView()
        {
            rotation = new Vector2(0, 0);
        }

        public Vector3 Position
        {
            get { return position; }
        }
        public Vector3 Facing
        {
            get { return Vector3.Transform(Vector3.Forward, Matrix.CreateRotationX(rotation.Y) * Matrix.CreateRotationY(rotation.X)); }
        }
        public Vector3 Up
        {
            get { return up; }
            set { up = value; }
        }

        public Matrix View
        {
            get
            {
                return Matrix.CreateLookAt(position, position + Facing, up);
            }
        }

        public float AspectRatio
        {
            get { return aspectRatio; }
            set
            {
                aspectRatio = value;
                BuildProjection();
            }
        }
        public float Fov
        {
            get { return fov; }
            set
            {
                fov = value;
                BuildProjection();
            }
        }
        public Vector2 DrawDistance
        {
            get { return drawDistance; }
            set
            {
                drawDistance = value;
                BuildProjection();
            }
        }
        public Matrix Projection { get; set; }
    }
}

