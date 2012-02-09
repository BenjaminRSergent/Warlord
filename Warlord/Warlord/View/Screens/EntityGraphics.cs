using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
