using System.Runtime.InteropServices;
using GameTools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Warlord.View.Human.Input
{
    class DebugMovementController : KeyboardListener, MouseListener
    {
        [DllImport("user32.dll")]
        static extern bool ClipCursor(ref System.Drawing.Rectangle lpRect);

        Camera3D camera;
        GameWindow gameWindow;

        bool shiftIsDown;

        public DebugMovementController(Camera3D camera, GameWindow gameWindow)
        {
            this.camera = camera;
            this.gameWindow = gameWindow;
        }
        public bool OnKeyDown(Keys key)
        {
            // a real controller tells its actor where to try to move;
            // it's the brains not the legs.            

            float moveSpeed = 0.06f;

            if(shiftIsDown)
                moveSpeed *= 10;

            switch(key)
            {
                case Keys.LeftShift:
                    shiftIsDown = true;
                    return true;
                case Keys.W:
                    camera.ForceMoveFly(new Vector3(0, 0, moveSpeed));
                    return true;
                case Keys.A:
                    camera.ForceMoveFly(new Vector3(-moveSpeed, 0, 0));
                    return true;
                case Keys.S:
                    camera.ForceMoveFly(new Vector3(0, 0, 0 - moveSpeed));
                    return true;
                case Keys.D:
                    camera.ForceMoveFly(new Vector3(moveSpeed, 0, 0));
                    return true;

                case Keys.Q:
                    camera.ForceMoveFly(new Vector3(0, -moveSpeed, 0));
                    return true;
                case Keys.E:
                    camera.ForceMoveFly(new Vector3(0, moveSpeed, 0));
                    return true;
            }

            return false;
        }
        public bool OnKeyUp(Keys key)
        {
            switch(key)
            {
                case Keys.LeftShift:
                    shiftIsDown = false;
                    return true;
            }

            return false;
        }
        public bool OnMouseMove(Vector2 prevPosition, Vector2 currentPosition)
        {
            Vector2 centerOfScreen = new Vector2( gameWindow.ClientBounds.Width / 2,
                                                  gameWindow.ClientBounds.Height / 2 );
            Vector2 facingRotation;
            Vector2 mouseMove = centerOfScreen - currentPosition;

            facingRotation.X = 0.001f * mouseMove.X;
            facingRotation.Y = 0.001f * mouseMove.Y;

            Mouse.SetPosition((int)centerOfScreen.X,(int)centerOfScreen.Y);

            System.Drawing.Rectangle window = new System.Drawing.Rectangle(gameWindow.ClientBounds.Left,
                                                                            gameWindow.ClientBounds.Top,
                                                                            gameWindow.ClientBounds.Right,
                                                                            gameWindow.ClientBounds.Bottom);
            ClipCursor(ref window);

            camera.ForceChangeRotation(facingRotation);            

            return true;
        }
        public bool OnLButtonDown(Vector2 location)
        {
            return false;
        }
        public bool OnLButtonUp(Vector2 location)
        {
            return false;
        }
        public bool OnRButtonDown(Vector2 location)
        {
            return false;
        }
        public bool OnRButtonUp(Vector2 location)
        {
            return false;
        }
        public bool OnMouseWheel(float deltaWheelMove)
        {
            return false;
        }
    }
}
