using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warlord.Interfaces.Subsystems;
using GameTools;
using Microsoft.Xna.Framework;
using Warlord.Event;
using Warlord.Logic.Data;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameTools.Graph;

namespace Warlord.View
{
    class FlyView : GameView, EventSubscriber
    {
        Camera3D camera;
        GameWindow gameWindow;
        GraphicsDevice device;        

        List<RegionGraphics> regionGraphics;

        Effect effect;

        public FlyView( GameWindow gameWindow, GraphicsDevice device, ContentManager content )
        {
            this.gameWindow = gameWindow;
            this.device = device;
            regionGraphics = new List<RegionGraphics>( );
            camera = new Camera3D( gameWindow.ClientBounds, new Vector3( 20, 80, -20 ), new Vector2((float)Math.PI, 0), Vector3.Up );            

            TextureRepository.BlockTextures = content.Load<Texture2D>("Textures/Blocks/block_textures");
            effect = content.Load<Effect>("Fxs/effects");

            WarlordApplication.GameEventManager.Subscribe( this, "region_added" );
            WarlordApplication.GameEventManager.Subscribe( this, "draw" );

            WarlordApplication.GameEventManager.Subscribe( this, "camera_move_request" );
            WarlordApplication.GameEventManager.Subscribe( this, "camera_rotate_request" );
        }

        private void Draw(GameTime gameTime)
        {
            SetupEffects( );

            foreach( EffectPass pass in effect.CurrentTechnique.Passes )
            {
                pass.Apply( );
                foreach( RegionGraphics region in regionGraphics )
                {
                    region.Update( );
                    device.SetVertexBuffer(region.RegionBuffer);
                    device.DrawUserPrimitives(PrimitiveType.TriangleList, region.RegionMesh, 0, region.RegionMesh.Length/3);
                }
            }
        }

        private void SetupEffects( )
        {
            effect.Parameters["World"].SetValue( Matrix.CreateRotationY( angle ) );
            effect.Parameters["View"].SetValue(camera.View);
            effect.Parameters["Projection"].SetValue(camera.Projection);
            effect.Parameters["CameraPosition"].SetValue(camera.Position);
            effect.Parameters["AmbientColor"].SetValue(Color.White.ToVector4());
            effect.Parameters["AmbientIntensity"].SetValue(0.8f);
            effect.Parameters["FogColor"].SetValue(Color.SkyBlue.ToVector4());
            effect.Parameters["BlockTexture"].SetValue(TextureRepository.BlockTextures);
        }

        public void HandleEvent(GameEvent theEvent)
        {
            if( theEvent.EventType == "region_added" )
                regionGraphics.Add( new RegionGraphics( device, theEvent.AdditionalData as Region ) );
            else if( theEvent.EventType == "draw" )
                Draw( theEvent.AdditionalData as GameTime );
            else if( theEvent.EventType == "camera_move_request" )
                MoveCamera( theEvent.AdditionalData as Vector3f );
            else if( theEvent.EventType == "camera_rotate_request" )
                RotateCamera( theEvent.AdditionalData as Vector2f );
        }

        private void RotateCamera(Vector2f rotation)
        {
            camera.ForceChangeRotation( rotation.ToVector2 );
        }

        private void MoveCamera(Vector3f movement)
        {
            camera.ForceMoveFly( movement.ToVector3 );
        }


        public float angle { get; set; }
    }
}
