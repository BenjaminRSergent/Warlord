using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Animation
{
    public class SkinnedEffectWrapper : BaseSkinnedShader
    {
        new SkinnedEffect effect;
        public SkinnedEffectWrapper(SkinnedEffect effect) : base(effect)
        {
            this.effect = effect;
        }

        public override Texture2D Texture
        {
            get
            {
                return effect.Texture;
            }
            set
            {
                effect.Texture = value;
            }
        }

        public override Matrix World
        {
            get
            {
                return effect.World;
            }
            set
            {
                effect.World = value;
            }
        }

        public override Matrix View
        {
            get
            {
                return effect.View;
            }
            set
            {
                effect.View = value;
            }
        }

        public override Matrix Projection
        {
            get
            {
                return effect.Projection;
            }
            set
            {
                effect.Projection = value;
            }
        }

        public override void SetBoneTransforms(Matrix[] skeleton)
        {
            effect.SetBoneTransforms(skeleton);
        }
    }
}
