using OpenTK;
using OpenTK.Graphics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace Graphics
{
    public class Game : GameWindow
    {
        public int WindowHeight { get; set; }
        public int WindowWidth { get; set; }
        public string WindowTitle { get; set; }
        public Game(int height, int width, string title) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            WindowHeight = height;
            WindowWidth = width;
            WindowTitle = title;
            this.CenterWindow(new Vector2i(WindowHeight, WindowWidth));
            this.Title = WindowTitle;
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(new Color4(0.5f, 0.5f, 0.5f, 1.0f));
            GL.Clear(ClearBufferMask.ColorBufferBit);
            this.Context.SwapBuffers();
        }
    }
}
