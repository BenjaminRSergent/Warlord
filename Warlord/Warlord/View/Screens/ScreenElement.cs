using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Warlord.View.Human.Input;

namespace Warlord.View.Human.Screens
{
    abstract class ScreenElement : KeyboardListener, MouseListener
    {
        public ScreenElement()
        {
        }

        abstract public void Draw(GameTime gameTime);

        virtual public bool OnKeyDown(Keys key)
        {
            return false;
        }
        public bool OnKeyHeld(Keys key)
        {
            return false;
        }
        virtual public bool OnKeyUp(Keys key)
        {
            return false;
        }

        virtual public bool OnMouseMove(Vector2 prevPosition, Vector2 currentPosition)
        {
            return false;
        }

        virtual public bool OnLButtonDown(Vector2 location)
        {
            return false;
        }

        virtual public bool OnLButtonUp(Vector2 location)
        {
            return false;
        }

        virtual public bool OnRButtonDown(Vector2 location)
        {
            return false;
        }

        virtual public bool OnRButtonUp(Vector2 location)
        {
            return false;
        }

        virtual public bool OnMouseWheel(float deltaWheelMove)
        {
            return false;
        }
    }
}
