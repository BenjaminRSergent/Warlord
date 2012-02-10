using GameTools.Graph;
using Microsoft.Xna.Framework;

namespace Warlord.Logic.Data.World
{
    class GeneratorSettings
    {
        public Vector3 RegionSize;

        public int midLevel;
        public int highLevel;

        public ZoneBlockSettings lowLevelZone;
        public ZoneBlockSettings midLevelZone;
        public ZoneBlockSettings highLevelZone;
    }
}
