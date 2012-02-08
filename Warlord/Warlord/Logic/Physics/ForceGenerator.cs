using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warlord.Logic.Data.Entity;
using Microsoft.Xna.Framework;

namespace Warlord.Logic.Physics
{
    interface ForceGenerator
    {
        void ApplyForceTo(GameTime gameTime, MovingEntity entity);
    }
}
