using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

        //mouse state variables
        private static MouseState _mouseState;
        private static MouseState _prevMouseState;
        private static bool _firstMove = true;
        private static System.Numerics.Vector2 _mousePos;
        private static System.Numerics.Vector2 _deltaMouse;


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

            if (_window.KeyboardState.IsKeyReleased(Keys.Space))
                _window.ChangeIsGUI();

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
            

            //mouse input
            _prevMouseState = _mouseState;
            _mouseState = _window.MouseState;
            if (_firstMove)
            {
                _mousePos = new(_mouseState.X, _mouseState.Y);
                _firstMove = false;
            }
            else
            {
                var deltaX = _mouseState.X - _mousePos.X;
                var deltaY = _mouseState.Y - _mousePos.Y;

                _deltaMouse = new(deltaX, deltaY);
                _mousePos = new(_mouseState.X, _mouseState.Y);
            }
            if (!_window.GetIsGUI())
                _camera.GetCameraController().CameraMouseLook(_deltaMouse, true);
        }

        public System.Numerics.Vector2 GetMousePos()
        {
            return _mousePos;
        }

        public System.Numerics.Vector2 GetDeltaMouse()
        {
            return _deltaMouse;
        }
    }
}