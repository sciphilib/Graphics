using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class OutlineMeshCreator
    {

        public Mesh CreateGridOutline(Grid grid)
        {
            vertices = grid.GetComponent<Mesh>()?.vertices;
            indices = new int[grid.Capacity * indicesPerCell];

            for (int k = 0; k < grid.SizeZ; k++)
            {
                for (int i = 0; i < grid.SizeX; i++)
                {
                    for (int j = 0; j < grid.SizeY; j++)
                    {
                        if (grid.GetCell(i, j, k).isActive)
                        {
                            AddIndices();
                        }
                    }
                }
            }
            return new Mesh(vertices, indices);
        }

        public  Mesh CreateGridSliceOutline(GridSlice gridSlice)
        {
            vertices = gridSlice.GetComponent<Mesh>()?.vertices;
            indices = new int[gridSlice.Size * indicesPerCell];

            for (int i = 0; i < 1; i++)
            {
                for (int k = 0; k < gridSlice.SizeZ; k++)
                {
                    for (int j = 0; j < gridSlice.SizeY; j++)
                    {
                        AddIndices();
                    }
                }
            }

            for (int j = 0; j < 1; j++)
            {
                for (int k = 0; k < gridSlice.SizeZ; k++)
                {
                    for (int i = 0; i < gridSlice.SizeX; i++)
                    {
                        AddIndices();
                    }
                }
            }

            for (int k = 0; k < 1; k++)
            {
                for (int i = 0; i < gridSlice.SizeX; i++)
                {
                    for (int j = 0; j < gridSlice.SizeY; j++)
                    {
                        AddIndices();
                    }
                }
            }
            return new Mesh(vertices, indices);
        }

        private void AddIndices()
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

        float[] vertices;
        int indicesPerCell = 24;
        int[] indices;
        int currIndex = 0, horizontalLine = 0, verticalLine = 0, currCell = 0;
        int sideFacesLimit = 8;
    }
}
