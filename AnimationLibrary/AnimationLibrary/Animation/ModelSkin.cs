using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameTools;
using Animation;
using System.Diagnostics;

namespace Animation
{
    public class ModelSkin
    {
        private Model model;
        private ModelExtra skeletonModelExtra;
        private Texture2D texture;
        private BaseSkinnedShader skinnedShader;
        List<Vector3> vertices;
        private BoundingBox modelBoundingBox;

        public ModelSkin(Model model, Model skeletonModel)
        {
            this.model = model;
            this.skeletonModelExtra = skeletonModel.Tag as ModelExtra;

            vertices = new List<Vector3>();

            if(model.Meshes[0].Effects[0] is SkinnedEffect)
            {
                SkinnedEffect effect = model.Meshes[0].Effects[0] as SkinnedEffect;
                effect.EnableDefaultLighting();
                SetShader(new SkinnedEffectWrapper(effect));
            }

            InitalCalcBoundingBox();
        }
        public ModelSkin(Model model, Model skeletonModel, BaseSkinnedShader shader)
        {
            this.model = model;
            this.skeletonModelExtra = skeletonModel.Tag as ModelExtra;

            skinnedShader = shader;

            InitalCalcBoundingBox();
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

                    skinnedShader.World = world;
                    skinnedShader.View = camera.View;
                    skinnedShader.Projection = camera.Projection;
                    skinnedShader.SetBoneTransforms(skeleton);

                    AdditionalShaderSetup(world, camera, skeleton);
                }

                mesh.Draw();
            }
        }

        private void InitalCalcBoundingBox()
        {
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            Matrix[] bones = new Matrix[this.model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(bones);

            foreach(ModelMesh mesh in this.model.Meshes)
            {
                //We need to add two to skip the model and top level skeleton "bones"
                Matrix transform = bones[mesh.ParentBone.Index];

                foreach(ModelMeshPart part in mesh.MeshParts)
                {
                    int stride = part.VertexBuffer.VertexDeclaration.VertexStride;
                    int numVertices = part.NumVertices;
                    byte[] verticesData = new byte[stride * numVertices];

                    part.VertexBuffer.GetData(verticesData);

                    for(int i = 0; i < verticesData.Length; i += stride)
                    {
                        float x = BitConverter.ToSingle(verticesData, i);
                        float y = BitConverter.ToSingle(verticesData, i + sizeof(float));
                        float z = BitConverter.ToSingle(verticesData, i + 2 * sizeof(float));

                        Vector3 vector = new Vector3(x, y, z);

                        vector = Vector3.Transform(vector, transform);

                        vertices.Add(vector);
                    }
                }
            }

            modelBoundingBox = BoundingBox.CreateFromPoints(vertices);
        }

        private Model Model { get { return model; } }
        public BoundingBox ModelBoundingBox { get { return modelBoundingBox; } }
        protected ModelExtra SkeletonModelExtra { get { return skeletonModelExtra; } }
        protected Texture2D Texture { get { return texture; } }
    }
}
