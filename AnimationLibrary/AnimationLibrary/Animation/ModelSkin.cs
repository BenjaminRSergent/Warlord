using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameTools;
using Animation;
using System.Diagnostics;

namespace SkinnedModels.Animation
{
    public class ModelSkin
    {
        private Model model;
        private ModelExtra skeletonModelExtra;
        private Texture2D texture;
        private BaseSkinnedShader skinnedShader;

        public ModelSkin(Model model, Model skeletonModel)
        {
            this.model = model;
            this.skeletonModelExtra = skeletonModel.Tag as ModelExtra;


            if(model.Meshes[0].Effects[0] is SkinnedEffect)
            {
                SkinnedEffect effect = model.Meshes[0].Effects[0] as SkinnedEffect;
                effect.EnableDefaultLighting();
                SetShader(new SkinnedEffectWrapper(effect));
            }
        }
        public ModelSkin(Model model, Model skeletonModel, BaseSkinnedShader shader)
        {
            this.model = model;
            this.skeletonModelExtra = skeletonModel.Tag as ModelExtra;

            skinnedShader = shader;
        }
        public void SetTexture(Texture2D texture)
        {
            this.texture = texture;
        }
        public void SetShader(BaseSkinnedShader skinnedShader)
        {
            this.skinnedShader = skinnedShader;

            foreach(ModelMesh mesh in model.Meshes)
            {
                foreach(ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = skinnedShader.effect;
                }
            }
        }
        protected virtual void AdditionalShaderSetup(Matrix world, Camera3D camera, Matrix[] finalBoneTransforms)
        {
            //If the shader is not the basic shader, it can do additional work here
        }
        public void Draw(GraphicsDevice graphics, Matrix world, Camera3D camera, List<Bone> transformedBones)
        {
            Matrix[] boneTransforms = new Matrix[transformedBones.Count];


            for(int i = 0; i < transformedBones.Count; i++)
            {
                Bone bone = transformedBones[i];
                bone.ComputeAbsoluteTransform();

                boneTransforms[i] = bone.AbsoluteTransform;
            }

            Matrix[] skeleton = new Matrix[transformedBones.Count];
            for(int s = 0; s < transformedBones.Count; s++)
            {
                Bone bone = transformedBones[skeletonModelExtra.Skeleton[s]];
                skeleton[s] = bone.SkinTransform * bone.AbsoluteTransform;
            }
            foreach(ModelMesh mesh in model.Meshes)
            {
                foreach(Effect effect in mesh.Effects)
                {
                    if(texture != null)
                        skinnedShader.Texture = texture;

                    skinnedShader.World = boneTransforms[mesh.ParentBone.Index] * world;
                    skinnedShader.View = camera.View;
                    skinnedShader.Projection = camera.Projection;
                    skinnedShader.SetBoneTransforms(skeleton);

                    AdditionalShaderSetup(world, camera, skeleton);
                }

                mesh.Draw();
            }
        }

        protected Model Model { get { return model; } }
        protected ModelExtra SkeletonModelExtra { get { return skeletonModelExtra; } }
        protected Texture2D Texture { get { return texture; } }
    }
}
