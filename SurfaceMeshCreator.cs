namespace Graphics
{
    public class SurfaceMeshCreator
    {
        public static Mesh Create(Surface surface)
        {
            int currQuad = 0;

            int currVertex = 0;
            int verticesPerQuad = 4;
            int coordinatesPerQuad = verticesPerQuad * 3;
            float[] vertices = new float[surface.Size * coordinatesPerQuad];

            int indicesPerQuad = 6;
            int[] indices = new int[surface.Size * indicesPerQuad];
            int currIndex = 0;
            int sideFaces;
            int sideFacesLimit = 4;

            for (int i = 0; i < surface.SizeX; i++)
            {
                for (int j = 0; j < surface.SizeY; j++)
                {
                    if (surface.GetQuad(i, j).isActive)
                    {
                        for (int corner = 0; corner < 4; corner++)
                        {
                            vertices[currVertex++] = surface.GetQuad(i, j).vertices[corner].X;
                            vertices[currVertex++] = surface.GetQuad(i, j).vertices[corner].Y;
                            vertices[currVertex++] = surface.GetQuad(i, j).vertices[corner].Z;
                        }

                        sideFaces = 0;
                        for (int m = 0; m < 2; m++)
                        {
                            indices[currIndex++] = 4 * currQuad;
                            indices[currIndex++] = ((sideFaces + 1) % sideFacesLimit) + 4 * currQuad;
                            indices[currIndex++] = ((sideFaces + 2) % sideFacesLimit) + 4 * currQuad;
                            sideFaces++;
                        }
                        currQuad++;
                    }
                }
            }
            return new Mesh(vertices, indices);
        }
    }
}