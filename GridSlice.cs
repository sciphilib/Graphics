using Graphics.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class GridSlice : Entity
    {
        public int ISlice { get; }
        public int JSlice { get; }
        public int KSlice { get; }
        public int SizeX { get; set; }
        public int SizeY { get; set; }
        public int SizeZ { get; set; }
        public int Size { get; set; }

        public GridSlice(Grid grid, int iSlice, int jSlice, int kSlice)
        {
            ISlice = iSlice;
            JSlice = jSlice;
            KSlice = kSlice;
            SizeX = grid.SizeX;
            SizeY = grid.SizeY;
            SizeZ = grid.SizeZ;
            if (ISlice != 0)
            {
                Size += grid.SizeY * grid.SizeZ;
            }
            if (JSlice != 0)
            {
                Size += grid.SizeX * grid.SizeZ;
            }
            if (KSlice != 0)
            {
                Size += grid.SizeX * grid.SizeY;
            }
        }

    }
}
