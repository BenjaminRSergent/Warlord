using System;

namespace Warlord.View.Human.Screens
{
    class EntityGraphics
        : ScreenElement
    {
        uint actorID;
        EntityGraphics(uint actorID)
        {
            this.actorID = actorID;
        }
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
