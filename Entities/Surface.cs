using Graphics.ECS;

namespace Graphics.Entities
{
    public class Surface : Entity
    {
        public int SizeX { get; set; }
        public int SizeY { get; set; }
        public int Size { get; set; }
        public int Capacity { get; set; }
        public float MaxProperty { get; set; }
        public float MinProperty { get; set; }

        private Quad[,] quads;

        public Surface(int sizeI, int sizeJ)
        {
            SizeX = sizeI;
            SizeY = sizeJ;
            Size = Capacity = SizeX * SizeY;
            quads = new Quad[SizeX, SizeY];
        }

        public ref Quad GetQuad(int i, int j)
        {
            return ref quads[i, j];
        }
    }
}
