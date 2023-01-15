using System.Numerics;

namespace Graphics
{
    public class ColorArrayCreator
    {
        static public double[] CreateSurfaceColorArray(Surface surface, params Vector3[] palette)
        {
            int verticesPerQuad = 4;
            int colorValuePerVert = 3;
            double[] array = new double[verticesPerQuad * colorValuePerVert * surface.Capacity];
            int index = 0;

            for (int i = 0; i < surface.SizeX; i++)
            {
                for (int j = 0; j < surface.SizeY; j++)
                {
                    for (int vertex = 0; vertex < verticesPerQuad; vertex++)
                    {
                        Map(surface.GetQuad(i, j).property, surface.MinProperty, surface.MaxProperty, out double[] mapValues, palette);
                        array[index++] = mapValues[0];
                        array[index++] = mapValues[1];
                        array[index++] = mapValues[2];
                    }
                }
            }
            return array;
        }
        static public double[] CreateGridColorArray(Grid grid, params Vector3[] palette)
        {
            int verticesPerCell = 8;
            int colorValuePerVert = 3;
            double[] array = new double[verticesPerCell * colorValuePerVert * grid.Size];
            int index = 0;

            for (int k = 0; k < grid.SizeZ; k++)
            {
                for (int i = 0; i < grid.SizeX; i++)
                {
                    for (int j = 0; j < grid.SizeY; j++)
                    {
                        for (int vertex = 0; vertex < verticesPerCell; vertex++)
                        {
                            Map(grid.GetCell(i, j, k).property, grid.MinProperty, grid.MaxProperty, out double[] mapValues, palette);
                            array[index++] = mapValues[0];
                            array[index++] = mapValues[1];
                            array[index++] = mapValues[2];
                        }
                    }
                }
            }
            return array;
        }

        static public double[] CreateGridSliceColorArray(Grid grid, GridSlice gridSlice, params Vector3[] palette)
        {
            int verticesPerCell = 8;
            int colorValuePerVert = 3;
            double[] array = new double[verticesPerCell * colorValuePerVert * gridSlice.Size];
            int index = 0;

            if (gridSlice.ISlice != 0)
            {
                for (int i = 0; i < 1; i++)
                {
                    for (int k = 0; k < grid.SizeZ; k++)
                    {
                        for (int j = 0; j < grid.SizeY; j++)
                        {
                            for (int vertex = 0; vertex < 8; vertex++)
                            {
                                Map(grid.GetCell(i, j, k).property, grid.MinProperty, grid.MaxProperty, out double[] mapValues, palette);
                                array[index++] = mapValues[0];
                                array[index++] = mapValues[1];
                                array[index++] = mapValues[2];
                            }
                        }
                    }
                }
            }
            if (gridSlice.JSlice != 0)
            {
                for (int j = 0; j < 1; j++)
                {
                    for (int k = 0; k < grid.SizeZ; k++)
                    {
                        for (int i = 0; i < grid.SizeX; i++)
                        {
                            for (int vertex = 0; vertex < 8; vertex++)
                            {
                                Map(grid.GetCell(i, j, k).property, grid.MinProperty, grid.MaxProperty, out double[] mapValues, palette);
                                array[index++] = mapValues[0];
                                array[index++] = mapValues[1];
                                array[index++] = mapValues[2];
                            }
                        }
                    }
                }
            }
            if (gridSlice.KSlice != 0)
            {
                for (int k = 0; k < 1; k++)
                {
                    for (int i = 0; i < grid.SizeX; i++)
                    {
                        for (int j = 0; j < grid.SizeY; j++)
                        {
                            for (int vertex = 0; vertex < 8; vertex++)
                            {
                                Map(grid.GetCell(i, j, k).property, grid.MinProperty, grid.MaxProperty, out double[] mapValues, palette);
                                array[index++] = mapValues[0];
                                array[index++] = mapValues[1];
                                array[index++] = mapValues[2];
                            }
                        }
                    }
                }
            }
            return array;
        }

        public static void Map(double value, double minProp, double maxProp, out double[] mapValues, params Vector3[] palette)
        {
            int counter = 0;
            int layers = palette.Length - 1;
            double diff = (maxProp - minProp) / layers;
            double layerMinProp = minProp;
            double layerMaxProp = minProp + diff;

            mapValues = new double[3];

            while (value > layerMaxProp)
            {
                counter++;
                layerMinProp = layerMaxProp;
                layerMaxProp += diff;
            }
            mapValues[0] = Lerp(value, layerMinProp, layerMaxProp, palette[counter].X, palette[counter + 1].X);
            mapValues[1] = Lerp(value, layerMinProp, layerMaxProp, palette[counter].Y, palette[counter + 1].Y);
            mapValues[2] = Lerp(value, layerMinProp, layerMaxProp, palette[counter].Z, palette[counter + 1].Z);
        }

        public static double Lerp(double value, double min1, double max1, double min2, double max2)
        {
            if (max1 == min1)
                return min2 + (value - min1) * (max2 - min2) / 1;
            else
                return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
        }
    }
}
