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

    class HumanInput : EventSubscriber
    {
        private KeyboardState prevKeyboardState;
        private MouseState prevMouseState;
        private GameWindow gameWindow;

        public HumanInput( GameWindow gameWindow )
        {    
            this.gameWindow = gameWindow;

            WarlordApplication.GameEventManager.Subscribe( this, "update" );
        }

        private void update( GameTime gameTime )
        {
            if( WarlordApplication.Active )
                CameraMoveRequest( );
        }
        private void CameraMoveRequest( )
        {
            KeyboardState keyState = Keyboard.GetState( );
            MouseState mouseState = Mouse.GetState( );

            Vector3f movement = Vector3.Zero;
            Vector2f facingRotation = Vector2.Zero;

            const float moveAmount = 0.25f;

            if( keyState.IsKeyDown( Keys.D ) )
                movement.X+= moveAmount;
            if( keyState.IsKeyDown( Keys.A ) )
                movement.X-= moveAmount;

            if( keyState.IsKeyDown( Keys.W ) )
                movement.Z += moveAmount;
            if( keyState.IsKeyDown( Keys.S ) )
                movement.Z-= moveAmount;

            if( keyState.IsKeyDown( Keys.Q ) )
                movement.Y+= moveAmount;
            if( keyState.IsKeyDown( Keys.E ) )
                movement.Y-= moveAmount;

            facingRotation.X = 0.001f * (gameWindow.ClientBounds.Width/2 - mouseState.X);
            facingRotation.Y = 0.001f * (gameWindow.ClientBounds.Height/2 - mouseState.Y);
            
            Mouse.SetPosition( gameWindow.ClientBounds.Width/2, gameWindow.ClientBounds.Height/2);

            WarlordApplication.GameEventManager.SendEvent( new GameEvent( new GameTools.Optional<object>(this),
                                                           "camera_move_request",
                                                           movement,
                                                           0 ));

            WarlordApplication.GameEventManager.SendEvent( new GameEvent( new GameTools.Optional<object>(this),
                                                           "camera_rotate_request",
                                                           facingRotation,
                                                           0 ));
        }

        public void HandleEvent(GameEvent theEvent)
        {
            if( theEvent.EventType == "update" )
                update( theEvent.AdditionalData as GameTime );
        }
    }
}
