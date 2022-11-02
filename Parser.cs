using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    static public class Parser
    {
        static public void Parse(string filePath, out float[]? verticesArray, out int quadCount, out float minHeight, out float maxHeight)
        {
            string? line;
            string[]? splittedArray;

            int quadsTotal = 0;
            int verticesArrayIndex = 0;
            int verticesArraySize = 0;
            minHeight = 0;
            maxHeight = 0;
            bool isFirstHeighValue = true;

            StreamReader reader = new(filePath, Encoding.UTF8);
            line = reader.ReadLine();
            if (line == null)
            {
                Console.WriteLine("Empty file.");
                verticesArray = null;
                quadCount = 0;
                return;
            }
            else
            {
                splittedArray = line?.Split(' ');
                int i = Convert.ToInt32(splittedArray[0], CultureInfo.InvariantCulture);
                int j = Convert.ToInt32(splittedArray[1], CultureInfo.InvariantCulture);
                quadsTotal = i * j;
            }

            verticesArraySize = quadsTotal * 12;
            verticesArray = new float[verticesArraySize];

            string[] elements;
            string row;
            bool isFirst;

            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                splittedArray = line?.Split(';');
                isFirst = true;

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
                            verticesArraySize -= 12;
                            break;
                        }
                    }

                    if (isFirstHeighValue)
                    {
                        minHeight = maxHeight = Convert.ToSingle(elements[1], CultureInfo.InvariantCulture);
                        isFirstHeighValue = false;
                    }

                    int coordinate = 0;
                    foreach (var obj1 in elements)
                    {
                        verticesArray[verticesArrayIndex] = Convert.ToSingle(obj1, CultureInfo.InvariantCulture);
                        if (coordinate == 1)
                        {
                            if (verticesArray[verticesArrayIndex] > maxHeight)
                            {
                                maxHeight = verticesArray[verticesArrayIndex];
                            }
                            else if (verticesArray[verticesArrayIndex] < minHeight)
                            {
                                minHeight = verticesArray[verticesArrayIndex];
                            }
                        }
                        verticesArrayIndex++;
                        coordinate++;
                    }
                }
            }
            quadCount = verticesArraySize / 12;
        }
    }
}


