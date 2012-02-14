using Microsoft.Xna.Framework;
using Warlord.Logic.Data.Entity;

namespace Warlord.Logic.Physics
{
    interface ForceGenerator
    {
        void ApplyForceTo(GameTime gameTime, GameEntity entity);
    }
}
