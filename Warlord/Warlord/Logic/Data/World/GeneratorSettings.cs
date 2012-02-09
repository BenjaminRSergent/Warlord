using GameTools.Graph;

namespace Warlord.Logic.Data.World
{
    class GeneratorSettings
    {
        public Vector3i RegionSize;

        public int midLevel;
        public int highLevel;

        public ZoneBlockSettings lowLevelZone;
        public ZoneBlockSettings midLevelZone;
        public ZoneBlockSettings highLevelZone;
    }
}
