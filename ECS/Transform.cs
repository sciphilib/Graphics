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
        }
        public Transform(Vector3 position, Matrix4 transform)
        {
            this.position = position;
            this.transform = transform;
        }


        //todo
        //not correct working
        
        public void Translate(Vector3 translation)
        {
            position += translation;
            var m = Matrix4.CreateTranslation(translation);
            transform = m * transform;
        }

        public void Scale(float scaleFactor)
        {
            var m = Matrix4.CreateScale(scaleFactor);
            transform = m * transform;
        }

        public void RotateX(float angle)
        {
            var m = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(angle));
            transform = m * transform;
        }

        public void RotateY(float angle)
        {
            var m = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(angle));
            transform = m * transform;
        }

        public void RotateZ(float angle)
        {
            var m = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(angle));
            transform = m * transform;
        }
    }
}
