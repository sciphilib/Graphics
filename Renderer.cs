using Graphics.ImGUI;
using ImGuiNET;
using OpenTK.Graphics.OpenGL;
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

    public class Renderer
    {
        private Camera? _camera;
        private Shader? _shader;
        private GameWindow _window;
        // scene variables
        private int VBO;
        private int VAO;
        private bool IsMeshMode = false;

        public Renderer(Window window)
        {
            _window = window;
            OnLoad();   
            window.BindRenderCallback(OnRender);
            window.BindDrawGUICallback(OnImGuiDraw);
            window.BindUpdateCallback(OnUpdate);
        }

        ~Renderer()
        {
            OnUnload();
        }
        private void OnLoad()
        {
            _camera = new(new Vector3(1.0f, 1.0f, 3.0f), new Vector3(0.0f, 0.0f, 0.0f), (float)_window.Size.X / _window.Size.Y);
            _shader = new("Shaders\\VertexShader.glsl", "Shaders\\FragmentShader.glsl");

            IsMeshMode = false;
            GL.Enable(EnableCap.DepthTest);
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
        }

        private void OnRender()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            _shader?.Use();
            var Mmodel = Matrix4.Identity;
            Matrix4 Mview;
            Matrix4 Mprojection;
            if (_camera != null)
            {
                Mview = _camera.GetViewMatrix();
                Mprojection = _camera.GetProjectionMatrix();
            }
            else
            {
                Mview = Matrix4.Identity;
                Mprojection = Matrix4.Identity;
            }
            _shader?.SetMat4("model", Mmodel);
            _shader?.SetMat4("view", Mview);
            _shader?.SetMat4("projection", Mprojection);

            GL.BindVertexArray(VAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }

        private void OnImGuiDraw()
        {
            ImGui.ShowDemoWindow();
            Util.CheckGLError("End of frame");
        }

        private void OnUpdate()
        {
            if (_window.KeyboardState.IsKeyDown(Keys.Escape))
                _window.Close();
        }

        private void OnUnload()
        {
            _shader?.Dispose();
        }

    }
}
