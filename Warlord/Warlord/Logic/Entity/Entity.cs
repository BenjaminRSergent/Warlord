using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameTools.Graph;
using Microsoft.Xna.Framework;

namespace Warlord.Logic.Data.Entity
{    
    class Entity
    {
        uint id;       
        Vector3 position;

        public Entity( uint id, Vector3 position )
        {
            this.id = id;
            this.position = position;
        }
        public void Teleport( Vector3 location )
        {
            this.position = location;
        }
        public uint actorID
        {
            get { return id; }
        }
        public Vector3f Position
        {
            get { return position; }
        }
    }
}
