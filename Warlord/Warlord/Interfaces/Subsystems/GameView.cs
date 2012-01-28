using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warlord.GameTools;

namespace Warlord.Interfaces.Subsystems
{
    class GameView
    {
        Optional<uint> actorID;

        public GameView( )
        {
            actorID = new Optional<uint>( );
        }
        public GameView( uint id)
        {
            actorID = new Optional<uint>( id );
        }
        public Optional<uint> ActorID
        {
            get { return actorID; }
        }
    }
}
