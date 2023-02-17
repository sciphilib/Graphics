using Graphics.ECS;

namespace Graphics.Components
{
    public class Mesh : Component
    {
        public float[] vertices;
        public int[] indices;
        public Mesh(float[] vertices, int[] indices)
        {
            this.vertices = vertices;
            this.indices = indices;
        }
    }
}
