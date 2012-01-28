using System;

namespace Warlord
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (WarlordApplication game = new WarlordApplication())
            {
                game.Run();
            }
        }
    }
#endif
}

