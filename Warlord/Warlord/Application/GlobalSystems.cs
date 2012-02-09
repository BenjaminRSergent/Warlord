using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warlord.Logic.Data.Entity;
using Warlord.Event;
using GameTools.Process;

namespace Warlord.Application
{
    static class GlobalSystems
    {
        static private WarlordApplication application;

        static public void SetCurrentApplication(WarlordApplication currentApplication)
        {
            application = currentApplication;
        }
        static public WarlordEntityManager EntityManager
        {
            get { return application.EntityManager; }
        }
        static public WarlordEventManager EventManager
        {
            get { return application.EventManager; }
        }
        static public ThreadManager ThreadManager
        {
            get{ return application.ThreadManager; }
        }
        static public bool GameWindowHasFocus
        {
            get { return application.IsActive; }
        }
    }
}
