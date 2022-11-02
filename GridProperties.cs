using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;
using System.Reflection;

namespace Graphics
{
    public class GridProperties
    {
        public int propertiesCount;
        public double[,] properties;

        public GridProperties(int propertiesCount, double[,] properties)
        {
            this.propertiesCount = propertiesCount;
            this.properties = properties;
        }

        static public double[] CreateColorArray(int propertyNumber, Grid grid, Vector3 palette1, Vector3 palette2)
        {
            int verticesPerCell = 8;
            int colorValuePerVert = 3;
            double[] array = new double[verticesPerCell * colorValuePerVert * grid.Size];

            int index = 0;
            //for (int i = 0; i < grid.SizeX; i++)
            //{
            //    for (int j = 0; j < grid.SizeY; j++)
            //    {
            //        for (int k = 0; k < grid.SizeZ; k++)
            //        {
            //            for (int vertex = 0; vertex < 8; vertex++)
            //            {

            //                array[index++] = Map(grid.GetCell(j, k, i).property, grid.MinHeight, grid.MaxHeight, (double)palette1.X, (double)palette2.X);
            //                array[index++] = Map(grid.GetCell(j, k, i).property, grid.MinHeight, grid.MaxHeight, (double)palette1.Y, (double)palette2.Y);
            //                array[index++] = Map(grid.GetCell(j, k, i).property, grid.MinHeight, grid.MaxHeight, (double)palette1.Z, (double)palette2.Z);
            //            }
            //        }
            //    }
            //}
            for (int k = 0; k < grid.SizeZ; k++)
            {
                for (int j = 0; j < grid.SizeX; j++)
                {
                    for (int i = 0; i < grid.SizeY; i++)
                    {
                        for (int vertex = 0; vertex < 8; vertex++)
                        {

                            array[index++] = Map(grid.GetCell(j, i, k).property, grid.MinHeight, grid.MaxHeight, (double)palette1.X, (double)palette2.X);
                            array[index++] = Map(grid.GetCell(j, i, k).property, grid.MinHeight, grid.MaxHeight, (double)palette1.Y, (double)palette2.Y);
                            array[index++] = Map(grid.GetCell(j, i, k).property, grid.MinHeight, grid.MaxHeight, (double)palette1.Z, (double)palette2.Z);
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
