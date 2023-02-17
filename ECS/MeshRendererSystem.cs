using Graphics.CameraStuff;
using Graphics.Components;
using OpenTK.Graphics.OpenGL;

namespace Graphics.ECS
{
    public class MeshRendererSystem : BaseSystem<Mesh>
    {

        public static void Render(CameraContext cameraContext)
        {

            foreach (var meshRenderer in components)
            {
                var owner = meshRenderer.Owner;
                owner.GetComponent<RenderProps>().Shader.Use();
                owner.GetComponent<RenderProps>().Shader.SetMat4("model", owner.GetComponent<Transform>().transform);
                owner.GetComponent<RenderProps>().Shader.SetMat4("view", cameraContext.viewMatrix);
                owner.GetComponent<RenderProps>().Shader.SetMat4("projection", cameraContext.projectionMatrix);
                GL.BindVertexArray(owner.GetComponent<RenderProps>().VAO);
                GL.DrawElements(owner.GetComponent<RenderProps>().PrimitiveType, owner.GetComponent<Mesh>().indices.Length, DrawElementsType.UnsignedInt, 0);
                GL.BindVertexArray(0);

                //Owner?.GetComponent<RenderProps>().Shader.Use();
                //Owner?.GetComponent<RenderProps>().Shader.SetMat4("model", Owner.GetComponent<Transform>().transform);
                //Owner?.GetComponent<RenderProps>().Shader.SetMat4("view", cameraContext.viewMatrix);
                //Owner?.GetComponent<RenderProps>().Shader.SetMat4("projection", cameraContext.projectionMatrix);
                //GL.BindVertexArray(VAO);
                //GL.DrawElements(Owner.GetComponent<RenderProps>().PrimitiveType, Owner.GetComponent<Mesh>().indices.Length, DrawElementsType.UnsignedInt, 0);
                //GL.BindVertexArray(0);

            }

        }
    } 
}
