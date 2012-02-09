﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warlord.Logic.Data;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameTools;
using Warlord.Event;
using Warlord.Event.EventTypes;
using Microsoft.Xna.Framework.Content;

namespace Warlord.View.Human.Display
{
    class WorldGraphics : ScreenElement
    {
        private Dictionary<Region, RegionGraphics> regionGraphics;

        private GraphicsDevice graphics;

        private Camera3D camera;
        private Effect effect;

        Queue<KeyValuePair<Region, RegionGraphics>> toAdd;

        public WorldGraphics(GraphicsDevice graphics, GameWindow gameWindow, ContentManager content, Camera3D camera)
        {
            this.graphics = graphics;
            regionGraphics = new Dictionary<Region, RegionGraphics>();

            toAdd = new Queue<KeyValuePair<Region, RegionGraphics>>();

            this.camera = camera;

            effect = content.Load<Effect>("Fxs/block_effects");

            GlobalSystems.EventManager.Subscribe(AddRegion, "region_created");
        }

        public override void Draw(GameTime gameTime)
        {
            DrawWorld();
        }

        private void DrawWorld()
        {
            SetupBlockEffects();
            UpdateRegionGraphicsPairs();

            BoundingFrustum frustum = new BoundingFrustum(camera.View * camera.Projection);

            foreach(EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                foreach(RegionGraphics region in regionGraphics.Values)
                {
                    if(region.IsInVolume(frustum))
                    { 
                        region.Update();

                        if(region.Clean && region.RegionMesh.Length > 0)
                        {
                            graphics.SetVertexBuffer(region.RegionBuffer);
                            graphics.DrawUserPrimitives(PrimitiveType.TriangleList, region.RegionMesh, 0, region.NumVertices / 3);
                        }
                    }
                }

            }
        }
        private void SetupBlockEffects()
        {
            GlobalSystems.EntityManager.Player.Teleport(camera.Position);

            effect.Parameters["World"].SetValue(Matrix.Identity);
            effect.Parameters["View"].SetValue(camera.View);
            effect.Parameters["Projection"].SetValue(camera.Projection);
            effect.Parameters["CameraPosition"].SetValue(camera.Position);
            effect.Parameters["AmbientColor"].SetValue(Color.White.ToVector4());
            effect.Parameters["AmbientIntensity"].SetValue(0.8f);
            effect.Parameters["FogColor"].SetValue(Color.SkyBlue.ToVector4());
            effect.Parameters["FogNear"].SetValue(16 * 9.5f);
            effect.Parameters["FogFar"].SetValue(16 * 10f);
            effect.Parameters["BlockTexture"].SetValue(TextureRepository.BlockTextures);
        }
        private void UpdateRegionGraphicsPairs()
        {
            KeyValuePair<Region, RegionGraphics> currentPair;

            lock(toAdd)
            {
                while(toAdd.Count > 0)
                {
                    currentPair = toAdd.Dequeue();
                    regionGraphics.Add(currentPair.Key, currentPair.Value);
                }
            }
        }
        private void AddRegion(BaseGameEvent theEvent)
        {
            Region newRegion = (theEvent as RegionCreatedEvent).Data.newRegion;
            KeyValuePair<Region, RegionGraphics> newRegionGraphicsPair;            

            if(!regionGraphics.ContainsKey(newRegion))
            {
                newRegionGraphicsPair = new KeyValuePair<Region, RegionGraphics>
                    (newRegion, new RegionGraphics(graphics, newRegion));

                lock(toAdd)
                {
                    toAdd.Enqueue(newRegionGraphicsPair);
                }
            }

        }
    }
}
