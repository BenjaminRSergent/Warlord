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
        bool ctrlIsDown;

        public DebugMovementController(Camera3D camera, GameWindow gameWindow)
        {
            this.camera = camera;
            this.gameWindow = gameWindow;
        }
        public bool OnKeyDown(Keys key)
        {
            return Move(key);
        }
        public bool OnKeyHeld(Keys key)
        {
            return Move(key);
        }
        public bool OnKeyUp(Keys key)
        {
            switch(key)
            {
                case Keys.LeftShift:
                    shiftIsDown = false;
                    return true;
                case Keys.LeftControl:                
                    shiftIsDown = false;
                    return true;
            }

            return false;
        }
        public bool OnMouseMove(Vector2 prevPosition, Vector2 currentPosition)
        {
            Point absolutePosition = new Point(gameWindow.ClientBounds.Left+(int)currentPosition.X,
                                               gameWindow.ClientBounds.Top+(int)currentPosition.Y);

            System.Drawing.Rectangle window = new System.Drawing.Rectangle(gameWindow.ClientBounds.Left,
                                                                           gameWindow.ClientBounds.Top,
                                                                           gameWindow.ClientBounds.Right,
                                                                           gameWindow.ClientBounds.Bottom);
            
            Vector2 centerOfScreen = new Vector2(gameWindow.ClientBounds.Width / 2,
                                                  gameWindow.ClientBounds.Height / 2);

            if(!gameWindow.ClientBounds.Contains(absolutePosition))
            { 
                Mouse.SetPosition((int)centerOfScreen.X, (int)centerOfScreen.Y);
                ClipCursor(ref window);
                return true;
            }

            Vector2 facingRotation;
            Vector2 mouseMove = centerOfScreen - currentPosition;

            Vector2.Clamp(mouseMove, Vector2.Zero, 2*centerOfScreen);

            facingRotation.X = 0.001f * mouseMove.X;
            facingRotation.Y = 0.001f * mouseMove.Y;

            Mouse.SetPosition((int)centerOfScreen.X, (int)centerOfScreen.Y);
            
            ClipCursor(ref window);

            camera.ChangeRotation(facingRotation);

            return true;
        }
        private bool Move(Keys key)
        {
            float moveSpeed = 0.25f;

            if(shiftIsDown)
                moveSpeed *= 2.0f;
            if(ctrlIsDown)
                moveSpeed *= 0.2f;

            switch(key)
            {
                case Keys.LeftShift:
                    shiftIsDown = true;
                    return true;
                case Keys.LeftControl:
                    ctrlIsDown = true;
                    return true;
                case Keys.W:
                    camera.MoveFly(new Vector3(0, 0, moveSpeed));
                    return true;
                case Keys.A:
                    camera.MoveFly(new Vector3(-moveSpeed, 0, 0));
                    return true;
                case Keys.S:
                    camera.MoveFly(new Vector3(0, 0, 0 - moveSpeed));
                    return true;
                case Keys.D:
                    camera.MoveFly(new Vector3(moveSpeed, 0, 0));
                    return true;

                case Keys.Q:
                    camera.MoveFly(new Vector3(0, -moveSpeed, 0));
                    return true;
                case Keys.E:
                    camera.MoveFly(new Vector3(0, moveSpeed, 0));
                    return true;
            }

            return false;
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
