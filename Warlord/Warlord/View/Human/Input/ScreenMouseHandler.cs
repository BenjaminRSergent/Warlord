using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Warlord.Application;
using Warlord.View.Human.Screens;

namespace Warlord.View.Human.Input
{
    class ScreenMouseHandler
    {
        private MouseState previousMouseState;
        private MouseState currentMouseState;

        Vector2 previousMouseLoc;
        Vector2 currentMouseLoc;

        private Screen screen;

        public ScreenMouseHandler(Screen screen)
        {
            this.screen = screen;
        }
        public void DispatchMouseInput()
        {
            currentMouseState = Mouse.GetState();
            previousMouseLoc = new Vector2(previousMouseState.X, previousMouseState.Y);   
            currentMouseLoc = new Vector2(currentMouseState.X, currentMouseState.Y);

            if(GlobalSystems.GameWindowHasFocus)
            { 
                if(currentMouseLoc != previousMouseLoc)
                    MouseMove();

                LButton();
                RButton();
                Wheel();            

                previousMouseState = currentMouseState;
            }
        }
        private void MouseMove( )
        {                     
            bool consumed;
            if(previousMouseLoc != currentMouseLoc)
            {
                consumed = false;
                foreach(MouseListener listener in screen.ScreenElements)
                {
                    if(listener.OnMouseMove(previousMouseLoc, currentMouseLoc))
                    {
                        consumed = true;
                        break;
                    }
                }

                if(!consumed)
                { 
                    foreach(MouseListener listener in screen.MouseListeners)
                    {
                        if(listener.OnMouseMove(previousMouseLoc, currentMouseLoc))
                            break;
                    }
                }
            }
        }
        private void LButton( )
        {
            bool consumed;
            if(currentMouseState.LeftButton == ButtonState.Pressed)
            {
                consumed = false;
                foreach(MouseListener listener in screen.ScreenElements)
                {
                    if(listener.OnLButtonDown( currentMouseLoc))
                    {
                        consumed = true;
                        break;
                    }
                }

                if(!consumed)
                { 
                    foreach(MouseListener listener in screen.MouseListeners)
                    {
                        if(listener.OnLButtonDown(currentMouseLoc))
                            break;
                    }
                }
            }
            else
            {
                consumed = false;
                foreach(MouseListener listener in screen.ScreenElements)
                {
                    if(listener.OnLButtonUp( currentMouseLoc))
                    {
                        consumed = true;
                        break;
                    }
                }

                if(!consumed)
                { 
                    foreach(MouseListener listener in screen.MouseListeners)
                    {
                        if(listener.OnLButtonUp(currentMouseLoc))
                            break;
                    }
                }
            }
        }
        private void RButton( )
        {
            bool consumed;
            if(currentMouseState.RightButton == ButtonState.Pressed)
            {
                consumed = false;
                foreach(MouseListener listener in screen.ScreenElements)
                {
                    if(listener.OnRButtonDown( currentMouseLoc))
                    {
                        consumed = true;
                        break;
                    }
                }

                if(!consumed)
                { 
                    foreach(MouseListener listener in screen.MouseListeners)
                    {
                        if(listener.OnRButtonDown(currentMouseLoc))
                            break;
                    }
                }
            }
            else
            {
                consumed = false;
                foreach(MouseListener listener in screen.ScreenElements)
                {
                    if(listener.OnRButtonUp( currentMouseLoc))
                    {
                        consumed = true;
                        break;
                    }
                }

                if(!consumed)
                { 
                    foreach(MouseListener listener in screen.MouseListeners)
                    {
                        if(listener.OnRButtonUp(currentMouseLoc))
                            break;
                    }
                }
            }
        }
        private void Wheel( )
        {
            int prevWheel = previousMouseState.ScrollWheelValue;
            int deltaWheel = currentMouseState.ScrollWheelValue - prevWheel;

            bool consumed = false;
            foreach(MouseListener listener in screen.ScreenElements)
            {
                if(listener.OnMouseWheel(deltaWheel))
                {
                    consumed = true;
                    break;
                }
            }

            if(!consumed)
            { 
                foreach(MouseListener listener in screen.MouseListeners)
                {
                    if(listener.OnMouseWheel(deltaWheel))
                    {
                        break;
                    }
                }
            }
        }        
    }
}
