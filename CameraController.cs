using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class CameraController
    {
        Camera _camera;
        public CameraController(Camera camera)
        {
            _camera = camera;
        }

        public void CameraMouseLook(Vector2 offset, bool constrainPitch)
        {
            offset.X *= _camera.MouseSensitivity;
            offset.Y *= _camera.MouseSensitivity;

            _camera.Yaw += offset.X;
            _camera.Pitch -= offset.Y;

            if (constrainPitch)
            {
                if (_camera.Pitch > 89.0f)
                    _camera.Pitch = 89.0f;
                if (_camera.Pitch < -89.0f)
                    _camera.Pitch = -89.0f;
            }

            _camera.UpdateCameraVectors();
        }
    }
}
