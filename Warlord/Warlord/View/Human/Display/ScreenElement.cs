using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Warlord.View.Human.Input;

namespace Warlord.View.Human.Display
{
    abstract class ScreenElement : KeyboardListener, MouseListener
    {
        private int zOrder;

        public ScreenElement( )
        {            
            this.zOrder = 0;
        }
        public ScreenElement( int zOrder )
        {            
            this.zOrder = zOrder;
        }

        abstract public void Draw( GameTime gameTime );      
        
        virtual public bool OnKeyDown(Keys key)
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

        public int ZOrder
        {
            get { return zOrder; }            
        }

    }
}
