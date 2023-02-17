using Graphics.ECS;
using Graphics.Shaders;
using OpenTK.Graphics.OpenGL;


namespace Graphics.Components
{
    public class RenderProps : Component
    {
        public string VertexShaderPath { get; }
        public string FragmentShaderPath { get; }
        public double[]? Color { get; }
        public Shader Shader { get; }
        public PrimitiveType PrimitiveType { get; }
        public int VAO { get; private set; }
        public int VBO { get; private set; }
        public int ColorVBO { get; private set; }
        public int EBO { get; private set; }

        public RenderProps(string fragmentShaderPath, string vertexShaderPath, double[]? color, PrimitiveType pType, Entity owner)
        {
            FragmentShaderPath = fragmentShaderPath;
            VertexShaderPath = vertexShaderPath;
            Shader = new(vertexShaderPath, fragmentShaderPath);
            Color = color;
            PrimitiveType = pType;
            Owner = owner;
            Init();
        }

        ~RenderProps()
        {
            Shader.Dispose();
        }

        public void Init()
        {
            GenBuffers();
            BindBuffers();
            SetAttribArrays();
            GL.BindVertexArray(0);
        }

        private void SetAttribArrays()
        {
            SetArrayBuffer(VBO, 0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            if (Color != null)
            {
                SetArrayBuffer(ColorVBO, 1, 3, VertexAttribPointerType.Double, false, 3 * sizeof(double), 0);
            }
        }

        private void GenBuffers()
        {
            VAO = GL.GenVertexArray();
            VBO = GL.GenBuffer();
            if (Color != null)
                ColorVBO = GL.GenBuffer();
            EBO = GL.GenBuffer();
        }

        private void BindBuffers()
        {
            BindColorVBO();
            BindVerticesVBO();
            BindEBO();
        }

        private void SetArrayBuffer(int buffer, int index, int size, VertexAttribPointerType type, bool normalized, int stride, int offset)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
            GL.VertexAttribPointer(index, size, type, normalized, stride, offset);
            GL.EnableVertexAttribArray(index);
        }

        private void BindVerticesVBO()
        {
            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, Owner.GetComponent<Mesh>().vertices.Length * sizeof(float), Owner.GetComponent<Mesh>().vertices, BufferUsageHint.StaticDraw);
        }

        private void BindColorVBO()
        {
            if (Color != null)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, ColorVBO);
                GL.BufferData(BufferTarget.ArrayBuffer, Color.Length * sizeof(double), Color, BufferUsageHint.StaticDraw);
            }
        }
        private void BindEBO()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Owner.GetComponent<Mesh>().indices.Length * sizeof(int), Owner.GetComponent<Mesh>().indices, BufferUsageHint.StaticDraw);
        }
    }
}
