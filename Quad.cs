using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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
