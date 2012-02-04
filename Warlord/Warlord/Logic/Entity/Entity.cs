using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameTools.Graph;

namespace Warlord.Logic.Data.Entity
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
        public void Teleport( Vector3f location )
        {
            this.position = location;
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
