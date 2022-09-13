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
        public WindowInput(Window window)
        {
            _window = window;
            _window.BindUpdateCallback(ProcessInput);
        }

        private void ProcessInput()
        {
            if (_window.KeyboardState.IsKeyDown(Keys.Escape))
                _window.Close();

            if (_window.KeyboardState.IsKeyPressed(Keys.Q))
            {
                _window.GetRenderer().ChangeMeshMode();
            }
        }
    }
}
