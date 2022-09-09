using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Security.Cryptography.X509Certificates;

namespace Graphics
{
    public class Game : GameWindow
    {

        private int VBO;
        private int VAO;

        private Shader? shader;

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

        protected override void OnLoad()
        {
            shader = new Shader("D:\\Microsoft Visual Studio\\Repos\\Graphics\\Shaders\\VertexShader.glsl", "D:\\Microsoft Visual Studio\\Repos\\Graphics\\Shaders\\FragmentShader.glsl");

            GL.ClearColor(new Color4(0.5f, 0.5f, 0.5f, 1.0f));

            float[] vertices = new float[]
            {
                0.0f, 0.5f, 0.0f,
                0.5f, -0.5f, 0.0f,
                -0.5f, -0.5f, 0.0f
            };

            VBO = GL.GenBuffer();
            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
             

            base.OnLoad();
        }


        protected override void OnUnload()
        {
            shader?.Dispose();
            base.OnUnload();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            if (KeyboardState.IsKeyDown(Keys.Escape))
                Close();


            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            shader?.Use();
            GL.BindVertexArray(VAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);


            this.Context.SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }
    }
}
