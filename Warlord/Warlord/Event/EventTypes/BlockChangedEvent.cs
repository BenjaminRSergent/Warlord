using System;
using Warlord.GameTools;
using Warlord.Logic.Data.World;

namespace Warlord.Event.EventTypes
{
    class BlockChangedData
    {
        public Block oldBlock;
        public Block newBlock;

        public BlockChangedData(Block oldBlock, Block newBlock)
        {
            this.oldBlock = oldBlock;
            this.newBlock = newBlock;
        }
    }

    class BlockChangedEvent : BaseGameEvent
    {
        BlockChangedData data;

        public BlockChangedEvent(Optional<Object> sender, int delay, BlockChangedData data)
            : base(sender, "block_changed", delay)
        {
            this.data = data;
        }

        public BlockChangedData Data
        {
            get { return data; }
        }
    }
}
