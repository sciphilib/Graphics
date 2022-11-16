using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graphics.ECS;
using OpenTK.Graphics.OpenGL;

namespace Graphics
{
    public class Mesh : Component
    {
        public float[] vertices;
        public int[] indices;
        public PrimitiveType PrimitiveType { get; set; }
        public Mesh(float[] vertices, int[] indices)
        {
            PrimitiveType = PrimitiveType.Triangles;
            this.vertices = vertices;
            this.indices = indices;
        }
    }
}
