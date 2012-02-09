using System;
using Warlord.Interfaces.Subsystems;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Warlord.View.Human.Input;
using Microsoft.Xna.Framework.Input;
using Warlord.View.Human.Screens;

namespace Warlord.View.Human.Display
{
    abstract class HumanView
        : GameView
    {
        private Screen currentScreen;
        ScreenKeyboardHandler keyboardDispatcher;
        ScreenMouseHandler mouseDispatcher;

        public virtual void Draw(GameTime gameTime)
        {
            foreach(ScreenElement element in currentScreen.ScreenElements)
            {
                element.Draw(gameTime);
            }
        }

        abstract public void Update(GameTime gameTime);

        virtual public void HandleInput()
        {
            keyboardDispatcher.DispatchKeyboardInput();
            mouseDispatcher.DispatchMouseInput();
        }

        protected Screen CurrentScreen
        {
            get { return CurrentScreen; }
            set
            {
                keyboardDispatcher = new ScreenKeyboardHandler(value);
                mouseDispatcher = new ScreenMouseHandler(value);
                currentScreen = value;
            }
        }
    }
}
