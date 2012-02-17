using Microsoft.Xna.Framework;

namespace Warlord.Logic.Data.Entity
{
    class Player : GameEntity
    {
        public Player(Vector3 position, EntityType type, float mass, float scale)
            : base(position, type, mass, scale)
        {

        }
    }
}
