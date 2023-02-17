using System.Text;
using Graphics.Entities;

namespace Graphics.Parsers
{
    public class GridParser
    {

        static public Grid Parse(string filePath)
        {
            BinaryReader binReader = new(File.Open(filePath, FileMode.Open), Encoding.UTF8);
            int sizeX, sizeY, sizeZ;
            bool isActive;

            sizeX = binReader.ReadInt32();
            sizeY = binReader.ReadInt32();
            sizeZ = binReader.ReadInt32();
            Grid grid = new(sizeX, sizeY, sizeZ);

            for (int k = 0; k < sizeZ; k++)
            {
                for (int i = 0; i < sizeX; i++)
                {
                    for (int j = 0; j < sizeY; j++)
                    {
                        grid.GetCell(i, j, k) = new Cell();

                        isActive = binReader.ReadBoolean();
                        if (!isActive)
                            grid.Size--;
                        grid.GetCell(i, j, k).IsActive = isActive;

                        for (int corner = 0; corner < 4; corner++)
                        {
                            grid.GetCell(i, j, k).TopCorners[corner].X = binReader.ReadSingle();
                            grid.GetCell(i, j, k).TopCorners[corner].Y = binReader.ReadSingle();
                            grid.GetCell(i, j, k).TopCorners[corner].Z = binReader.ReadSingle();

                            grid.GetCell(i, j, k).BottomCorners[corner].X = binReader.ReadSingle();
                            grid.GetCell(i, j, k).BottomCorners[corner].Y = binReader.ReadSingle();
                            grid.GetCell(i, j, k).BottomCorners[corner].Z = binReader.ReadSingle();
                        }
                    }
                }
            }
            return grid;
        }
    }
}
