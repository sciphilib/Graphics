using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;



namespace Graphics
{
    public class MeshRenderer
    {
        public string VertexShaderPath { get; set; }
        public string FragmentShaderPath { get; set; }
        public Mesh Mesh { get; set; }
        public float[] Color { get; set; }

        private int VAO, VBO, colorVBO, EBO;
        private Shader shader;

        public MeshRenderer(Mesh mesh, string vertexShaderPath, string fragmentShaderPath, float[] color)
        {
            VertexShaderPath = vertexShaderPath;
            FragmentShaderPath = fragmentShaderPath;
            Color = color;
            Mesh = mesh;
            shader = new(VertexShaderPath, FragmentShaderPath);
        }
        
        public void GenBuffers()
        {
            VAO = GL.GenVertexArray();
            VBO = GL.GenBuffer();
            colorVBO = GL.GenBuffer();
            EBO = GL.GenBuffer();
        }

        public void BindBuffers()
        {
            // vertices vbo
            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, Mesh.vertices.Length * sizeof(float), Mesh.vertices, BufferUsageHint.StaticDraw);
            // colors vbo
            GL.BindBuffer(BufferTarget.ArrayBuffer, colorVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, Color.Length * sizeof(float), Color, BufferUsageHint.DynamicDraw);
            // ebo
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Mesh.indices.Length * sizeof(float), Mesh.indices, BufferUsageHint.StaticDraw);
            
        }

        public void SetAttribArray()
        {
            // position attribute
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            // color attribute
            GL.BindBuffer(BufferTarget.ArrayBuffer, colorVBO);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(1);
        }

        public void Init()
        {
            GenBuffers();
            BindBuffers();
            SetAttribArray();

            GL.BindVertexArray(0);
        }

        public void Render()
        {
            shader.Use();
            Matrix4 modelMatrix = Matrix4.CreateTranslation(-new Vector3(Mesh.vertices[0], Mesh.vertices[1], Mesh.vertices[2])) * Matrix4.CreateScale(0.0000000025f);
            Matrix4 viewMatrix = Matrix4.Identity;
            Matrix4 projectionMatrix = Matrix4.Identity;
            shader.SetMat4("model", modelMatrix);
            shader.SetMat4("view", viewMatrix);
            shader.SetMat4("projection", projectionMatrix);
            GL.BindVertexArray(VAO);
            GL.DrawElements(PrimitiveType.Triangles, Mesh.indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);

        }

    }
}
