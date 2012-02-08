using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameTools.Graph;
using Microsoft.Xna.Framework;
using Warlord.GameTools;
using System.Diagnostics;

namespace Warlord.Logic.Data.Entity
{
    class GameEntity
    {
        private Optional<uint> id;

        public GameEntity()
        {
            id = new Optional<uint>();
        }
        public void InitalizeID(uint id)
        {
            Debug.Assert(!this.id.Valid);

            this.id = new Optional<uint>(id);
        }
        public uint actorID
        {
            get
            {
                Debug.Assert(id.Valid);
                return id.Data;
            }
        }
    }
}
