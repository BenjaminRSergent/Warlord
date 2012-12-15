using Microsoft.Xna.Framework;
using Warlord.Interfaces.Subsystems;
using Warlord.View.Human.Input;
using Warlord.View.Human.Screens;

namespace Warlord.View.Human
{
    abstract class HumanView
        : GameView
    {
        private Screen currentScreen;
        private ScreenKeyboardHandler keyboardDispatcher;
        private ScreenMouseHandler mouseDispatcher;

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
