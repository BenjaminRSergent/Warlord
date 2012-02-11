using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace Animation
{
    public class AnimationClipReader : ContentTypeReader<AnimationClip>
    {
        protected override AnimationClip Read(ContentReader input, AnimationClip existingInstance)
        {
            AnimationClip clip = new AnimationClip();
            clip.Name = input.ReadString();
            clip.Duration = input.ReadDouble();

            int numBones = input.ReadInt32();
            for (int i = 0; i < numBones; i++)
            {
                AnimationClip.AnimatedBone bone = new AnimationClip.AnimatedBone();
                clip.AnimatedBoneCollection.Add(bone);

                bone.Name = input.ReadString();

                int numKeyFrames = input.ReadInt32();

                for (int k = 0; k < numKeyFrames; k++)
                {
                    AnimationClip.Keyframe keyframe = new AnimationClip.Keyframe();
                    keyframe.Time = input.ReadDouble();
                    keyframe.Rotation = input.ReadQuaternion();
                    keyframe.Translation = input.ReadVector3();

                    bone.Keyframes.Add(keyframe);
                }

            }

            return clip;
        }
    }
}
