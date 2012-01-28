using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameTools.Graph;

namespace Warlord.Logic
{
    interface FiniteWorldGenerator
    {
        FiniteWorld GetDefaultWorld( );
        FiniteWorld GetDebugWorld( );
        FiniteWorld GetSimpleWorld( Vector2i worldSize, Vector3i RegionSize );
    }
}
