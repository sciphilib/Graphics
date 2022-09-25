using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    static public class Parser
    {
        static public void Parse(string filePath, Vector3 palette1, Vector3 palette2, out float[]? vertices)
        {
            string? line;
            string[]? splittedArray;

            int vertexCount = 0;
            int verticesArrayIndex = 0;

            StreamReader reader = new(filePath, Encoding.UTF8);
            line = reader.ReadLine();
            if (line == null)
            {
                Console.WriteLine("Empty file.");
                vertices = null;
                return;
            }
            else
            {
                splittedArray = line?.Split(' ');
                int i = Convert.ToInt32(splittedArray[0]);
                int j = Convert.ToInt32(splittedArray[1]);
                vertexCount = i * j;
            }
            //vertices = new float[vertexCount * 12 + 400];
            vertices = new float[vertexCount * 12 * 2];

            string[] elements;
            string row;
            bool isFirst;

            float randomProperty = 0;
            float deltaFloat = 10.0f / vertexCount;

            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                splittedArray = line?.Split(';');
                isFirst = true;

                randomProperty += deltaFloat;

                foreach (var obj in splittedArray)
                {
                    if (obj[0] == ' ')
                    {
                        row = obj.Remove(0, 1);

                    }
                    else
                    {
                        row = obj;
                    }
                    elements = row.Split(' ');

                    if (isFirst)
                    {
                        if (elements[2] == "1")
                        {
                            isFirst = false;
                            //Console.WriteLine("Active");
                            continue;
                        }
                        else
                        {
                            //Console.WriteLine("Not Active");
                            break;
                        }
                    }

                    foreach (var obj1 in elements)
                    {
                        vertices[verticesArrayIndex] = Convert.ToSingle(obj1);
                        verticesArrayIndex++;
                    }
                    vertices[verticesArrayIndex] = Map(randomProperty, 0.0f, 10.0f, palette1.X, palette2.X);
                    verticesArrayIndex++;
                    vertices[verticesArrayIndex] = Map(randomProperty, 0.0f, 10.0f, palette1.Y, palette2.Y);
                    verticesArrayIndex++;
                    vertices[verticesArrayIndex] = Map(randomProperty, 0.0f, 10.0f, palette1.Z, palette2.Z);
                    verticesArrayIndex++;
                }
            }
        }
        private static float Map(float value, float min1, float max1, float min2, float max2)
        {
            return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
        }
    }
}


