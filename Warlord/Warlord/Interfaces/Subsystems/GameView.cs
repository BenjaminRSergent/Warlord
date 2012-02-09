using Warlord.GameTools;

namespace Warlord.Interfaces.Subsystems
{
    class GameView
    {
        Optional<uint> actorID;

        public GameView()
        {
            actorID = new Optional<uint>();
        }
        public GameView(uint id)
        {
            actorID = new Optional<uint>(id);
        }
        public Optional<uint> ActoractorID
        {
            get { return actorID; }
        }
    }
}
