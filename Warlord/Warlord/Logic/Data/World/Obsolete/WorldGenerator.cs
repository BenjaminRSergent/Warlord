using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameTools.Graph;

namespace Warlord.Logic.Data.World
{
    interface WorldGenerator
    {
        WorldBase GetDefaultWorld( );
        WorldBase GetDebugWorld( );
        WorldBase GetSimpleWorldWith3D( Vector2i worldSize, Vector3i RegionSize );
    }
}
