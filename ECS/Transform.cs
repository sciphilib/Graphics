using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace Graphics.ECS
{
    public class Transform : Component
    {
        public Vector3 position;
        public Matrix4 transform;

        public Transform()
        {
            position = Vector3.Zero;
            transform = Matrix4.Identity;
            TransformSystem.Register(this);
        }
        public Transform(Vector3 position, Matrix4 transform)
        {
            this.position = position;
            this.transform = transform;
        }

        public void Translate(Vector3 translation)
        {
            position += translation;
            transform *= Matrix4.CreateTranslation(translation);
        }

        public void Scale(float scaleFactor)
        {
            transform *= Matrix4.CreateScale(scaleFactor);
        }

        public void RotateX(float angle)
        {
            transform *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(angle));
        }

        public void RotateY(float angle)
        {
            transform *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(angle));
        }

        public void RotateZ(float angle)
        {
            transform *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(angle));
        }
    }
}
