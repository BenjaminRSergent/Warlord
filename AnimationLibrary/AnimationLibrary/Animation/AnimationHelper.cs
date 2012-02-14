using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Animation;
using Microsoft.Xna.Framework;

namespace Animation
{
    public static class AnimationHelper
    {
        public static List<Bone> ObtainBones(Model skeletonModel)
        {
            List<Bone> skeleton = new List<Bone>();
            skeleton.Clear();
            foreach(ModelBone bone in skeletonModel.Bones)
            {
                // Create the bone object and add to the heirarchy
                Bone newBone = new Bone(bone.Name, bone.Transform, bone.Parent != null ? skeleton[bone.Parent.Index] : null);

                // Add to the bones for this model
                skeleton.Add(newBone);
            }

            return skeleton;
        }
    }
}
