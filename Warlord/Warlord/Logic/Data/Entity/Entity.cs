using System.Diagnostics;
using Microsoft.Xna.Framework;
using Warlord.GameTools;

namespace Warlord.Logic.Data.Entity
{
    class GameEntity
    {
        private Optional<uint> id;
        protected Vector3 currentPosition;

        public GameEntity(Vector3 position)
        {
            this.currentPosition = position;
            id = new Optional<uint>();
        }
        public void InitalizeID(uint id)
        {
            Debug.Assert(!this.id.Valid);

            this.id = new Optional<uint>(id);
        }
        public void Teleport(Vector3 newPosition)
        {
            currentPosition = newPosition;
        }
        public override int GetHashCode()
        {
            return (int)id.Data;
        }
        public uint actorID
        {
            get
            {
                Debug.Assert(id.Valid);
                return id.Data;
            }
        }
        public Vector3 CurrentPosition
        {
            get { return currentPosition; }
        }
    }
}
