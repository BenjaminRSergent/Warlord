using System;
using Warlord.Interfaces.Subsystems;
using Warlord.Event;
using Warlord.Logic.Data.Entity;

namespace Warlord
{
    interface GameApplication
    {
        bool Active { get; }
        EventManager GameEventManager { get; }
        EntityManager EntityManager { get; }
        void ReportError( String errorReport );
    }
}
