using Graphics.ECS;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Graphics
{
    public class Grid : Entity
    {
        public int Capacity { get; }
        public int Size { get; set; }
        public int SizeX { get; }
        public int SizeY { get; }
        public int SizeZ { get; }

        public double MaxProperty { get; set; }
        public double MinProperty { get; set; }

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
            Console.WriteLine($"Max Height = {MaxProperty}");
            Console.WriteLine($"Min Height = {MinProperty}");
            foreach (var cell in cells)
            {
                Console.WriteLine($"isActive = {cell.isActive}");
                for (int corner = 0; corner < 4; corner++)
                {
                    Console.WriteLine($"Top {corner}: {cell.topCorners[corner].X}, {cell.topCorners[corner].Y}, {cell.topCorners[corner].Z}");
                    Console.WriteLine($"Bot {corner}: {cell.bottomCorners[corner].X}, {cell.bottomCorners[corner].Y}, {cell.bottomCorners[corner].Z}");
                }
                Console.WriteLine($"Property: {cell.property}");
            }
        }
        public ref Cell GetCell(int i, int j, int k)
        {
            return ref cells[k, i, j];
        }

        public void SetVisible(bool visible)
        {
            if (visible)
            {
                foreach (var cell in cells)
                {
                    cell.isActive = true;
                }
            }
            else
            {
                foreach (var cell in cells)
                {
                    cell.isActive = false;
                }
            }
        }
    }
}
