using Warlord.GameTools;

namespace Warlord.Interfaces.Subsystems
{
    class GameView
    {
        Optional<uint> entityID;

        public GameView()
        {
            entityID = new Optional<uint>();
        }
        public GameView(uint id)
        {
            entityID = new Optional<uint>(id);
        }
        public Optional<uint> EntityID
        {
            get { return entityID; }
        }
    }
}
