using System.Numerics;
using Graphics.ECS;

namespace Graphics.Entities
{
    public class Quad : Entity
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
