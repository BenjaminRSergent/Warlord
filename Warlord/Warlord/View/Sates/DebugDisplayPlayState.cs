﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warlord.Logic.Data;
using Warlord.View.Human.Display;
using GameTools.State;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameTools;
using Microsoft.Xna.Framework.Content;
using GameTools.Graph;
using Warlord.Event;

namespace Warlord.View.Sates
{
    class DebugDisplayPlayState : State<DebugDisplay>
    {
        Dictionary<Region, RegionGraphics> regionGraphics;
        GameWindow gameWindow;
        GraphicsDevice graphics;

        Camera3D camera;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        Effect effect;

        public DebugDisplayPlayState( DebugDisplay owner,
                                      GameWindow gameWindow,
                                      GraphicsDevice graphics,
                                      SpriteBatch spriteBatch,
                                      ContentManager content) : base( owner )
        {
            this.gameWindow = gameWindow;
            this.graphics = graphics;
            this.spriteBatch = spriteBatch;
            
            camera = new Camera3D(gameWindow.ClientBounds, new Vector3(0, 80, 0), new Vector2((float)Math.PI, 0), Vector3.Up);

            effect = content.Load<Effect>("Fxs/block_effects");

            GlobalApplication.Application.GameEventManager.Subscribe(AddRegion, "region_added");
            GlobalApplication.Application.GameEventManager.Subscribe(RemoveRegion, "region_removed");            

            GlobalApplication.Application.GameEventManager.Subscribe(MoveCamera, "camera_move_request");
            GlobalApplication.Application.GameEventManager.Subscribe(RotateCamera, "camera_rotate_request");

            spriteBatch = new SpriteBatch(graphics);
            spriteFont = content.Load<SpriteFont>("Font/DebugFont");
        }

        public override void EnterState()
        {
            regionGraphics = new Dictionary<Region,RegionGraphics>( );
        }

        public override void Update()
        {
            graphics.Clear(Color.SkyBlue);

            if(regionGraphics.Count > 16)
                DrawWorld();
            else
            {
                RasterizerState rs = graphics.RasterizerState;
                DepthStencilState ds = graphics.DepthStencilState;
                BlendState bs = graphics.BlendState;
                graphics.Clear(Color.Black);
                spriteBatch.Begin( );
                spriteBatch.DrawString( spriteFont, "starting the world..", new Vector2( gameWindow.ClientBounds.Width/3, gameWindow.ClientBounds.Height/2), Color.White);
                spriteBatch.End( );
                graphics.RasterizerState = rs;
                graphics.DepthStencilState = ds;
                graphics.BlendState = bs;
                    
            }
        }
        public override void ExitState()
        {
            regionGraphics.Clear( );
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
            effect.Parameters["FogNear"].SetValue(16 * 7f);
            effect.Parameters["FogFar"].SetValue(16 * 8f);
            effect.Parameters["BlockTexture"].SetValue(TextureRepository.BlockTextures);
        }
        private void DrawWorld( )
        {
            SetupBlockEffects();

            List<RegionGraphics> threadSafeGraphics = new List<RegionGraphics>( regionGraphics.Values );

            BoundingFrustum frustum = new BoundingFrustum( camera.View * camera.Projection );

            threadSafeGraphics = threadSafeGraphics.Where( region => region.IsInVolume(frustum) ).ToList( );

            foreach(EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                
                foreach(RegionGraphics region in threadSafeGraphics)
                {                    
                    region.Update( );

                    if(region.Clean && region.RegionMesh.Length > 0)
                    {
                        graphics.SetVertexBuffer(region.RegionBuffer);
                        graphics.DrawUserPrimitives(PrimitiveType.TriangleList, region.RegionMesh, 0, region.RegionMesh.Length / 3);
                    }
                }
            
            }
        }
        private void AddRegion(object sender, object regionObject)
        {
            lock(regionGraphics)
            {
                if( !regionGraphics.ContainsKey( regionObject as Region) )
                regionGraphics.Add(regionObject as Region, new RegionGraphics(graphics, regionObject as Region));
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
    }
}
