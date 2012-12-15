using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameTools;
using Warlord.Logic.Data.World;

namespace Warlord.Interfaces
{
    interface BlockAccess
    {
        Optional<Block> GetBlockAt( Vector3 location );

        List<Block> GetBlockWithin(BoundingBox currentBox);
    }
}
