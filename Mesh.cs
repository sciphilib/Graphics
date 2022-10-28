using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class Mesh
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
