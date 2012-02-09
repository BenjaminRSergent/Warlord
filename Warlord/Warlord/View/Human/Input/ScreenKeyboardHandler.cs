using Microsoft.Xna.Framework.Input;
using Warlord.Application;
using Warlord.View.Human.Screens;

namespace Warlord.View.Human.Input
{
    class ScreenKeyboardHandler
    {
        private KeyboardState previousKeyboardState;
        private KeyboardState currentKeyboardState;

        private Screen screen;

        public ScreenKeyboardHandler(Screen screen)
        {
            this.screen = screen;
        }
        public void DispatchKeyboardInput( )
        {
            currentKeyboardState = Keyboard.GetState();

            if(GlobalSystems.GameWindowHasFocus)
            { 
                KeysUp( );
                KeysDown( );
            }

            previousKeyboardState = currentKeyboardState;
        }
        private void KeysUp( )
        {
            Keys[] previousKeysDown = previousKeyboardState.GetPressedKeys();

            bool consumed;
            foreach(Keys key in previousKeysDown)
            {
                consumed = false;
                if(currentKeyboardState.IsKeyUp(key))
                {
                    foreach(KeyboardListener listener in screen.ScreenElements)
                    {
                        if(listener.OnKeyUp(key))
                        {
                            consumed = true;
                            break;
                        }
                    }

                    if( consumed )
                        continue;

                    foreach(KeyboardListener listener in screen.KeyboardListeners)
                    {
                        if(listener.OnKeyUp(key))
                        {
                            break;
                        }
                    }
                }
            }
        }
        private void KeysDown( )
        {
            Keys[] currentKeysDown = currentKeyboardState.GetPressedKeys();

            bool consumed;
            foreach(Keys key in currentKeysDown)
            {
                consumed = false;

                foreach(KeyboardListener listener in screen.ScreenElements)
                {
                    if(listener.OnKeyDown(key))
                    {
                        consumed = true;
                        break;
                    }
                }

                if( consumed )
                    continue;

                foreach(KeyboardListener listener in screen.KeyboardListeners)
                {
                    if(listener.OnKeyDown(key))
                    {
                        break;
                    }
                }
            }
        }
    }
}
