using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class WindowInput
    {
        private Window _window;
        private Camera _camera;

        //time variables
        private float _lastFrame = 0.0f;
        private float _deltaTime = 0.0f;


        public WindowInput(Window window)
        {
            _window = window;
            _window.BindUpdateCallback(ProcessInput);
            _camera = _window.GetRenderer().GetCamera();
        }

        private void ProcessInput()
        {
            if (_window.KeyboardState.IsKeyDown(Keys.Escape))
                _window.Close();

            if (_window.KeyboardState.IsKeyPressed(Keys.Q))
                _window.GetRenderer().ChangeMeshMode();

            //WASD camera
            float currentFrame = (float)_window.UpdateTime;
            _deltaTime = currentFrame / _lastFrame;
            _lastFrame = currentFrame;
            if (_window.KeyboardState.IsKeyDown(Keys.W))
            {
                _camera.CameraPosition += _camera.CameraSpeed * _deltaTime * _camera.CameraFront;
            }
            if (_window.KeyboardState.IsKeyDown(Keys.S))
            {
                _camera.CameraPosition += -(_camera.CameraSpeed * _deltaTime * _camera.CameraFront);
            }
            if (_window.KeyboardState.IsKeyDown(Keys.A))
            {
                _camera.CameraPosition += -(_camera.CameraSpeed * _deltaTime * Vector3.Normalize(Vector3.Cross(_camera.CameraFront, _camera.CameraUp)));
            }
            if (_window.KeyboardState.IsKeyDown(Keys.D))
            {
                _camera.CameraPosition += _camera.CameraSpeed * _deltaTime * Vector3.Normalize(Vector3.Cross(_camera.CameraFront, _camera.CameraUp));
            }
        }
    }
}
