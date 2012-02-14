using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Animation
{
    public abstract class BaseSkinnedShader
    {
        public BaseSkinnedShader( Effect baseEffect )
        {
            effect = baseEffect;
        }
        public Effect effect { get; private set; }
        abstract public Texture2D Texture { get; set; }
        abstract public Matrix World { get; set; }
        abstract public Matrix View { get; set; }
        abstract public Matrix Projection { get; set; }       

        abstract public void SetBoneTransforms(Microsoft.Xna.Framework.Matrix[] skeleton);
    }
}
