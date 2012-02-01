using System;
using Warlord.Interfaces.Subsystems;

namespace Warlord
{
    interface GameApplication
    {
        bool Active { get; }
        Warlord.Event.EventManager GameEventManager { get; }
        Warlord.Logic.ProcessManager ProcessManager { get; }
        Warlord.Logic.Data.EntityManager EntityManager { get; }
        void ReportError( String errorReport );
    }
}
