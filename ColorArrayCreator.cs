using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class ColorArrayCreator
    {
        static public double[] CreateGridColorArray(Grid grid, Vector3 palette1, Vector3 palette2)
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
                        for (int vertex = 0; vertex < 8; vertex++)
                        {

                            array[index++] = Map(grid.GetCell(i, j, k).property, grid.MinProperty, grid.MaxProperty, (double)palette1.X, (double)palette2.X);
                            array[index++] = Map(grid.GetCell(i, j, k).property, grid.MinProperty, grid.MaxProperty, (double)palette1.Y, (double)palette2.Y);
                            array[index++] = Map(grid.GetCell(i, j, k).property, grid.MinProperty, grid.MaxProperty, (double)palette1.Z, (double)palette2.Z);
                        }
                    }
                }
            }
            return array;
        }

        static public double[] CreateGridSliceColorArray(Grid grid, GridSlice gridSlice, Vector3 palette1, Vector3 palette2)
        {
            int verticesPerCell = 8;
            int colorValuePerVert = 3;
            double[] array = new double[verticesPerCell * colorValuePerVert * gridSlice.Size];
            int index = 0;

            for (int i = 0; i < 1; i++)
            {
                for (int k = 0; k < grid.SizeZ; k++)
                {
                    for (int j = 0; j < grid.SizeY; j++)
                    {
                        for (int vertex = 0; vertex < 8; vertex++)
                        {
                            array[index++] = Map(grid.GetCell(gridSlice.ISlice, j, k).property, grid.MinProperty, grid.MaxProperty, (double)palette1.X, (double)palette2.X);
                            array[index++] = Map(grid.GetCell(gridSlice.ISlice, j, k).property, grid.MinProperty, grid.MaxProperty, (double)palette1.Y, (double)palette2.Y);
                            array[index++] = Map(grid.GetCell(gridSlice.ISlice, j, k).property, grid.MinProperty, grid.MaxProperty, (double)palette1.Z, (double)palette2.Z);
                        }
                    }
                }
            }
            for (int j = 0; j < 1; j++)
            {
                for (int k = 0; k < grid.SizeZ; k++)
                {
                    for (int i = 0; i < grid.SizeX; i++)
                    {
                        for (int vertex = 0; vertex < 8; vertex++)
                        {
                            array[index++] = Map(grid.GetCell(i, gridSlice.JSlice, k).property, grid.MinProperty, grid.MaxProperty, (double)palette1.X, (double)palette2.X);
                            array[index++] = Map(grid.GetCell(i, gridSlice.JSlice, k).property, grid.MinProperty, grid.MaxProperty, (double)palette1.Y, (double)palette2.Y);
                            array[index++] = Map(grid.GetCell(i, gridSlice.JSlice, k).property, grid.MinProperty, grid.MaxProperty, (double)palette1.Z, (double)palette2.Z);
                        }
                    }
                }
            }
            for (int k = 0; k < 1; k++)
            {
                for (int i = 0; i < grid.SizeX; i++)
                {
                    for (int j = 0; j < grid.SizeY; j++)
                    {
                        for (int vertex = 0; vertex < 8; vertex++)
                        {
                            array[index++] = Map(grid.GetCell(i, j, gridSlice.KSlice).property, grid.MinProperty, grid.MaxProperty, (double)palette1.X, (double)palette2.X);
                            array[index++] = Map(grid.GetCell(i, j, gridSlice.KSlice).property, grid.MinProperty, grid.MaxProperty, (double)palette1.Y, (double)palette2.Y);
                            array[index++] = Map(grid.GetCell(i, j, gridSlice.KSlice).property, grid.MinProperty, grid.MaxProperty, (double)palette1.Z, (double)palette2.Z);
                        }
                    }
                }
            }
            return array;
        }

        public static double Map(double value, double min1, double max1, double min2, double max2)
        {
            if (min1 == max1)
                return min2 + (value - min1) * (max2 - min2) / 1;
            else
                return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
        }
    }
}
