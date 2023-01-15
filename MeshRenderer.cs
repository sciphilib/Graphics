using OpenTK.Graphics.OpenGL;
using Graphics.ECS;

namespace Graphics
{
    public class MeshRenderer : Component
    {
        public int VAO, VBO, colorVBO, EBO;
        public void GenBuffers()
        {
            VAO = GL.GenVertexArray();
            VBO = GL.GenBuffer();
            if (Owner?.GetComponent<RenderProps>()._color != null)
                colorVBO = GL.GenBuffer();
            EBO = GL.GenBuffer();
        }

        public void BindBuffers()
        {
            // vertices vbo
            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, Owner.GetComponent<Mesh>().vertices.Length * sizeof(float), Owner.GetComponent<Mesh>().vertices, BufferUsageHint.StaticDraw);
            // colors vbo
            if (Owner?.GetComponent<RenderProps>()._color != null)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, colorVBO);
                GL.BufferData(BufferTarget.ArrayBuffer, Owner.GetComponent<RenderProps>()._color.Length * sizeof(double), Owner.GetComponent<RenderProps>()._color, BufferUsageHint.StaticDraw);
            }
            // ebo
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Owner.GetComponent<Mesh>().indices.Length * sizeof(int), Owner.GetComponent<Mesh>().indices, BufferUsageHint.StaticDraw);
        }

        public void SetAttribArray()
        {
            // position attribute
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            // color attribute
            if (Owner?.GetComponent<RenderProps>()._color != null)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, colorVBO);
                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Double, false, 3 * sizeof(double), 0);
                GL.EnableVertexAttribArray(1);
            }
        }

        public void Init()
        {
            GenBuffers();
            BindBuffers();
            SetAttribArray();

            GL.BindVertexArray(0);
        }

        public void Render(CameraContext cameraContext)
        {
            Owner?.GetComponent<RenderProps>()._shader.Use();
            Owner?.GetComponent<RenderProps>()._shader.SetMat4("model", Owner.GetComponent<Transform>().transform);
            Owner?.GetComponent<RenderProps>()._shader.SetMat4("view", cameraContext.viewMatrix);
            Owner?.GetComponent<RenderProps>()._shader.SetMat4("projection", cameraContext.projectionMatrix);
            GL.BindVertexArray(VAO);
            GL.DrawElements(Owner.GetComponent<RenderProps>()._primitiveType, Owner.GetComponent<Mesh>().indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }
    }
}
