using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warlord.Logic.Data.World
{
    enum BlockShape
    {        
        FullBlock = 0,
        RampXIncreasing = 1,
        RampXDecreasing = 2,
        RampZIncreasing = 4,
        RampZDecreasing = 8,
        MAXIMUM = 16
    }
}
