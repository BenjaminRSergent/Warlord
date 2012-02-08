using System;
using Warlord.Interfaces.Subsystems;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Warlord.View.Human.Input;
using Microsoft.Xna.Framework.Input;

namespace Warlord.View.Human.Display
{
    abstract class HumanView
        : GameView
    {
        // Screen class?
        private Stack<ScreenElement> screenElements = new Stack<ScreenElement>();

        private List<KeyboardListener> keyboardListeners = new List<KeyboardListener>();
        private List<MouseListener> mouseListeners = new List<MouseListener>();

        protected MouseState prevMouseState;
        protected Keys[] keysDown = new Keys[0];

        public virtual void Draw(GameTime gameTime)
        {
            foreach(ScreenElement element in screenElements)
            {
                element.Draw(gameTime);
            }
        }

        protected void PushScreenElement(ScreenElement element)
        {
            screenElements.Push(element);
        }
        protected ScreenElement PopScreenElement()
        {
            return screenElements.Pop();
        }

        abstract public void Update(GameTime gameTime);

        virtual public void HandleInput()
        {
            HandleKeyboard();
            HandleMouse();
        }

        private void HandleKeyboard()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            foreach(Keys key in keysDown)
            {
                if(!keyboardState.IsKeyDown(key))
                {
                    foreach(ScreenElement element in screenElements)
                    {
                        if(element.OnKeyUp(key))
                            continue;
                    }

                    foreach(KeyboardListener listener in keyboardListeners)
                    {
                        if(listener.OnKeyUp(key))
                            continue;
                    }
                }

            }

            keysDown = keyboardState.GetPressedKeys();

            foreach(Keys key in keysDown)
            {
                foreach(ScreenElement element in screenElements)
                {
                    if(element.OnKeyDown(key))
                        continue;
                }

                foreach(KeyboardListener listener in keyboardListeners)
                {
                    if(listener.OnKeyDown(key))
                        continue;
                }
            }
        }
        private void HandleMouse()
        {
            MouseState state = Mouse.GetState();

            Vector2 prevMouseLoc = new Vector2(prevMouseState.X, prevMouseState.Y);
            Vector2 currMouseLoc = new Vector2(state.X, state.Y);

            if(prevMouseLoc != currMouseLoc)
            {
                foreach(MouseListener listener in mouseListeners)
                {
                    listener.OnMouseMove(prevMouseLoc, currMouseLoc);
                }
            }

        }
        protected List<KeyboardListener> KeyboardListeners
        {
            get { return keyboardListeners; }
        }
        protected List<MouseListener> MouseListeners
        {
            get { return mouseListeners; }
        }
    }
}
