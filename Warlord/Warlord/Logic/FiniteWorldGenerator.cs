using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warlord.Logic
{
    interface FiniteWorldGenerator
    {
        FiniteWorld GetDefaultWorld( );
        FiniteWorld GetDebugWorld( );
        FiniteWorld GetSimpleWorld( );
    }
}
