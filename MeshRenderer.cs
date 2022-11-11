using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Graphics.ECS;



namespace Graphics
{
    public class MeshRenderer : Component
    {
        public string VertexShaderPath { get; set; }
        public string FragmentShaderPath { get; set; }
        public double[]? Color { get; set; }

        public Mesh? mesh;
        public int VAO, VBO, colorVBO, EBO;
        public Shader shader;

        public MeshRenderer(string vertexShaderPath, string fragmentShaderPath, double[] color)
        {
            VertexShaderPath = vertexShaderPath;
            FragmentShaderPath = fragmentShaderPath;
            Color = color;
            shader = new(VertexShaderPath, FragmentShaderPath);
        }

        public MeshRenderer(string vertexShaderPath, string fragmentShaderPath)
        {
            VertexShaderPath = vertexShaderPath;
            FragmentShaderPath = fragmentShaderPath;
            shader = new(VertexShaderPath, FragmentShaderPath);
        }

        ~MeshRenderer()
        {
            shader.Dispose();
        }
        
        public void GenBuffers()
        {
            VAO = GL.GenVertexArray();
            VBO = GL.GenBuffer();
            if (Color != null)
                colorVBO = GL.GenBuffer();
            EBO = GL.GenBuffer();
        }

        public void BindBuffers()
        {
            // vertices vbo
            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, mesh.vertices.Length * sizeof(float), mesh.vertices, BufferUsageHint.StaticDraw);
            // colors vbo
            if (Color != null)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, colorVBO);
                GL.BufferData(BufferTarget.ArrayBuffer, Color.Length * sizeof(double), Color, BufferUsageHint.DynamicDraw);
            }
            // ebo
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, mesh.indices.Length * sizeof(int), mesh.indices, BufferUsageHint.StaticDraw);
        }

        public void SetAttribArray()
        {
            // position attribute
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            // color attribute
            if (Color != null)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, colorVBO);
                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Double, false, 3 * sizeof(double), 0);
                GL.EnableVertexAttribArray(1);
            }
        }

        public void Init()
        {
            mesh = Owner?.GetComponent<Mesh>();

            GenBuffers();
            BindBuffers();
            SetAttribArray();

            GL.BindVertexArray(0);
        }

        public void Render(Matrix4 view, Matrix4 proj, PrimitiveType primitiveType)
        {
            shader.Use();
            var transform = mesh?.Owner?.GetComponent<Transform>();
            shader.SetMat4("model", transform.transform);
            shader.SetMat4("view", view);
            shader.SetMat4("projection", proj);
            GL.BindVertexArray(VAO);
            GL.DrawElements(primitiveType, mesh.indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }
    }
}
