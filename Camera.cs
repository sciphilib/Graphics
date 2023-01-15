using OpenTK.Mathematics;

namespace Graphics
{
    public class Camera
    {
        private CameraController _cameraController;

        public Vector3 CameraPosition { get; set; }
        public Vector3 CameraRight { get; set; }
        public Vector3 CameraUp { get; set; }
        public Vector3 CameraFront { get; set; }
        public Vector3 CameraWorldUp { get; set; } = Vector3.UnitY;
        public float FOV { get; set; } = 45.0f;
        public float CameraSpeed { get; set; } = 10.0f;
        public float AspectRatio { get; set; }
        public float MouseSensitivity { get; set; } = 0.1f;
        public float Yaw { get; set; } = -45.0f;
        public float Pitch { get; set; } = -10.0f;
        public Camera(Vector3 position, Vector3 target, float aspectRation)
        {
            _cameraController = new(this);
            CameraPosition = position;
            CameraFront = -Vector3.Normalize(position - target);
            AspectRatio = aspectRation;
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(CameraPosition, CameraPosition + CameraFront, CameraUp);
        }

        public Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FOV), AspectRatio, 0.01f, 100.0f);
        }

        public void UpdateCameraVectors()
        {
            Vector3 front = new()
            {
                X = (float)(Math.Cos(MathHelper.DegreesToRadians(Yaw)) * Math.Cos(MathHelper.DegreesToRadians(Pitch))),
                Y = (float)(Math.Sin(MathHelper.DegreesToRadians(Pitch))),
                Z = (float)(Math.Sin(MathHelper.DegreesToRadians(Yaw)) * Math.Cos(MathHelper.DegreesToRadians(Pitch)))
            };
            CameraFront = Vector3.Normalize(front);
            CameraRight = Vector3.Normalize(Vector3.Cross(CameraFront, CameraWorldUp));
            CameraUp = Vector3.Normalize(Vector3.Cross(CameraRight, CameraFront));
        }

        public CameraController GetCameraController()
        {
            return _cameraController;
        }
    }
}
