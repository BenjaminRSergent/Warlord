using System;
namespace Warlord.Logic.Data.Entity
{
    interface EntityManager
    {
        Warlord.GameTools.Optional<Entity> GetEntity(uint id);
        Entity Player{ get; }
    }
}
