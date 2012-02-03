using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Warlord.Event;
using Microsoft.Xna.Framework;
using GameTools.Graph;

namespace Warlord.View
{
    //Flagged for revise

    class FlyInput
    {
        private KeyboardState prevKeyboardState;
        private MouseState prevMouseState;

        private GameWindow gameWindow;

        public FlyInput( GameWindow gameWindow )
        {    
            this.gameWindow = gameWindow;

            GlobalApplication.Application.GameEventManager.Subscribe( update, "update" );
        }
        private void update( object sender, object gameTime )
        {
            update( gameTime as GameTime );
        }
        private void update( GameTime gameTime )
        {
            KeyboardState keyboardState;
            MouseState mouseState;

            if( GlobalApplication.Application.Active )
            {
                keyboardState = Keyboard.GetState( );
                mouseState = Mouse.GetState( );

                CameraMoveRequest( keyboardState, mouseState );

                prevKeyboardState = keyboardState;
                prevMouseState = mouseState;
            }
        }
        private void CameraMoveRequest( KeyboardState keyboardState, MouseState mouseState )
        {
            Vector3f movement = Vector3.Zero;
            Vector2f facingRotation = Vector2.Zero;

            const float moveAmount = 0.1f;

            if( keyboardState.IsKeyDown( Keys.D ) )
                movement.X+= moveAmount;
            if( keyboardState.IsKeyDown( Keys.A ) )
                movement.X-= moveAmount;

            if( keyboardState.IsKeyDown( Keys.W ) )
                movement.Z += moveAmount;
            if( keyboardState.IsKeyDown( Keys.S ) )
                movement.Z-= moveAmount;

            if( keyboardState.IsKeyDown( Keys.Q ) )
                movement.Y+= moveAmount;
            if( keyboardState.IsKeyDown( Keys.E ) )
                movement.Y-= moveAmount;

            if( keyboardState.IsKeyDown( Keys.LeftShift ) )
                movement = 10 * movement;

            if( keyboardState.IsKeyDown( Keys.LeftControl ) )
                movement = 10 * movement;

            facingRotation.X = 0.001f * (gameWindow.ClientBounds.Width/2 - mouseState.X);
            facingRotation.Y = 0.001f * (gameWindow.ClientBounds.Height/2 - mouseState.Y);
            
            Mouse.SetPosition( gameWindow.ClientBounds.Width/2, gameWindow.ClientBounds.Height/2);

            GlobalApplication.Application.GameEventManager.SendEvent( new GameEvent( new GameTools.Optional<object>(this),
                                                           "camera_move_request",
                                                           movement,
                                                           0 ));

            GlobalApplication.Application.GameEventManager.SendEvent( new GameEvent( new GameTools.Optional<object>(this),
                                                           "camera_rotate_request",
                                                           facingRotation,
                                                           0 ));
        }
    }
}
