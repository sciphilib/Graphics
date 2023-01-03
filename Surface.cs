using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class Surface
    {
        public int SizeI { get; set; }
        public int SizeJ { get; set; }
        public int Size { get; set; }
        public int Capacity { get; set; }
        public float MaxProperty { get; set; }
        public float MinProperty { get; set; }

        private Quad[,] quads;

        public Surface(int sizeI, int sizeJ)
        {
            SizeI = sizeI;
            SizeJ = sizeJ;
            Size = Capacity = SizeI * SizeJ;
            quads = new Quad[SizeI, SizeJ];
        }

        public ref Quad GetQuad(int i, int j)
        {
            return ref quads[i, j];
        }
    }
}
