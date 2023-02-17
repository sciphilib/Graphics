using Graphics.Components;
using Graphics.Entities;

namespace Graphics.Creators
{
    public class GridSliceMeshCreator
    {

        public GridSliceMeshCreator(Grid grid, GridSlice gridSlice)
        {
            this.grid = grid;
            this.gridSlice = gridSlice;

            if (gridSlice.ISlice >= 0 && gridSlice.ISlice <= grid.SizeX
                && gridSlice.JSlice >= 0 && gridSlice.JSlice <= grid.SizeY
                && gridSlice.KSlice >= 0 && gridSlice.KSlice <= grid.SizeZ)
            {
                vertices = new float[gridSlice.Size * verticesPerCell];
                indices = new int[gridSlice.Size * indicesPerCell];
            }
            else
            {
                vertices = null;
                indices = null;
            }
        }


        public Mesh Create()
        {
            if (gridSlice.ISlice != 0)
            {
                MakeSliceI();
            }
            if (gridSlice.JSlice != 0)
            {
                MakeSliceJ();
            }
            if (gridSlice.KSlice != 0)
            {
                MakeSliceK();
            }
            return new Mesh(vertices, indices);
        }

        private void AddVertices(int i, int j, int k)
        {
            if (vertices != null)
            {
                for (int corner = 0; corner < 4; corner++)
                {
                    vertices[currVertex++] = grid.GetCell(i, j, k).TopCorners[corner].X;
                    vertices[currVertex++] = grid.GetCell(i, j, k).TopCorners[corner].Y;
                    vertices[currVertex++] = grid.GetCell(i, j, k).TopCorners[corner].Z;

                    vertices[currVertex++] = grid.GetCell(i, j, k).BottomCorners[corner].X;
                    vertices[currVertex++] = grid.GetCell(i, j, k).BottomCorners[corner].Y;
                    vertices[currVertex++] = grid.GetCell(i, j, k).BottomCorners[corner].Z;
                }
            }
        }

        private void AddIndices()
        {
            if (indices != null)
            {
                // side faces
                for (int m = 0; m < 8; m++)
                {
                    indices[currIndex++] = sideFaces % sideFacesLimit + 8 * currCell;
                    indices[currIndex++] = (sideFaces + 1) % sideFacesLimit + 8 * currCell;
                    indices[currIndex++] = (sideFaces + 2) % sideFacesLimit + 8 * currCell;
                    sideFaces++;
                }

                // top face
                for (int m = 0; m < 6; m++)
                {
                    indices[currIndex++] = topFaces % sideFacesLimit + 8 * currCell;
                    if (m != 2 && m != 5)
                        topFaces += 2;
                }

                // bottom face
                for (int m = 0; m < 6; m++)
                {
                    indices[currIndex++] = bottomFaces % sideFacesLimit + 8 * currCell;
                    if (m != 2 && m != 5)
                        bottomFaces += 2;
                }
                currCell++;
            }
        }

        public void MakeSliceI()
        {
            for (int i = 0; i < 1; i++)
            {
                for (int k = 0; k < grid.SizeZ; k++)
                {
                    for (int j = 0; j < grid.SizeY; j++)
                    {
                        if (grid.GetCell(gridSlice.ISlice, j, k).IsActive)
                        {
                            AddVertices(gridSlice.ISlice, j, k);
                            AddIndices();
                        }
                    }
                }
            }
        }

        public void MakeSliceJ()
        {
            for (int j = 0; j < 1; j++)
            {
                for (int k = 0; k < grid.SizeZ; k++)
                {
                    for (int i = 0; i < grid.SizeX; i++)
                    {
                        if (grid.GetCell(i, gridSlice.JSlice, k).IsActive)
                        {
                            AddVertices(i, gridSlice.JSlice, k);
                            AddIndices();
                        }
                    }
                }
            }
        }

        public void MakeSliceK()
        {
            for (int k = 0; k < 1; k++)
            {
                for (int i = 0; i < grid.SizeX; i++)
                {
                    for (int j = 0; j < grid.SizeY; j++)
                    {
                        if (grid.GetCell(i, j, gridSlice.KSlice).IsActive)
                        {
                            AddVertices(i, j, gridSlice.KSlice);
                            AddIndices();
                        }
                    }
                }
            }
        }


        private readonly Grid grid;
        private readonly GridSlice gridSlice;
        private int currCell = 0;
        private int currVertex = 0;
        private readonly int verticesPerCell = 24;
        private float[]? vertices;
        private readonly int indicesPerCell = 36;
        private int[]? indices;
        private int currIndex = 0;
        private int sideFaces = 0;
        private int topFaces = 0;
        private int bottomFaces = 1;
        private readonly int sideFacesLimit = 8;
    }
}
