
namespace Warlord.Logic.Data.World
{
    class ZoneBlockSettings
    {
        public delegate float ModifyDensity(float density, float modifier);

        ModifyDensity modifyMethod;
        public ZoneBlockSettings( float dirtLevel, float stoneLevel, ModifyDensity modifyMethod )
        {
            this.dirtLevel = dirtLevel;
            this.stoneLevel = stoneLevel;
            this.modifyMethod = modifyMethod;
        }

        public BlockType GetBlockFromDensity( float density, float modifier )
        {
            float effectiveDensity = modifyMethod.Invoke(density,modifier);

            if( effectiveDensity < dirtLevel )
                return BlockType.Air;
            
            if( effectiveDensity < stoneLevel )
                return BlockType.Dirt;
            
            return BlockType.Stone;            
        }

        float dirtLevel;
        float stoneLevel;
    }
}
