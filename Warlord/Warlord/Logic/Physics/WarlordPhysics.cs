using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Warlord.Application;
using Warlord.Event;
using Warlord.Event.EventTypes;
using Warlord.Logic.Data.Entity;

namespace Warlord.Logic.Physics
{
    class WarlordPhysics
    {
        List<ForceGenerator> globalForces;
        CollisionDetection collisionDetection;

        public WarlordPhysics()
        {
            globalForces = new List<ForceGenerator>();
            collisionDetection = new CollisionDetection();
        }
        public void InitializeBasicForces()
        {
            ConstantDirectionalForce gravity = new ConstantDirectionalForce(9.81f, Vector3.Down);
            SimpleDrag airResistance = new SimpleDrag(16f);
        }
        public void AttachGlobalForce(ForceGenerator force)
        {
            globalForces.Add(force);
        }
        public void Update(GameTime gameTime)
        {
        }
    }
}
