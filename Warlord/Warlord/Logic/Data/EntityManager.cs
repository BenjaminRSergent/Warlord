using System;
namespace Warlord.Logic.Data
{
    interface EntityManager
    {
        Warlord.GameTools.Optional<Entity> GetEntity(uint id);
        Entity Player{ get; }
    }
}
