using System;
using Warlord.Application;

namespace Warlord
{
    static class Program
    {
        static void Main(string[] args)
        {
            using(WarlordApplication game = new WarlordApplication())
            {
                game.Run();
            }
        }
    }
}

