using Graphics.ECS;

namespace Graphics.Entities
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

        public Cell[,,] Cells { get; }

        public Grid(int sizeX, int sizeY, int sizeZ)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            SizeZ = sizeZ;
            Capacity = sizeX * sizeY * sizeZ;
            Size = Capacity;
            Cells = new Cell[sizeZ, sizeX, sizeY];
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
            foreach (var cell in Cells)
            {
                Console.WriteLine($"IsActive = {cell.IsActive}");
                for (int corner = 0; corner < 4; corner++)
                {
                    Console.WriteLine($"Top {corner}: {cell.TopCorners[corner].X}, {cell.TopCorners[corner].Y}, {cell.TopCorners[corner].Z}");
                    Console.WriteLine($"Bot {corner}: {cell.BottomCorners[corner].X}, {cell.BottomCorners[corner].Y}, {cell.BottomCorners[corner].Z}");
                }
                Console.WriteLine($"Property: {cell.Property}");
            }
        }
        public ref Cell GetCell(int i, int j, int k)
        {
            return ref Cells[k, i, j];
        }

        public void SetVisible(bool visible)
        {
            if (visible)
            {
                foreach (var cell in Cells)
                {
                    cell.IsActive = true;
                }
            }
            else
            {
                foreach (var cell in Cells)
                {
                    cell.IsActive = false;
                }
            }
        }
    }
}
