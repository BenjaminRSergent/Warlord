using System.Collections.Generic;
using System.Diagnostics;
using Animation;
using GameTools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SkinnedModels.Animation
{
    public class AnimatedComposite
    {
        private Model skeletonModel;
        private ModelExtra skeletonModelExtra;
        private Dictionary<string, ModelSkin> parts;
        private AnimationPlayer animationPlayer;
        private Matrix world;        

        public AnimatedComposite(Model skeletonModel)
        {
            this.skeletonModel = skeletonModel;
            skeletonModelExtra = skeletonModel.Tag as ModelExtra;
            parts = new Dictionary<string, ModelSkin>();
            animationPlayer = new AnimationPlayer(skeletonModel, skeletonModelExtra.Clips[0]);
        }
        public void AddModel(string name, ModelSkin model)
        {
            parts.Add(name, model);
        }
        public void RemoveModel(string name)
        {
            parts.Remove(name);
        }
        public void SetModelTexture(string name, Texture2D texture)
        {
            parts[name].SetTexture(texture);
        }
        public void SetEffect(BaseSkinnedShader effect)
        {
            foreach(ModelSkin model in parts.Values)
            {
                model.SetShader(effect);
            }
        }
        public void ChangePart(string name, ModelSkin model)
        {
            Debug.Assert(parts.ContainsKey(name));
            parts[name] = model;
        }
        public void AddAnimation(Model animationSkin)
        {
            AnimationClip animation = (animationSkin.Tag as ModelExtra).clips[0];
            animationPlayer.AddAnimation(animation);
        }
        public void AddAnimation(AnimationClip animation)
        {
            animationPlayer.AddAnimation(animation);
        }
        public void PlayAnimation(string name, float speed, float weight, bool loop)
        {
            animationPlayer.Play(name, speed, weight, loop);
        }
        public void Update(GameTime gameTime)
        {
            animationPlayer.Update(gameTime);
        }
        public void Draw(GraphicsDevice graphics, Camera3D camera)
        {
            List<Bone> boneTransforms;
            boneTransforms = animationPlayer.GetTransformedBones();
            foreach(ModelSkin model in parts.Values)
            {
                model.Draw(graphics, world, camera, boneTransforms);
            }
        }

        public Matrix World
        {
            get { return world; }
            set { world = value; }
        }

        public void StopAnimation(string name)
        {
            animationPlayer.Stop(name);
        }
    }
}
