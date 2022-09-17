using Graphics.ImGUI;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class Window : GameWindow
    {
        public delegate void OnEventCallback();

        private ImGuiController _controller;
        private Renderer _renderer;
        private WindowInput _windowInput;

        private OnEventCallback? OnUpdate;
        private OnEventCallback? OnRender;
        private OnEventCallback? OnDrawGUI;

        private bool _isGUI;
        public Window(int width, int height, string title) :
            base(GameWindowSettings.Default, new NativeWindowSettings() { Size = new Vector2i(width, height),
                Title = title, APIVersion = new Version(3, 3) })
        {
            CenterWindow(new Vector2i(Size.X, Size.Y));
            _isGUI = false;
        }

        public void BindUpdateCallback(OnEventCallback call)
        {
            OnUpdate = call;
        }

        public void BindRenderCallback(OnEventCallback call)
        {
            OnRender = call;
        }
        public void BindDrawGUICallback(OnEventCallback call)
        {
            OnDrawGUI = call;
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            OnUpdate?.Invoke();

            if (_isGUI)
                _renderer.GetCamera().GetCameraController().CameraUpdate(WindowInput.GetDeltaMouse(), true);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            // render scene objects
            OnRender?.Invoke();
            _controller?.Update(this, (float)args.Time);
            
            // render gui
            if (_isGUI)
            {
                CursorState = CursorState.Normal;
                OnDrawGUI?.Invoke();
            }
            else
                CursorState = CursorState.Grabbed;
            _controller?.Render();
            
            Context.SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs args)
        {
            base.OnResize(args);
            _controller?.WindowResized(ClientSize.X, ClientSize.Y);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            _controller = new ImGuiController(ClientSize.X, ClientSize.Y);
            _renderer = new(this);
            _windowInput = new(this);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
        }

        public Renderer GetRenderer()
        {
            return _renderer;
        }

        public WindowInput GetWindowInput()
        {
            return _windowInput;
        }

        public void ChangeIsGUI()
        {
            _isGUI = !_isGUI;
        }

        public bool GetIsGUI()
        {
            return _isGUI;
        }
    }
}
