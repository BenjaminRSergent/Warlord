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
    class FlyView : GameView
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

            WarlordApplication.GameEventManager.Subscribe( AddRegion, "region_added" );
            WarlordApplication.GameEventManager.Subscribe( Draw, "draw" );

            WarlordApplication.GameEventManager.Subscribe( MoveCamera, "camera_move_request" );
            WarlordApplication.GameEventManager.Subscribe( RotateCamera, "camera_rotate_request" );
        }

        private void Draw(object sender, object gameTimeObject)
        {
            GameTime gameTime = gameTimeObject as GameTime;
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

        private void AddRegion(object sender, object regionObject )
        {
            regionGraphics.Add( new RegionGraphics( device, regionObject as Region ) );
        }
        private void RotateCamera(object sender, Object rotation)
        {
            RotateCamera( (rotation as Vector2f).ToVector2 );
        }
        private void MoveCamera(object sender, Object movement)
        {
            MoveCamera( (movement as Vector3f).ToVector3 );
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
