using System.Collections.Generic;
using GameTools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Warlord.Application;
using Warlord.Event;
using Warlord.Event.EventTypes;
using Warlord.Logic.Data;
using Warlord.View.Human.Display;
using Warlord.GameTools;
using XNAGraphicsHelper;
using Warlord.Logic.Data.World;

namespace Warlord.View.Human.Screens
{
    class WorldGraphics : ScreenElement
    {
        private Dictionary<Region, RegionGraphics> regionGraphics;

        private GraphicsDevice graphics;

        private Camera3D camera;
        private Effect effect;
        private Vector2 fog;        
        private bool synchronising;
        private WireFrameDrawer wireframeDrawer;
        Queue<KeyValuePair<Region, RegionGraphics>> toAdd;

        public WorldGraphics(GraphicsDevice graphics, GameWindow gameWindow, ContentManager content, Camera3D camera)
        {
            this.graphics = graphics;
            this.camera = camera;
            this.fog = new Vector2(110, 120);

            regionGraphics = new Dictionary<Region, RegionGraphics>();
            toAdd = new Queue<KeyValuePair<Region, RegionGraphics>>();
            wireframeDrawer = new WireFrameDrawer(graphics);

            effect = content.Load<Effect>("Fxs/block_effects");

            GlobalSystems.EventManager.Subscribe(AddRegion, "region_created");
            GlobalSystems.EventManager.Subscribe(RenewRegion, "sending_region_list");

            SynchroniseAllRegions();
        }

        public override void Draw(GameTime gameTime)
        {
            DrawWorld();
        }

        private void DrawWorld()
        {
            SetupBlockEffects();
            UpdateRegionGraphicsPairs();

            BoundingFrustum frustum = GetExpandedCameraFrustum(0.05f);

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
        private BoundingFrustum GetExpandedCameraFrustum(float percentExpand)
        {
            float originalFov = camera.Fov;
            camera.Fov = originalFov * (1 + percentExpand);
            BoundingFrustum frustum = new BoundingFrustum(camera.View * camera.Projection);
            camera.Fov = originalFov;

            return frustum;
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
            effect.Parameters["FogNear"].SetValue(fog.X);
            effect.Parameters["FogFar"].SetValue(fog.Y);
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

            if( regionGraphics.Count > 500 )
                SynchroniseAllRegions();
        }
        private void AddRegion(BaseGameEvent theEvent)
        {
            Region newRegion = (theEvent as RegionCreatedEvent).Data.newRegion;
            AddRegion(newRegion);
        }
        private void AddRegion(Region newRegion)
        {
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
        private void RenewRegion(BaseGameEvent theEvent)
        {
            if(synchronising)
            {
                synchronising = false;

                foreach(Region region in (theEvent as SendingRegionListEvent).CurrentRegionList)
                {
                    AddRegion(region);
                }
            }
        }
        private void SynchroniseAllRegions()
        {
            synchronising = true;
            regionGraphics.Clear();

            GlobalSystems.EventManager.SendEvent(new RefreshRegionGraphicsEvent(new Optional<object>(this), 0));
        }

        public Vector2 Fog
        {
            get
            {
                return fog;
            }
            set
            {
                fog = value;
            }
        }
    }
}
