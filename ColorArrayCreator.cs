using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class ColorArrayCreator
    {
        static public double[] CreateGridColorArray(Grid grid, Vector3 palette1, Vector3 palette2, Vector3 palette3, Vector3 palette4)
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

                            array[index++] = Map(grid.GetCell(i, j, k).property, grid.MinProperty, grid.MaxProperty, (double)palette1.X, (double)palette2.X, (double)palette3.X, (double)palette4.X);
                            array[index++] = Map(grid.GetCell(i, j, k).property, grid.MinProperty, grid.MaxProperty, (double)palette1.Y, (double)palette2.Y, (double)palette3.Y, (double)palette4.Y);
                            array[index++] = Map(grid.GetCell(i, j, k).property, grid.MinProperty, grid.MaxProperty, (double)palette1.Z, (double)palette2.Z, (double)palette3.Z, (double)palette4.Z);
                        }
                    }
                }
            }
            return array;
        }

        static public double[] CreateGridSliceColorArray(Grid grid, GridSlice gridSlice, Vector3 palette1, Vector3 palette2, Vector3 palette3, Vector3 palette4)
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
                                array[index++] = Map(grid.GetCell(gridSlice.ISlice, j, k).property, grid.MinProperty, grid.MaxProperty, (double)palette1.X, (double)palette2.X, (double)palette3.X, (double)palette4.X);
                                array[index++] = Map(grid.GetCell(gridSlice.ISlice, j, k).property, grid.MinProperty, grid.MaxProperty, (double)palette1.Y, (double)palette2.Y, (double)palette3.Y, (double)palette4.Y);
                                array[index++] = Map(grid.GetCell(gridSlice.ISlice, j, k).property, grid.MinProperty, grid.MaxProperty, (double)palette1.Z, (double)palette2.Z, (double)palette3.Z, (double)palette4.Z);
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
                                array[index++] = Map(grid.GetCell(i, gridSlice.JSlice, k).property, grid.MinProperty, grid.MaxProperty, (double)palette1.X, (double)palette2.X, (double)palette3.X, (double)palette4.X);
                                array[index++] = Map(grid.GetCell(i, gridSlice.JSlice, k).property, grid.MinProperty, grid.MaxProperty, (double)palette1.Y, (double)palette2.Y, (double)palette3.Y, (double)palette4.Y);
                                array[index++] = Map(grid.GetCell(i, gridSlice.JSlice, k).property, grid.MinProperty, grid.MaxProperty, (double)palette1.Z, (double)palette2.Z, (double)palette3.Z, (double)palette4.Z);
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
                                array[index++] = Map(grid.GetCell(i, j, gridSlice.KSlice).property, grid.MinProperty, grid.MaxProperty, (double)palette1.X, (double)palette2.X, (double)palette3.X, (double)palette4.X);
                                array[index++] = Map(grid.GetCell(i, j, gridSlice.KSlice).property, grid.MinProperty, grid.MaxProperty, (double)palette1.Y, (double)palette2.Y, (double)palette3.Y, (double)palette4.Y);
                                array[index++] = Map(grid.GetCell(i, j, gridSlice.KSlice).property, grid.MinProperty, grid.MaxProperty, (double)palette1.Z, (double)palette2.Z, (double)palette3.Z, (double)palette4.Z);
                            }
                        }
                    }
                }
            }
            return array;
        }

        public static double Map(double value, double minProp, double maxProp, double color1, double color2, double color3, double color4)
        {
            if (value <= maxProp / 3)
            {
                return Lerp(value, minProp, (maxProp / 3), color1, color2);
            }
            else if (maxProp / 3 < value && value < 2 * (maxProp /3))
            {
                return Lerp(value, (maxProp / 3), 2 * (maxProp / 3), color2, color3);
            }
            else
            {
                return Lerp(value, 2 * (maxProp / 3), maxProp, color3, color4);
            }

        }

        public static double Lerp(double value, double min1, double max1, double min2, double max2)
        {
            return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
        }
    }
}
