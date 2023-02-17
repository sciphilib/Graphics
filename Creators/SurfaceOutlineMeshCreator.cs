using Graphics.Components;
using Graphics.Entities;

namespace Graphics.Creators
{
    public class SurfaceOutlineMeshCreator
    {
        public static Mesh CreateSurfaceOutlineMesh(Surface surface)
        {
            float[] vertices;
            int[] indices;

            int indicesPerQuad = 8;
            int currIndex = 0, currQuad = 0;
            int horizontalLine = 0, verticalLine = 0;

            vertices = surface.GetComponent<Mesh>()?.vertices;
            indices = new int[surface.Size * indicesPerQuad];

            for (int i = 0; i < surface.SizeX; i++)
            {
                for (int j = 0; j < surface.SizeY; j++)
                {
                    if (surface.GetQuad(i, j).isActive)
                    {
                        // horizontal lines
                        for (int m = 0; m < 2; m++)
                        {
                            indices[currIndex++] = horizontalLine;
                            indices[currIndex++] = horizontalLine + 1;
                            horizontalLine++;
                            horizontalLine++;
                        }

                        //vertical lines
                        for (int m = 0; m < 2; m++)
                        {
                            indices[currIndex++] = verticalLine + 2 * currQuad;
                            indices[currIndex++] = verticalLine + 3 - m * 2 + 2 * currQuad;
                            verticalLine++;
                        }
                        currQuad++;
                    }
                }
            }
            return new Mesh(vertices, indices);
        }
    }
}