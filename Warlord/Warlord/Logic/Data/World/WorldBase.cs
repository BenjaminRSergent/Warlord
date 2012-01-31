using System;
using GameTools.Graph;
using Warlord.Logic.Data;
namespace Warlord.Logic.Data.World
{
    interface WorldBase
    {
        void AddBlock(Vector3i absolutePosition, BlockType type);
        void RemoveBlock(Vector3i absolutePosition);

        void Update( Vector3f playerPosition );

        Block GetBlock(Vector3i absolutePosition);
        Block HighestBlockAt(Vector2i location);  

        Vector3i RegionSize{ get; }
    }
}
