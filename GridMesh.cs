using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class GridMesh
    {
        public static Mesh Create(Grid grid)
        {
            int currCell = 0;

            int currVertex = 0;
            int verticesPerCell = 24;
            float[] vertices = new float[grid.Capacity * verticesPerCell];

            int indicesPerCell = 36;
            int[] indices = new int[grid.Capacity * indicesPerCell];
            int currIndex = 0;
            int sideFaces = 0;
            int topFaces = 0;
            int bottomFaces = 1;
            int sideFacesLimit = 8;
            
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

                            // side faces
                            for (int m = 0; m < 8; m++)
                            {
                                indices[currIndex++] =  (sideFaces % sideFacesLimit) + 8 * currCell;
                                indices[currIndex++] = ((sideFaces + 1) % sideFacesLimit) + 8 * currCell;
                                indices[currIndex++] = ((sideFaces + 2) % sideFacesLimit) + 8 * currCell;
                                sideFaces++;
                            }

                            // top face
                            for (int m = 0; m < 6; m++)
                            {
                                indices[currIndex++] = (topFaces % sideFacesLimit) + 8 * currCell;
                                if (m != 2 && m != 5)
                                    topFaces += 2;
                            }

                            // bottom face
                            for (int m = 0; m < 6; m++)
                            {
                                indices[currIndex++] = (bottomFaces % sideFacesLimit) + 8 * currCell;
                                if (m != 2 && m != 5)
                                    bottomFaces += 2;
                            }
                            currCell++;
                        }
                    }
                }
            }
            return new Mesh(vertices, indices);
        }
    }
}
