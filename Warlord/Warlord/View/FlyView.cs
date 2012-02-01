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

// Flagged for refactoring

namespace Warlord.View
{
    class FlyView : GameView
    {
        Camera3D camera;
        GameWindow gameWindow;
        GraphicsDevice device;

        Model testModel;

        volatile Dictionary<Region, RegionGraphics> regionGraphics;
        Vector4 warpPoints;

        Effect effect;

        public FlyView(GameWindow gameWindow, GraphicsDevice device, ContentManager content)
        {
            this.gameWindow = gameWindow;
            this.device = device;
            regionGraphics = new Dictionary<Region,RegionGraphics>( );
            camera = new Camera3D(gameWindow.ClientBounds, new Vector3(0, 80, 0), new Vector2((float)Math.PI, 0), Vector3.Up);

            TextureRepository.BlockTextures = content.Load<Texture2D>("Textures/Blocks/block_textures");
            effect = content.Load<Effect>("Fxs/block_effects");

            GlobalApplication.Application.GameEventManager.Subscribe(AddRegion, "region_added");
            GlobalApplication.Application.GameEventManager.Subscribe(RemoveRegion, "region_removed");
            GlobalApplication.Application.GameEventManager.Subscribe(Draw, "draw");

            GlobalApplication.Application.GameEventManager.Subscribe(MoveCamera, "camera_move_request");
            GlobalApplication.Application.GameEventManager.Subscribe(RotateCamera, "camera_rotate_request");

            testModel = content.Load<Model>("Models/test_dude");

            warpPoints = new Vector4(0, 32, 0, 1);
        }

        private void Draw(object sender, object gameTimeObject)
        {
            GameTime gameTime = gameTimeObject as GameTime;
            SetupBlockEffects();

            List<RegionGraphics> threadSafeGraphics = new List<RegionGraphics>( regionGraphics.Values );

            foreach(EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                
                foreach(RegionGraphics region in threadSafeGraphics)
                {
                    region.Update();
                    if(region.RegionMesh.Length > 0)
                    {
                        device.SetVertexBuffer(region.RegionBuffer);
                        device.DrawUserPrimitives(PrimitiveType.TriangleList, region.RegionMesh, 0, region.RegionMesh.Length / 3);
                    }
                }
                
            }

            foreach(ModelMesh mesh in testModel.Meshes)
            {
                foreach(IEffectMatrices meshEffect in mesh.Effects)
                { 
                    meshEffect.World = Matrix.CreateTranslation( new Vector3( 0, 80, 0 ) );
                    meshEffect.View = camera.View;
                    meshEffect.Projection = camera.Projection;
                }

                mesh.Draw( );
            }
            
        }
        private void SetupWarpEffects()
        {
            effect.Parameters["World"].SetValue(Matrix.CreateRotationY(angle));
            effect.Parameters["View"].SetValue(camera.View);
            effect.Parameters["Projection"].SetValue(camera.Projection);
            effect.Parameters["CameraPosition"].SetValue(camera.Position);
            effect.Parameters["AmbientColor"].SetValue(Color.White.ToVector4());
            effect.Parameters["AmbientIntensity"].SetValue(0.8f);
            effect.Parameters["FogColor"].SetValue(Color.Black.ToVector4());
            effect.Parameters["FogNear"].SetValue(0);
            effect.Parameters["FogFar"].SetValue(150);
            effect.Parameters["BlockTexture"].SetValue(TextureRepository.BlockTextures);
        }
        private void SetupBlockEffects()
        {
            GlobalApplication.Application.EntityManager.Player.Teleport(camera.Position);

            effect.Parameters["World"].SetValue(Matrix.Identity);
            effect.Parameters["View"].SetValue(camera.View);
            effect.Parameters["Projection"].SetValue(camera.Projection);
            effect.Parameters["CameraPosition"].SetValue(camera.Position);
            effect.Parameters["AmbientColor"].SetValue(Color.White.ToVector4());
            effect.Parameters["AmbientIntensity"].SetValue(0.8f);
            effect.Parameters["FogColor"].SetValue(Color.SkyBlue.ToVector4());
            effect.Parameters["FogNear"].SetValue(16 * 6f);
            effect.Parameters["FogFar"].SetValue(16 * 7f);
            effect.Parameters["BlockTexture"].SetValue(TextureRepository.BlockTextures);
        }
        private void AddRegion(object sender, object regionObject)
        {
            lock(regionGraphics)
            {
                if( !regionGraphics.ContainsKey( regionObject as Region) )
                regionGraphics.Add(regionObject as Region, new RegionGraphics(device, regionObject as Region));
            }
        }
        private void RemoveRegion(object sender, object regionObject)
        {
            lock(regionGraphics)
            {
                regionGraphics.Remove(regionObject as Region);
            }
        }
        private void RotateCamera(object sender, Object rotation)
        {
            RotateCamera((rotation as Vector2f).ToVector2);
        }
        private void MoveCamera(object sender, Object movement)
        {
            MoveCamera((movement as Vector3f).ToVector3);
        }
        private void RotateCamera(Vector2f rotation)
        {
            camera.ForceChangeRotation(rotation.ToVector2);
        }
        private void MoveCamera(Vector3f movement)
        {
            if(movement != Vector3f.Zero)
            {
                camera.ForceMoveFly(movement.ToVector3);
                GlobalApplication.Application.GameEventManager.SendEvent( new GameEvent(new GameTools.Optional<object>(this), "actor_moved", new Vector3f(camera.Position), 0 ));
            }
        }

        public float angle { get; set; }
    }
}
