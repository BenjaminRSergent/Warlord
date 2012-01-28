using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warlord.Logic.Data
{
    enum BlockFaceField
    {
        XIncreasing = 1,
        YIncreasing = 2,
        ZIncreasing = 4,

        XDecreasing = 8,
        YDecreasing = 16,
        ZDecreasing = 32,

        MAXIMUM = 64
    }
}
