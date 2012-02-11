using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Animation;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace SkinnedModels.Animation
{
    public class PlayingClip
    {
        private AnimationClip masterClip;
        private List<BoneInfo> boneInfo;
        private Quaternion[] boneRotations;
        private Vector3[] boneTranslations;

        private float position;
        private float speed;
        private float originalWeight;
        private float[] relativeWeight;
        private bool loop;
        private bool done;

        public PlayingClip(AnimationClip masterClip, float speed, float weight, bool loop)
        {
            this.masterClip = masterClip;
            this.speed = speed;
            this.originalWeight = weight;
            this.loop = loop;
            position = 0;

            relativeWeight = new float[masterClip.AnimatedBoneCollection.Count];
            boneRotations = new Quaternion[masterClip.AnimatedBoneCollection.Count];
            boneTranslations = new Vector3[masterClip.AnimatedBoneCollection.Count];

            boneInfo = new List<BoneInfo>( );

            for(int index = 0; index < masterClip.AnimatedBoneCollection.Count; index++)
            {
                boneInfo.Add(new BoneInfo(masterClip.AnimatedBoneCollection[index]));
            }
        }
        public void IncrementPosition(GameTime gameTime)
        {
            position += (gameTime.ElapsedGameTime.Milliseconds / 1000.0f) * speed;

            for(int index = 0; index < boneInfo.Count; index++)
            {
                boneInfo[index].SetPosition(position);
                boneRotations[index] = boneInfo[index].Rotation;
                boneTranslations[index] = boneInfo[index].Translation;
            }

            if(position > masterClip.Duration)
            {
                if(loop)
                    position = 0;
                else
                    done = true;
            }
        }
        public bool IsBoneStatic( int index )
        {
            if( boneInfo[index].Keyframe1 == boneInfo[index].Keyframe2)
                return true;
            else 
                return false;
        }
        public bool Done { get { return done; } }

        public float Position { get { return position; } }
        public float Speed { get { return speed; } }
        public float OriginalWeight { get { return originalWeight; } }
        public float[] RelativeWeight { get { return relativeWeight; } }
        public Quaternion[] BoneRotations { get { return boneRotations; } set { boneRotations = value; } }
        public Vector3[] BoneTranslations { get { return boneTranslations; } set { boneTranslations = value; } }

        private class BoneInfo
        {
            private int currentKeyframe;
            public bool valid = false;

            private Quaternion rotation;
            private Vector3 translation;

            public AnimationClip.Keyframe Keyframe1;
            public AnimationClip.Keyframe Keyframe2;

            public AnimationClip.AnimatedBone ClipBone { get; set; }

            public BoneInfo(AnimationClip.AnimatedBone bone)
            {
                ClipBone = bone;

                currentKeyframe = 0;

                SetKeyframes();
            }
            public void SetPosition(float position)
            {
                Quaternion newRotation = Quaternion.Identity;

                List<AnimationClip.Keyframe> keyframes = ClipBone.Keyframes;
                Debug.Assert(keyframes.Count != 0);

                while(position < Keyframe1.Time && currentKeyframe > 0)
                {
                    currentKeyframe--;
                    SetKeyframes();
                }

                while(position >= Keyframe2.Time && currentKeyframe < ClipBone.Keyframes.Count - 2)
                {
                    currentKeyframe++;
                    SetKeyframes();
                }

                if(Keyframe1 == Keyframe2)
                {
                    rotation = Keyframe1.Rotation;
                    translation = Keyframe1.Translation;
                }
                else
                {
                    float t = (float)((position - Keyframe1.Time) / (Keyframe2.Time - Keyframe1.Time));

                    rotation = Quaternion.Slerp(Keyframe1.Rotation, Keyframe2.Rotation, t);
                    translation = Vector3.Lerp(Keyframe1.Translation, Keyframe2.Translation, t);
                }

                valid = true;
            }

            private void SetKeyframes()
            {
                if(ClipBone.Keyframes.Count > 0)
                {
                    Keyframe1 = ClipBone.Keyframes[currentKeyframe];
                    if(currentKeyframe == ClipBone.Keyframes.Count - 1)
                        Keyframe2 = Keyframe1;
                    else
                        Keyframe2 = ClipBone.Keyframes[currentKeyframe + 1];
                }
                else
                {
                    Keyframe1 = null;
                    Keyframe2 = null;
                }
            }

            public Quaternion Rotation
            {
                get { return rotation; }
            }
            public Vector3 Translation
            {
                get { return translation; }
            }
        }
    }
}

