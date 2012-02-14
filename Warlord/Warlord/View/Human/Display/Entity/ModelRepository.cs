using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Animation;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Warlord.View.Human.Display.Entity
{
    static class ModelRepository
    {
        private static AnimatedComposite bear;

        public static void Initialize( ContentManager content )
        {
            Model bearModel = content.Load<Model>("Models/bear_model");
            Model bearSkeleton = content.Load<Model>("Models/bear_skeleton");
            Model bearWalk = content.Load<Model>("Models/bear_walk");

            bear = new AnimatedComposite(bearSkeleton);

            bear.AddModel("full_model", bearModel);
            bear.AddAnimation(bearWalk);

            bear.SetModelTexture("full_model", TextureRepository.BearTexture);                
        }

        public static AnimatedComposite GetBear( )
        {
            return bear;
        }
    }
}
