using System.Collections.Generic;
using System.Diagnostics;
using Animation;
using GameTools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Animation
{
    public class AnimatedComposite
    {
        private Model skeletonModel;
        private ModelExtra skeletonModelExtra;
        private Dictionary<string, ModelSkin> parts;
        private AnimationPlayer animationPlayer;
        private Matrix world;
        private BoundingBox modelBoundingBox;

        public AnimatedComposite(Model skeletonModel)
        {
            this.skeletonModel = skeletonModel;
            skeletonModelExtra = skeletonModel.Tag as ModelExtra;
            parts = new Dictionary<string, ModelSkin>();
            animationPlayer = new AnimationPlayer(skeletonModel, skeletonModelExtra.Clips[0]);
            world = Matrix.Identity;
        }
        public AnimatedComposite(AnimatedComposite source)
        {
            this.skeletonModel = source.skeletonModel;
            this.skeletonModelExtra = source.skeletonModelExtra;
            this.parts = source.parts;            
            this.animationPlayer = new AnimationPlayer(source.animationPlayer);
            this.world = source.world;
            this.modelBoundingBox = source.modelBoundingBox;
        }
        public void AddModel(string name, Model model)
        {
            parts.Add(name, new ModelSkin(model, skeletonModel));

            CalcBoundingBox();
        }
        public void RemoveModel(string name)
        {
            parts.Remove(name);

            CalcBoundingBox();
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
        private void CalcBoundingBox()
        {
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            Vector3 minVector;
            Vector3 maxVector;

            BoundingBox partBox;
            foreach(ModelSkin model in parts.Values)
            {
                partBox = model.ModelBoundingBox;
                minVector = partBox.Min;
                maxVector = partBox.Max;
                if(minVector.X < min.X) min.X = minVector.X;
                if(minVector.Y < min.Y) min.Y = minVector.Y;
                if(minVector.Z < min.Z) min.Z = minVector.Z;

                if(maxVector.X > max.X) max.X = maxVector.X;
                if(maxVector.Y > max.Y) max.Y = maxVector.Y;
                if(maxVector.Z > max.Z) max.Z = maxVector.Z;
            }

            modelBoundingBox = new BoundingBox(min, max);
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

        public BoundingBox ModelBoundingBox { get { return modelBoundingBox; } }
    }
}
