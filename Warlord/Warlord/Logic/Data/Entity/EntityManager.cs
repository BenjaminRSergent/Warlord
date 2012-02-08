using System;
using System.Collections.Generic;

namespace Warlord.Logic.Data.Entity
{
    interface EntityManager
    {
        GameEntity GetEntity(uint id);
        void AddEntity(GameEntity newEntity);

        Player Player { get; }
    }
}
