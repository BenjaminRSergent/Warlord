using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Warlord.View.Human.Display.EntityView;

namespace Warlord.Logic.Data.Entity
{
    static class EntityBoundingBoxes
    {
        private static Dictionary<EntityType, BoundingBox> boxMap;

        public static void PullFromModels( )
        {
            boxMap = new Dictionary<EntityType,BoundingBox>( );

            //This is rather wasteful, but is only needs to be done once on start up.
            boxMap.Add(EntityType.bear, ModelRepository.GetEntity(EntityType.bear).ModelBoundingBox);
        }

        public static BoundingBox GetBox( EntityType type )
        {
            Debug.Assert(boxMap != null && boxMap.ContainsKey(type));
            return boxMap[type];
        }
    }
}
