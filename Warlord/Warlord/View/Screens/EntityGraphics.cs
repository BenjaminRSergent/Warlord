using System;
using SkinnedModels.Animation;
using Microsoft.Xna.Framework;
using GameTools;
using Microsoft.Xna.Framework.Graphics;

namespace Warlord.View.Human.Screens
{
    class EntityGraphics
        : ScreenElement
    {
        private uint entityID;
        private AnimatedComposite model;
        private GraphicsDevice graphics;
        private Camera3D camera;

        EntityGraphics(uint entityID, GraphicsDevice graphics, Camera3D camera)
        {
            this.entityID = entityID;
            this.graphics = graphics;
            this.camera = camera;
        }
        public override void Draw(GameTime gameTime)
        {
            model.Draw(graphics, camera);
        }
    }
}
