using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Animation
{
    public class AnimationClip
    {
        private List<AnimatedBone> animatedBones = new List<AnimatedBone>();
        public string Name;
        public double Duration;
        public List<AnimatedBone> AnimatedBoneCollection { get { return animatedBones; } }

        public class AnimatedBone
        {
            private string name = "";
            private List<Keyframe> keyframes = new List<Keyframe>();

            public string Name { get { return name; } set { name = value; } }
            public List<Keyframe> Keyframes { get { return keyframes; } }
        }

        public class Keyframe
        {
            public double Time;
            public Quaternion Rotation;
            public Vector3 Translation;

            public Keyframe()
            {
            }
            public Matrix Transform
            {
                get
                {
                    return Matrix.CreateFromQuaternion(Rotation) * Matrix.CreateTranslation(Translation);
                }
                set
                {
                    Matrix transform = value;
                    transform.Right = Vector3.Normalize(transform.Right);
                    transform.Up = Vector3.Normalize(transform.Up);
                    transform.Backward = Vector3.Normalize(transform.Backward);
                    Rotation = Quaternion.CreateFromRotationMatrix(transform);
                    Translation = transform.Translation;
                }
            }
            public override bool Equals(object obj)
            {
                if(obj is Keyframe)
                    return this == (obj as Keyframe);
                else
                    return false;
            }
            public static bool operator ==(Keyframe lhs, Keyframe rhs)
            {
                return (lhs.Rotation == rhs.Rotation) && (lhs.Translation == rhs.Translation);
            }
            public static bool operator !=(Keyframe lhs, Keyframe rhs)
            {
                return !(lhs == rhs);
            }
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }
    }
}
