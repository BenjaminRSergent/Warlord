using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Warlord.Logic.Data.Entity
{
    interface EntityManager
    {
        GameEntity GetEntity(uint id);
        List<GameEntity> GetEntitiesWithin( Vector3 position, int radius );
        Player Player { get; }
    }
}
