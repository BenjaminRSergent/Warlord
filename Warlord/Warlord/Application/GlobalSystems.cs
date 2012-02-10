using GameTools.Process;
using Warlord.Event;
using Warlord.Logic.Data.Entity;

namespace Warlord.Application
{
    static class GlobalSystems
    {
        private static WarlordApplication application;

        public static void SetCurrentApplication(WarlordApplication currentApplication)
        {
            application = currentApplication;
        }
        public static WarlordEntityManager EntityManager
        {
            get { return application.EntityManager; }
        }
        public static WarlordEventManager EventManager
        {
            get { return application.EventManager; }
        }
        public static ThreadManager ThreadManager
        {
            get { return application.ThreadManager; }
        }
        public static bool GameWindowHasFocus
        {
            get { return application.IsActive; }
        }
    }
}
