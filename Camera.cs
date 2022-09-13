using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class Camera
    {
        public Vector3 CameraPosition { get; set; }
        public Vector3 CameraRight { get; set; }
        public Vector3 CameraUp { get; set; }
        public Vector3 CameraFront { get; set; }
        public Vector3 CameraWorldUp { get; set; } = Vector3.UnitY;
        public float Fov { get; set; } = 45.0f;

        public float CameraSpeed { get; set; } = 0.05f;

        public float AspectRatio { get; set; }

        public Camera(Vector3 position, Vector3 target, float aspectRation)
        {
            CameraPosition = position;
            CameraFront = -Vector3.Normalize(position - target);
            CameraRight = Vector3.Normalize(Vector3.Cross(CameraFront, CameraWorldUp));
            CameraUp = Vector3.Normalize(Vector3.Cross(CameraFront, CameraRight));
            AspectRatio = aspectRation;
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(CameraPosition, CameraPosition + CameraFront, CameraUp);
        }

        public Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(Fov), AspectRatio, 0.01f, 100.0f);
        }
    }
}
