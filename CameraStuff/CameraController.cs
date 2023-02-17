using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graphics.WindowStuff;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;


namespace Graphics.CameraStuff
{
    public class CameraController
    {
        private Camera _camera;
        public CameraController(Camera camera)
        {
            _camera = camera;
        }

        public void CameraUpdate(Vector2 offset, bool constrainPitch = true)
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

        public void CameraKeyboardProcess()
        {
            if (WindowInput.IsKeyDown(Keys.W))
            {
                _camera.CameraPosition += _camera.CameraSpeed * WindowInput.GetDeltaTime() * _camera.CameraFront;
            }
            if (WindowInput.IsKeyDown(Keys.S))
            {
                _camera.CameraPosition += -(_camera.CameraSpeed * WindowInput.GetDeltaTime() * _camera.CameraFront);
            }
            if (WindowInput.IsKeyDown(Keys.A))
            {
                _camera.CameraPosition += -(_camera.CameraSpeed * WindowInput.GetDeltaTime() * Vector3.Normalize(Vector3.Cross(_camera.CameraFront, _camera.CameraUp)));
            }
            if (WindowInput.IsKeyDown(Keys.D))
            {
                _camera.CameraPosition += _camera.CameraSpeed * WindowInput.GetDeltaTime() * Vector3.Normalize(Vector3.Cross(_camera.CameraFront, _camera.CameraUp));
            }
            if (WindowInput.IsKeyDown(Keys.LeftControl))
            {
                _camera.CameraPosition += -(_camera.CameraSpeed * WindowInput.GetDeltaTime() * Vector3.Normalize(_camera.CameraUp));
            }
            if (WindowInput.IsKeyDown(Keys.LeftShift))
            {
                _camera.CameraPosition += _camera.CameraSpeed * WindowInput.GetDeltaTime() * Vector3.Normalize(_camera.CameraUp);
            }
        }
    }
}
