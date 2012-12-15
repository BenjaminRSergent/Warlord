using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Animation;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Warlord.Logic.Data.Entity;
using Microsoft.Xna.Framework;

namespace Warlord.View.Human.Display.EntityView
{
    static class ModelRepository
    {
        private static Dictionary<EntityType, AnimatedComposite> modelMap;

        public static void Initialize( ContentManager content )
        {
            modelMap = new Dictionary<EntityType,AnimatedComposite>( );

            Model bearModel = content.Load<Model>("Models/bear_model");
            Model bearSkeleton = content.Load<Model>("Models/bear_skeleton");
            Model bearWalk = content.Load<Model>("Models/bear_walk");

            AnimatedComposite bear = new AnimatedComposite(bearSkeleton);

            bear.AddModel("full_model", bearModel);
            bear.AddAnimation(bearWalk);

            bear.SetModelTexture("full_model", TextureRepository.BearTexture);
          
            modelMap.Add(EntityType.bear, bear);

            EntityBoundingBoxes.PullFromModels( );
        }

        public static AnimatedComposite GetBear( )
        {
            return new AnimatedComposite(modelMap[EntityType.bear]);
        }

        public static AnimatedComposite GetEntity(EntityType entityType)
        {
            return new AnimatedComposite(modelMap[entityType]);
        }
    }
}
