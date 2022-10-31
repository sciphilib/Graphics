using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class Grid
    {
        public int Capacity { get; }
        public int Size { get; set; }
        public int SizeX { get; }
        public int SizeY { get; }
        public int SizeZ { get; }

        private Cell[,,] cells;

        public Grid(int sizeX, int sizeY, int sizeZ)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            SizeZ = sizeZ;
            Capacity = sizeX * sizeY * sizeZ;
            Size = Capacity;
            cells = new Cell[sizeZ, sizeX, sizeY];
        }
        public void PrintGrid()
        {
            Console.WriteLine($"Grid capacity = {Capacity}");
            Console.WriteLine($"Grid size = {Size}");
            Console.WriteLine($"Z = {SizeZ}");
            Console.WriteLine($"Y = {SizeY}");
            Console.WriteLine($"X = {SizeX}");
            foreach (var cell in cells)
            {
                Console.WriteLine($"isActive = {cell.isActive}");
                for (int corner = 0; corner < 4; corner++)
                {
                    Console.WriteLine($"Top {corner}: {cell.topCorners[corner].X}, {cell.topCorners[corner].Y}, {cell.topCorners[corner].Z}");
                    Console.WriteLine($"Bot {corner}: {cell.bottomCorners[corner].X}, {cell.bottomCorners[corner].Y}, {cell.bottomCorners[corner].Z}");
                }
            }
        }
        public ref Cell GetCell(int i, int j, int k)
        {
            return ref cells[k, i, j];
        }
    }
}
