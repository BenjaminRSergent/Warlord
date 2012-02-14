using System;
using Animation;
using Microsoft.Xna.Framework;
using GameTools;
using Microsoft.Xna.Framework.Graphics;
using Warlord.View.Human.Display.Entity;
using XNAGraphicsHelper;

namespace Warlord.View.Human.Screens
{
    class DebugBearGraphics
        : ScreenElement
    {
        private uint entityID;
        private AnimatedComposite model;
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
        }
        public override void Draw(GameTime gameTime)
        {
            wireFrame.DrawBoundingBox(model.ModelBoundingBox, camera.View, camera.Projection, Matrix.CreateTranslation(0, 15, 0));

            model.Update(gameTime);
            model.World = Matrix.CreateTranslation(0, 15, 0);
            model.Draw(graphics, camera);
        }
    }
}
