using System;
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
        Queue<KeyValuePair<Region, RegionGraphics>> toRemove;

        public WorldGraphics( GraphicsDevice graphics, GameWindow gameWindow, ContentManager content, Camera3D camera )
        {
            this.graphics = graphics;
            regionGraphics = new Dictionary<Region,RegionGraphics>( );

            toAdd = new Queue<KeyValuePair<Region,RegionGraphics>>( );
            toRemove = new Queue<KeyValuePair<Region,RegionGraphics>>( );
            
            this.camera = camera;

            effect = content.Load<Effect>("Fxs/block_effects");

            GlobalApplication.Application.GameEventManager.Subscribe(AddRegion, "region_created");
            GlobalApplication.Application.GameEventManager.Subscribe(RemoveRegion, "region_removed");
        }

        public override void Draw(GameTime gameTime)
        {
            DrawWorld( );
        }

        private void DrawWorld( )
        {
            SetupBlockEffects();
            UpdateRegionGraphicsPairs( );
                       
            BoundingFrustum frustum = new BoundingFrustum( camera.View * camera.Projection );            

            foreach(EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                
                foreach(RegionGraphics region in regionGraphics.Values)
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
        private void UpdateRegionGraphicsPairs( )
        {
            KeyValuePair<Region, RegionGraphics> currentPair;

            lock(toAdd)
            { 
                while(toAdd.Count > 0)
                {
                    currentPair = toAdd.Dequeue( );
                    regionGraphics.Add(currentPair.Key, currentPair.Value);
                }
            }
            lock(toRemove)
            { 
                while(toRemove.Count > 0)
                {                    
                    currentPair = toRemove.Dequeue( );
                    currentPair.Value.Dispose( );
                    regionGraphics.Remove(currentPair.Key);
                }
            }
        }
        private void AddRegion(BaseGameEvent theEvent)
        {   
            Region newRegion = (theEvent as RegionCreatedEvent).Data.newRegion;

            KeyValuePair<Region, RegionGraphics> newRegionGraphicsPair;

            newRegionGraphicsPair = new KeyValuePair<Region, RegionGraphics>
                (newRegion, new RegionGraphics(graphics, newRegion));

            if(!regionGraphics.ContainsKey(newRegion))
            {
                lock(toAdd)
                { 
                    toAdd.Enqueue(newRegionGraphicsPair);
                }
            }
            
        }
        private void RemoveRegion(BaseGameEvent theEvent)
        {
            Region deadRegion = (theEvent as RegionRemovedEvent).Data.deadRegion;

            KeyValuePair<Region, RegionGraphics> deadRegionGraphicsPair;
            deadRegionGraphicsPair = new KeyValuePair<Region, RegionGraphics>
                (deadRegion, new RegionGraphics(graphics, deadRegion));

            lock(toRemove)
            { 
                toRemove.Enqueue(new KeyValuePair<Region, RegionGraphics>
                    (deadRegion, new RegionGraphics(graphics, deadRegion)));
            }
        }

     
    }
}
