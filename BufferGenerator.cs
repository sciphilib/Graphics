using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    static public class BufferGenerator
    {
        public static void GenerateColor(float[] surfaceVertices, int verticesCount, Vector3 palette1, Vector3 palette2, float min, float max, out float[]? colorArray)
        {
            //int arraySize = verticesCount * 12; // for surface
            int arraySize = verticesCount; // for grid
            colorArray = new float[arraySize];

            int index = 0;
            int surfaceVerticesIndex = 1;
            Console.WriteLine($"MIN {min}");
            Console.WriteLine($"MAX {max}");

            while (index < arraySize)
            {
                for (int i = 0; i < 4; i++)
                {
                    colorArray[index] = Map(surfaceVertices[surfaceVerticesIndex], min, max, palette1.X, palette2.X);
                    index++;
                    colorArray[index] = Map(surfaceVertices[surfaceVerticesIndex], min, max, palette1.Y, palette2.Y);
                    index++;
                    colorArray[index] = Map(surfaceVertices[surfaceVerticesIndex], min, max, palette1.Z, palette2.Z);
                    index++;
                    surfaceVerticesIndex += 3;
                }
            }

        }

        public static void GenerateEBOelements(int quadCount, out int[] indicesArray)
        {
            indicesArray = new int[quadCount * 6];
            indicesArray[0] = 0;
            indicesArray[1] = 1;
            indicesArray[2] = 3;
            indicesArray[3] = 1;
            indicesArray[4] = 2;
            indicesArray[5] = 3;
            int index = 6;
            while (index < quadCount * 6)
            {
                indicesArray[index] = indicesArray[index - 1] + 1;
                index++;
                indicesArray[index] = indicesArray[index - 1] + 1;
                index++;
                indicesArray[index] = indicesArray[index - 1] + 2;
                index++;
                indicesArray[index] = indicesArray[index - 2];
                index++;
                indicesArray[index] = indicesArray[index - 1] + 1;
                index++;
                indicesArray[index] = indicesArray[index - 1] + 1;
                index++;
            }
        }

        public static void GenerateGridEBO(int cellsCount, out int[] indicesArray)
        {
            indicesArray = new int[cellsCount * 36];
            int currentIndex = 0;
            int minIndex;
            for (int i = 0; i < cellsCount; i++)
            {
                minIndex = 3 * i;
                indicesArray[currentIndex++] = minIndex;
                indicesArray[currentIndex++] = minIndex + 1;
                indicesArray[currentIndex++] = minIndex + 3;
                indicesArray[currentIndex++] = minIndex;
                indicesArray[currentIndex++] = minIndex + 1;
                indicesArray[currentIndex++] = minIndex + 5;
                indicesArray[currentIndex++] = minIndex;
                indicesArray[currentIndex++] = minIndex + 3;
                indicesArray[currentIndex++] = minIndex + 7;
                indicesArray[currentIndex++] = minIndex;
                indicesArray[currentIndex++] = minIndex + 4;
                indicesArray[currentIndex++] = minIndex + 5;
                indicesArray[currentIndex++] = minIndex;
                indicesArray[currentIndex++] = minIndex + 4;
                indicesArray[currentIndex++] = minIndex + 7;
                indicesArray[currentIndex++] = minIndex + 1;
                indicesArray[currentIndex++] = minIndex + 2;
                indicesArray[currentIndex++] = minIndex + 3;
                indicesArray[currentIndex++] = minIndex + 1;
                indicesArray[currentIndex++] = minIndex + 2;
                indicesArray[currentIndex++] = minIndex + 6;
                indicesArray[currentIndex++] = minIndex + 1;
                indicesArray[currentIndex++] = minIndex + 5;
                indicesArray[currentIndex++] = minIndex + 6;
                indicesArray[currentIndex++] = minIndex + 2;
                indicesArray[currentIndex++] = minIndex + 3;
                indicesArray[currentIndex++] = minIndex + 7;
                indicesArray[currentIndex++] = minIndex + 2;
                indicesArray[currentIndex++] = minIndex + 6;
                indicesArray[currentIndex++] = minIndex + 7;
                indicesArray[currentIndex++] = minIndex + 4;
                indicesArray[currentIndex++] = minIndex + 5;
                indicesArray[currentIndex++] = minIndex + 7;
                indicesArray[currentIndex++] = minIndex + 5;
                indicesArray[currentIndex++] = minIndex + 6;
                indicesArray[currentIndex++] = minIndex + 7;
            }
        }

        public static float Map(float value, float min1, float max1, float min2, float max2)
        {   
            if (min1 == max1)
                return min2 + (value - min1) * (max2 - min2) / 1;
            else
                return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
        }
    }
}
