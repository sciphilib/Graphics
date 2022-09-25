using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    static public class Parser
    {
        static public void Parse(string filePath, out float[]? vertices)
        {
            string? line;
            string[]? splittedArray;
            var rand = new Random();

            int vertexCount = 0;
            int verticesArrayIndex = 0;
            //float[] vertices = new float[]

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
            vertices = new float[vertexCount * 12 + 400];
            //vertices = new float[vertexCount * 12 + 400 * 4];

            string[] elements;
            string row;
            bool isFirst;
            
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                splittedArray = line?.Split(';');
                isFirst = true;

                //float randomProperty = rand.NextSingle() * 10;

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

                    // elements -- [0 0 1] ...
                    //Console.WriteLine(elements[0]);
                    //Console.WriteLine(elements[1]);
                    //Console.WriteLine(elements[2]);

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
                    //vertices[verticesArrayIndex] = randomProperty;
                    //verticesArrayIndex++;
                }
            }
        }
    }
}
