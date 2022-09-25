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
        public static void GenerateColor(int verticesCount, Vector3 palette1, Vector3 palette2, out float[]? colorArray)
        {
            int arraySize = verticesCount * 12;
            colorArray = new float[arraySize];

            float property = 0;
            float deltaProperty = 10.0f / verticesCount;

            int index = 0;

            while (index < arraySize)
            {
                for (int i = 0; i < 4; i++)
                {
                    colorArray[index] = Map(property, 0.0f, 10.0f, palette1.X, palette2.X);
                    index++;
                    colorArray[index] = Map(property, 0.0f, 10.0f, palette1.Y, palette2.Y);
                    index++;
                    colorArray[index] = Map(property, 0.0f, 10.0f, palette1.Z, palette2.Z);
                    index++;
                }
                property += deltaProperty;
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

        private static float Map(float value, float min1, float max1, float min2, float max2)
        {
            return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
        }
    }
}
