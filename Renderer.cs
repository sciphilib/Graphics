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
        private Camera _camera;
        private Shader _shader;
        private Shader _sunShader;
        private GameWindow _window;

        // scene variables
        private int VBO;
        private int VAO;
        private int sunVAO;
        private bool IsMeshMode = false;

        // imgui variables
        private System.Numerics.Vector3 _objectPos = System.Numerics.Vector3.Zero;
        private System.Numerics.Vector3 _objectRot = System.Numerics.Vector3.Zero;

        public Renderer(Window window)
        {
            _window = window;
            OnLoad();   
            window.BindRenderCallback(OnRender);
            window.BindDrawGUICallback(OnImGuiDraw);
        }

        ~Renderer()
        {
            OnUnload();
        }
        private void OnLoad()
        {
            _camera = new(new Vector3(1.0f, 1.0f, 3.0f), new Vector3(0.0f, 0.0f, 0.0f), (float)_window.Size.X / _window.Size.Y);
            _shader = new("Shaders\\VertexLightingShader.glsl", "Shaders\\FragmentLightingShader.glsl");
            _sunShader = new("Shaders\\VertexSunShader.glsl", "Shaders\\FragmentSunShader.glsl");

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

            // object
            VBO = GL.GenBuffer();
            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            // sun
            sunVAO = GL.GenVertexArray();
            GL.BindVertexArray(sunVAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);



            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        private void OnRender()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            _shader?.Use();
            Matrix4 modelMatrix = Matrix4.Identity;
            Matrix4 viewMatrix;
            Matrix4 projectionMatrix;
            Matrix4 rotMatrix;
            Matrix4 transMatrix;

            Vector3 rotVec = new(_objectRot.X, _objectRot.Y, _objectRot.Z);
            rotMatrix = Matrix4.CreateRotationX(rotVec.X) * Matrix4.CreateRotationY(rotVec.Y) * Matrix4.CreateRotationZ(rotVec.Z);
            transMatrix = Matrix4.CreateTranslation(new Vector3(_objectPos.X, _objectPos.Y, _objectPos.Z));
            modelMatrix = rotMatrix * transMatrix;


            if (_camera != null)
            {
                viewMatrix = _camera.GetViewMatrix();
                projectionMatrix = _camera.GetProjectionMatrix();
            }
            else
            {
                viewMatrix = Matrix4.Identity;
                projectionMatrix = Matrix4.Identity;
            }

            // object's shader settings
            _shader?.SetMat4("model", modelMatrix);
            _shader?.SetMat4("view", viewMatrix);
            _shader?.SetMat4("projection", projectionMatrix);

            GL.PolygonMode(MaterialFace.FrontAndBack, IsMeshMode ? PolygonMode.Line : PolygonMode.Fill);    
            GL.BindVertexArray(VAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            // sun's shader settings
            _sunShader?.Use();
            Matrix4 SunModelMatrix = Matrix4.CreateScale(0.5f) * Matrix4.CreateTranslation(new Vector3(0.0f, 0.0f, 6.0f));
            _sunShader?.SetMat4("model", SunModelMatrix);
            _sunShader?.SetMat4("view", viewMatrix);
            _sunShader?.SetMat4("projection", projectionMatrix);
            GL.BindVertexArray(sunVAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }

        private void OnImGuiDraw()
        {
            ImGui.SliderFloat3("Cube position", ref _objectPos, -5.0f, 5.0f);
            ImGui.SliderFloat3("Cube rotation", ref _objectRot, MathHelper.DegreesToRadians(0.0f), MathHelper.DegreesToRadians(360.0f));
            Util.CheckGLError("End of frame");
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
        }

        private void OnUnload()
        {
            _shader?.Dispose();
        }

        public void ChangeMeshMode()
        {
            IsMeshMode = !IsMeshMode;
        }

        public Camera GetCamera()
        {
            return _camera;
        }
    }
}
