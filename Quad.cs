using System.Numerics;

namespace Graphics
{
    public class Quad
    {

        public Vector3[] vertices;
        public bool isActive;
        public float property;

        public Quad()
        {
            vertices = new Vector3[4];
        }
    }
}
