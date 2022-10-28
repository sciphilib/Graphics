using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class MeshLoader
    {
        public static void CreateGridMesh(Grid grid, out Mesh mesh)
        {
            int currVertex = 0;

            int verticesPerCell = 24;
            float[] vertices = new float[grid.Capacity * verticesPerCell];

            int indicesPerCell = 36;
            int[] indices = new int[grid.Capacity * indicesPerCell];
            
            for (int k = 0; k < grid.SizeZ; k++)
            {
                for (int i = 0; i < grid.SizeX; i++)
                {
                    for (int j = 0; j < grid.SizeY; j++)
                    {
                        if (grid.GetCell(i, j, k).isActive)
                        {
                            for (int corner = 0; corner < 4; corner++)
                            {
                                vertices[currVertex++] = grid.GetCell(i, j, k).topCorners[corner].X;
                                vertices[currVertex++] = grid.GetCell(i, j, k).topCorners[corner].Y;
                                vertices[currVertex++] = grid.GetCell(i, j, k).topCorners[corner].Z;

                                vertices[currVertex++] = grid.GetCell(i, j, k).bottomCorners[corner].X;
                                vertices[currVertex++] = grid.GetCell(i, j, k).bottomCorners[corner].Y;
                                vertices[currVertex++] = grid.GetCell(i, j, k).bottomCorners[corner].Z;
                            }
                        }
                    }
                }
            }

            mesh = new Mesh(vertices, indices);
        }
    }
}
