using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class GridOutlineMesh
    {
        public static Mesh Create(Grid grid)
        {
            var vertices = grid.GetComponent<Mesh>()?.vertices;
            int indicesPerCell = 24;
            int[] indices = new int[grid.Capacity * indicesPerCell];
            int currIndex = 0, horizontalLine = 0, verticalLine = 0, currCell = 0;
            int sideFacesLimit = 8;

            for (int k = 0; k < grid.SizeZ; k++)
            {
                for (int i = 0; i < grid.SizeX; i++)
                {
                    for (int j = 0; j < grid.SizeY; j++)
                    {
                        if (grid.GetCell(i, j, k).isActive)
                        {
                            // horizontal lines
                            for (int m = 0; m < 8; m++)
                            {
                                indices[currIndex++] = (horizontalLine % sideFacesLimit) + 8 * currCell;
                                indices[currIndex++] = ((horizontalLine + 2) % sideFacesLimit) + 8 * currCell;
                                horizontalLine++;
                            }

                            // vertical lines
                            for (int m = 0; m < 4; m++)
                            {
                                indices[currIndex++] = (verticalLine % sideFacesLimit) + 8 * currCell;
                                indices[currIndex++] = ((verticalLine + 1) % sideFacesLimit) + 8 * currCell;
                                verticalLine++;
                                verticalLine++;
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
