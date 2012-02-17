using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Warlord.Logic.Physics
{
    enum CollisionType
    {
        None,
        Block, 
        Entity
    }
    struct CollisionData
    {
        public Vector3 collisionLocation;
        public Vector3 collisionNormal;
        public CollisionType collisionType;
        public object hitObject;        
    }
}
