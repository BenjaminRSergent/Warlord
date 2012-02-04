//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework.Input;

//namespace Warlord
//{
//    delegate void KeyboardReaction( Keys key );
//    class KeyboardInput
//    {
//        private KeyboardState prevKeyboardState;

//        bool listening;        

//        public KeyboardInput( )
//        {
//            GlobalApplication.Application.GameEventManager.Subscribe(ReadKeyboard,"read_input");
//        }

//        void ReadKeyboard( object sender, object data )
//        {
//            KeyboardState keyboard;

//            foreach(Keys key in keyboard.GetPressedKeys())
//            {
//                if( prevKeyboardState.IsKeyDown(key) )
//                    //Held Key Event
//                else
//                    //Pressed Key Event
//            }
            
//            foreach(Keys key in prevKeyboardState.GetPressedKeys())
//            {
//                if( !keyboard.IsKeyDown(key) )
//                    //Key Up Event
//            }

//            prevKeyboardState = keyboard;
//        }
//        public bool Listening
//        {
//            get { return listening; }
//            set { listening = value; }
//        }
//    }
//}
