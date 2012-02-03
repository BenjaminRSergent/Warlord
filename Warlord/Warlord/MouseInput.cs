//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Input;

//namespace Warlord
//{
//    delegate void MouseMove( Vector2 normalizedMove );
//    delegate void WheelMove( int wheelMove );
//    delegate void MouseClick( );

//    class MouseInput
//    {
//        private MouseState prevMouseState;
//        bool listening;        

//        public MouseInput( )
//        {
//            GlobalApplication.Application.GameEventManager.Subscribe(ReadMouse,"read_input");
//        }

//        private void ReadMouse( object sender, object data )
//        {
//            MouseState mouseState = Mouse.GetState( );
                                    
//            ReadButtons( mouseState );            
//            ReadWheel( mouseState );
//            ReadMove( mouseState );         
                

//            prevMouseState = mouseState;
//        }        
//        private void ReadWheel( MouseState mouseState )
//        {
//            int wheelMove = mouseState.ScrollWheelValue - prevMouseState.ScrollWheelValue;

//            if( wheelMove != 0 )
//                OnWheelMove.Invoke( wheelMove );
//        }
//        private void ReadButtons( MouseState mouseState )
//        {
//            ButtonState prevLeftButton = prevMouseState.LeftButton;
//            ButtonState prevRightButton = prevMouseState.RightButton;

//            ButtonState currentLeftButton = mouseState.LeftButton;
//            ButtonState currentRightButton = mouseState.RightButton;

//            if(currentLeftButton == ButtonState.Pressed)
//            {
//                if(prevMouseState.LeftButton == ButtonState.Released)
//                    EventMan
//                else
//                    OnLeftButtonHeld.Invoke();
//            }
//            else
//            {
//                if( prevMouseState.LeftButton == ButtonState.Pressed )
//                    OnLeftButtonUp( );
//            }

//            if(currentRightButton == ButtonState.Pressed)
//            {
//                if(prevMouseState.RightButton == ButtonState.Released)
//                    OnRightButtonDown.Invoke();
//                else
//                    OnRightButtonHeld.Invoke();
//            }
//            else
//            {
//                if( prevMouseState.RightButton == ButtonState.Pressed )
//                    OnRightButtonUp( );
//            }
//        }
//        private void ReadMove(MouseState mouseState)
//        {
//            if( mouseState.X != prevMouseState.X ||
//                mouseState.X != prevMouseState.X )
//            {
//                Vector2 mouseMove = new Vector2(mouseState.X - prevMouseState.X,
//                                                mouseState.Y - prevMouseState.Y );

//                mouseMove.Normalize( );

//                OnMouseMove.Invoke(mouseMove);
//            }
//        }

//        public bool Listening
//        {
//            get { return listening; }
//            set { listening = value; }
//        }
//    }
//}
