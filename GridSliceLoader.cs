using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class GridSliceLoader
    {
        public int ISlice { get; set; }
        public int JSlice { get; set; }
        public int KSlice { get; set; }

        public GridSliceLoader()
        {
            ISlice = -1;
            JSlice = -1;
            KSlice = -1;
        }

        public GridSliceLoader(int iSlice, int jSlice, int kSlice)
        {
            ISlice = iSlice;
            JSlice = jSlice;
            KSlice = kSlice;
        }

        public static void MakeSlice(Grid grid, GridSliceLoader slice)
        {
            for (int k = 0; k < grid.SizeZ; k++)
            {
                for (int i = 0; i < grid.SizeX; i++)
                {
                    for (int j = 0; j < grid.SizeY; j++)
                    {
                        grid.GetCell(i, j, k).isActive = false;
                    }
                }
            }
            MakeSliceK(grid, slice);
            MakeSliceI(grid, slice);
            MakeSliceJ(grid, slice);
        }

        public static void MakeZeroSlice(Grid grid, GridSliceLoader slice)
        {
            for (int k = 0; k < grid.SizeZ; k++)
            {
                for (int i = 0; i < grid.SizeX; i++)
                {
                    for (int j = 0; j < grid.SizeY; j++)
                    {
                        grid.GetCell(i, j, k).isActive = true;
                    }
                }
            }
        }
            
        public static void MakeSliceK(Grid grid, GridSliceLoader slice)
        {
            for (int k = 0; k < 1; k++)
            {
                for (int i = 0; i < grid.SizeX; i++)
                {
                    for (int j = 0; j < grid.SizeY; j++)
                    {
                        grid.GetCell(i, j, slice.KSlice).isActive = true;
                    }
                }
            }
        }

        public static void MakeSliceI(Grid grid, GridSliceLoader slice)
        {
            for (int i = 0; i < 1; i++)
            {
                for (int k = 0; k < grid.SizeZ; k++)
                {
                    for (int j = 0; j < grid.SizeY; j++)
                    {
                        grid.GetCell(slice.ISlice, j, k).isActive = true;
                    }
                }
            }
        }

        public static void MakeSliceJ(Grid grid, GridSliceLoader slice)
        {
            for (int j = 0; j < 1; j++)
            {
                for (int k = 0; k < grid.SizeZ; k++)
                {
                    for (int i = 0; i < grid.SizeX; i++)
                    {
                        grid.GetCell(i, slice.JSlice, k).isActive = true;
                    }
                }
            }
        }

        public static bool operator ==(GridSliceLoader gridSlice1, GridSliceLoader gridSlice2)
        {
            return gridSlice1.ISlice == gridSlice2.ISlice
                && gridSlice1.JSlice == gridSlice2.JSlice
                && gridSlice1.KSlice == gridSlice2.KSlice;
        }

        public static bool operator !=(GridSliceLoader gridSlice1, GridSliceLoader gridSlice2)
        {
            return gridSlice1.ISlice != gridSlice2.ISlice
                || gridSlice1.JSlice != gridSlice2.JSlice
                || gridSlice1.KSlice != gridSlice2.KSlice;
        }
    }
}
