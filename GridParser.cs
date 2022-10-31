using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Graphics
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
                        grid.GetCell(i, j, k).isActive = isActive;
                        for (int corner = 0; corner < 4; corner++)
                        {
                            grid.GetCell(i, j, k).topCorners[corner].X = binReader.ReadSingle();
                            grid.GetCell(i, j, k).topCorners[corner].Y = binReader.ReadSingle();
                            grid.GetCell(i, j, k).topCorners[corner].Z = binReader.ReadSingle();

                            grid.GetCell(i, j, k).bottomCorners[corner].X = binReader.ReadSingle();
                            grid.GetCell(i, j, k).bottomCorners[corner].Y = binReader.ReadSingle();
                            grid.GetCell(i, j, k).bottomCorners[corner].Z = binReader.ReadSingle();
                        }
                    }
                }
            }
            return grid;
        }
    }
}
