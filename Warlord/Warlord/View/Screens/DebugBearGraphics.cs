using System;
using Animation;
using Microsoft.Xna.Framework;
using GameTools;
using Microsoft.Xna.Framework.Graphics;
using Warlord.View.Human.Display.Entity;
using XNAGraphicsHelper;
using Warlord.Application;
using Warlord.Logic.Data.Entity;

namespace Warlord.View.Human.Screens
{
    class DebugBearGraphics
        : ScreenElement
    {
        private uint entityID;
        private AnimatedComposite model;
        private GameEntity entity;
        private GraphicsDevice graphics;
        private Camera3D camera;
        private WireFrameDrawer wireFrame;

        public DebugBearGraphics(uint entityID, GraphicsDevice graphics, Camera3D camera)
        {
            this.entityID = entityID;
            this.graphics = graphics;
            this.camera = camera;
            wireFrame = new WireFrameDrawer(graphics);

            // For debug purposes, all models are bears
            model = ModelRepository.GetBear();
            model.PlayAnimation("Walking", 1, 1, true);

            entity = GlobalSystems.EntityManager.GetEntity(entityID);
        }
        public override void Draw(GameTime gameTime)
        {            
            Vector3 direction = entity.Facing;
            const float step = 0.5f;
            float length = 10*(entity.GetFuturePosition(gameTime) - entity.CurrentPosition).Length( );
            BoundingBox currentBox = entity.CurrentBoundingBox;
            Matrix transformStep = Matrix.CreateTranslation(step*direction);
            
            for(float onRay = 0; onRay < length; onRay += step)
            {
                currentBox.Min = Vector3.Transform(currentBox.Min, transformStep);
                currentBox.Max = Vector3.Transform(currentBox.Max, transformStep);
                wireFrame.DrawBoundingBox(currentBox, camera.View, camera.Projection);
            }
            

            model.Update(gameTime);
            model.World = Matrix.CreateScale(entity.Scale);
            model.World *= Matrix.CreateTranslation(entity.CurrentPosition);
            model.Draw(graphics, camera);
        }
    }
}
