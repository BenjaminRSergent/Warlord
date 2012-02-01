using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warlord
{
    class GlobalApplication
    {
        static private GameApplication application;       

        public GlobalApplication( WarlordApplication currentApplication )
        {
            application = currentApplication;
        }

        internal static GameApplication Application
        {
            get { return GlobalApplication.application; }
        }
    }
}
