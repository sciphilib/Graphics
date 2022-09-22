using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
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
        private static Window _window;
        
        // time variables
        private static float _lastFrame = 0.0f;
        private static float _deltaTime = 0.0f;

        // mouse state variables
        private static MouseState _mouseState;
        private static MouseState _lastMouseState;
        private static KeyboardState _keyboardState;
        private static KeyboardState _lastKeyboardState;
        private static bool _firstMove = true;
        private static Vector2 _mousePos;
        private static Vector2 _deltaMouse;

        // singleton
        private static WindowInput _instance;
        private static object syncRoot = new Object();


        private WindowInput(Window window)
        {
            _window = window;
            _window.BindUpdateCallback(ProcessInput);
        }

        public static WindowInput getInstance(Window window)
        {
            if (_instance == null)
            {
                lock (syncRoot)
                {
                    if (_instance == null)
                        _instance = new WindowInput(window);
                }
            }
            return _instance;
        }
        private static void ProcessInput()
        {
            _lastKeyboardState = _keyboardState;
            _lastMouseState = _mouseState;

            //float currentFrame = (float)_window.UpdateTime;
            //_deltaTime = currentFrame / _lastFrame;
            _deltaTime = Window.GetDeltaTime();
            //_lastFrame = currentFrame;

            // mouse input
            _mouseState = _window.MouseState;
            _keyboardState = _window.KeyboardState;
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
        }

        public static Vector2 GetMousePos()
        {
            return _mousePos;
        }

        public static Vector2 GetDeltaMouse()
        {
            return _deltaMouse;
        }

        public static bool IsKeyDown(Keys key)
        {
            return _keyboardState.IsKeyDown(key);
        }

        public static bool IsKeyPressed(Keys key)
        {
            return _keyboardState.IsKeyPressed(key);
        }

        public static float GetDeltaTime()
        {
            return _deltaTime;
        }

    }
}