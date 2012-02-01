using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameTools.Graph;

namespace Warlord.Logic.Data
{
    
    class Entity
    {
        uint id;       
        Vector3f position;

        public Entity( uint id, Vector3f position )
        {
            this.id = id;
            this.position = position;
        }
        public uint ID
        {
            get { return id; }
        }
        public Vector3f Position
        {
            get { return position; }
        }
    }
}
