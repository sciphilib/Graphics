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

namespace Graphics
{
    public class Game : GameWindow
    {

        private int VBO;
        private int VAO;

        private Shader? shader;
        private Camera camera;
        private bool IsMeshMode;

        public int WindowHeight { get; set; }
        public int WindowWidth { get; set; }
        public string WindowTitle { get; set; }
        public Game(int width, int height, string title) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            WindowHeight = height;
            WindowWidth = width;
            WindowTitle = title;
            this.CenterWindow(new Vector2i(WindowWidth, WindowHeight));
            this.Title = WindowTitle;
            camera = new(new Vector3(1.0f, 1.0f, 3.0f), new Vector3(0.0f, 0.0f, 0.0f), (float) WindowWidth / WindowHeight);

        }

        protected override void OnLoad()
        {
            IsMeshMode = false;
            shader = new Shader("Shaders\\VertexShader.glsl", "Shaders\\FragmentShader.glsl");

            GL.ClearColor(new Color4(0.5f, 0.5f, 0.5f, 1.0f));

            float[] vertices = new float[]
            {
                -0.5f, -0.5f, -0.5f,
                 0.5f, -0.5f, -0.5f,
                 0.5f,  0.5f, -0.5f,
                 0.5f,  0.5f, -0.5f,
                -0.5f,  0.5f, -0.5f,
                -0.5f, -0.5f, -0.5f,

                -0.5f, -0.5f,  0.5f,
                 0.5f, -0.5f,  0.5f,
                 0.5f,  0.5f,  0.5f,
                 0.5f,  0.5f,  0.5f,
                -0.5f,  0.5f,  0.5f,
                -0.5f, -0.5f,  0.5f,

                -0.5f,  0.5f,  0.5f,
                -0.5f,  0.5f, -0.5f,
                -0.5f, -0.5f, -0.5f,
                -0.5f, -0.5f, -0.5f,
                -0.5f, -0.5f,  0.5f,
                -0.5f,  0.5f,  0.5f,

                 0.5f,  0.5f,  0.5f,
                 0.5f,  0.5f, -0.5f,
                 0.5f, -0.5f, -0.5f,
                 0.5f, -0.5f, -0.5f,
                 0.5f, -0.5f,  0.5f,
                 0.5f,  0.5f,  0.5f,

                -0.5f, -0.5f, -0.5f,
                 0.5f, -0.5f, -0.5f,
                 0.5f, -0.5f,  0.5f,
                 0.5f, -0.5f,  0.5f,
                -0.5f, -0.5f,  0.5f,
                -0.5f, -0.5f, -0.5f,

                -0.5f,  0.5f, -0.5f,
                 0.5f,  0.5f, -0.5f,
                 0.5f,  0.5f,  0.5f,
                 0.5f,  0.5f,  0.5f,
                -0.5f,  0.5f,  0.5f,
                -0.5f,  0.5f, -0.5f,
            };

            VBO = GL.GenBuffer();
            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
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

            if (KeyboardState.IsKeyPressed(Keys.Q))
            {
                IsMeshMode = !IsMeshMode;
                GL.PolygonMode(MaterialFace.FrontAndBack, IsMeshMode ? PolygonMode.Line : PolygonMode.Fill);
            }

            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            shader?.Use();
            var Mmodel = Matrix4.Identity;
            Matrix4 Mview = camera.GetViewMatrix();
            Matrix4 Mprojection = camera.GetProjectionMatrix();                          
            shader?.SetMat4("model", Mmodel);
            shader?.SetMat4("view", Mview);
            shader?.SetMat4("projection", Mprojection);

            GL.BindVertexArray(VAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);


            this.Context.SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }
    }
}
