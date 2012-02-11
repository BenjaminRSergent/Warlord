using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Animation
{
    public class Bone
    {
        private Bone parent = null;
        private List<Bone> children = new List<Bone>();

        private Matrix bindTransform = Matrix.Identity;
        private Vector3 bindScale = Vector3.One;

        private Vector3 translation = Vector3.Zero;
        private Quaternion rotation = Quaternion.Identity;
        private Vector3 scale = Vector3.One;

        private Matrix origionalAbsoluteTransform = Matrix.Identity;
        private Matrix absoluteTransform = Matrix.Identity;

        public Bone(string name, Matrix bindTransform, Bone parent)
        {
            this.Name = name;
            this.parent = parent;
            if(parent != null)

                this.bindScale = new Vector3(bindTransform.Right.Length(),
                    bindTransform.Up.Length(), bindTransform.Backward.Length());

            bindTransform.Right = bindTransform.Right / bindScale.X;
            bindTransform.Up = bindTransform.Up / bindScale.Y;
            bindTransform.Backward = bindTransform.Backward / bindScale.Y;
            this.bindTransform = bindTransform;

            // Set the skinning bind transform
            // That is the inverse of the absolute transform in the bind pose

            ComputeAbsoluteTransform();
            origionalAbsoluteTransform = AbsoluteTransform;
            SkinTransform = Matrix.Invert(AbsoluteTransform);
        }
        public void SetTransform(Matrix Transformation)
        {
            Matrix setTo = Transformation * Matrix.Invert(BindTransform);

            Translation = setTo.Translation;
            Rotation = Quaternion.CreateFromRotationMatrix(setTo);
        }
        public void ComputeAbsoluteTransform()
        {
            Matrix transform = Matrix.CreateScale(Scale * bindScale) *
                Matrix.CreateFromQuaternion(Rotation) *
                Matrix.CreateTranslation(Translation) *
                BindTransform;

            if(Parent != null)
            {
                AbsoluteTransform = transform * Parent.AbsoluteTransform;
            }
            else
            { 
                AbsoluteTransform = transform;
            }
        }

        public string Name { get; set; }
        public Matrix BindTransform { get { return bindTransform; } }
        public Matrix SkinTransform { get; set; }

        public Quaternion Rotation { get { return rotation; } set { rotation = value; } }
        public Vector3 Translation { get { return translation; } set { translation = value; } }
        public Vector3 Scale { get { return scale; } set { scale = value; } }

        public Bone Parent { get { return parent; } }
        public List<Bone> Children { get { return children; } }

        public Matrix OrigionalAbsoluteTransform { get { return origionalAbsoluteTransform; } }
        public Matrix AbsoluteTransform { get { return absoluteTransform; } set { absoluteTransform = value; } }
    }
}
