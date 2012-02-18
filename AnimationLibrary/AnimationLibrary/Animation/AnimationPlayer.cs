using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Animation;
using Microsoft.Xna.Framework.Graphics;

namespace Animation
{
    public class AnimationPlayer
    {
        private List<Bone> skeleton;
        private List<AnimationClip> storedClips;
        private List<PlayingClip> playingClips;

        private PlayingClip neutral;

        private float[] totalWeight;

        public AnimationPlayer(Model skeletonModel, AnimationClip neutral)
        {
            storedClips = new List<AnimationClip>();
            playingClips = new List<PlayingClip>();
            skeleton = AnimationHelper.ObtainBones(skeletonModel);
            totalWeight = new float[skeleton.Count];

            this.neutral = new PlayingClip(neutral, 1, 0, true);
        }
        public AnimationPlayer(AnimationPlayer source)
        {
            this.skeleton = source.skeleton;
            this.neutral = new PlayingClip(source.neutral);
            this.storedClips = new List<AnimationClip>(source.storedClips);
            this.playingClips = new List<PlayingClip>(source.playingClips);
            this.totalWeight = source.totalWeight;
        }
        public List<Bone> GetTransformedBones()
        {
            Matrix totalTransform = Matrix.Identity;
            Vector3 totalTranslation = Vector3.Zero;

            Quaternion[] rotation = new Quaternion[skeleton.Count];
            Vector3[] translation = new Vector3[skeleton.Count];

            Quaternion newRotation;
            Vector3 newTranslation;

            for(int index = 0; index < skeleton.Count; index++)
            {
                rotation[index] = neutral.BoneRotations[index];
                translation[index] = neutral.BoneTranslations[index];
            }

            foreach(PlayingClip clip in playingClips)
            {
                for(int index = 0; index < skeleton.Count; index++)
                {
                    newRotation = clip.BoneRotations[index];
                    newTranslation = clip.BoneTranslations[index];

                    if(clip.RelativeWeight[index] > 0)
                    {
                        rotation[index] = Quaternion.Slerp(rotation[index], newRotation, clip.RelativeWeight[index]);
                        translation[index] = Vector3.Lerp(translation[index], newTranslation, clip.RelativeWeight[index]);
                    }
                }
            }

            for(int index = 0; index < skeleton.Count; index++)
            {
                totalTransform = Matrix.CreateFromQuaternion(rotation[index]);
                totalTransform.Translation = translation[index];
                skeleton[index].SetTransform(totalTransform);
            }

            return skeleton;
        }
        public void Update(GameTime gameTime)
        {
            List<PlayingClip> toRemove = new List<PlayingClip>();

            neutral.IncrementPosition(gameTime);

            foreach(PlayingClip clip in playingClips)
            {
                clip.IncrementPosition(gameTime);
                if(clip.Done)
                    toRemove.Add(clip);
            }

            foreach(PlayingClip clip in toRemove)
            {
                playingClips.Remove(clip);
            }

            if(toRemove.Count > 0)
                UpdateWeights();
        }
        public void AddAnimation(AnimationClip animation)
        {
            storedClips.Add(animation);
        }
        public void RemoveAnimation(AnimationClip animation)
        {
            storedClips.Remove(animation);
        }
        private void UpdateWeights()
        {
            for(int index = 0; index < skeleton.Count; index++)
            {
                totalWeight[index] = 0;

                foreach(PlayingClip clip in playingClips)
                {
                    if(!clip.IsBoneStatic(index))
                        totalWeight[index] += clip.OriginalWeight;
                }
                foreach(PlayingClip clip in playingClips)
                {
                    if(clip.IsBoneStatic(index))
                        clip.RelativeWeight[index] = 0;
                    else
                        clip.RelativeWeight[index] = clip.OriginalWeight / totalWeight[index];
                }
            }
        }
        public void Play(string name, float speed, float weight, bool loop)
        {
            AnimationClip clipToPlay = null;


            foreach(PlayingClip clip in playingClips)
            {
                if(clip.Name == name)
                    return;
            }

            foreach(AnimationClip clip in storedClips)
            {
                if(clip.Name == name)
                {
                    clipToPlay = clip;
                    break;
                }
            }

            if(clipToPlay != null)
            {
                playingClips.Add(new PlayingClip(clipToPlay, speed, weight, loop));
                UpdateWeights();
            }
        }

        internal void Stop(string name)
        {
            PlayingClip theClip = null;
            foreach(PlayingClip clip in playingClips)
            {
                if(clip.Name == name)
                {
                    theClip = clip;
                    break;
                }
            }

            if(theClip != null)
                playingClips.Remove(theClip);
        }
    }
}
