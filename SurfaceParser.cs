using System.Globalization;
using System.Numerics;
using System.Text;

namespace Graphics
{
    public class SurfaceParser
    {
        static public Surface? Parse(string filePath)
        {
            string? line;
            string[]? splittedArray;
            string[] elements;
            string row;

            bool isFirst;

            StreamReader reader = new(filePath, Encoding.UTF8);
            line = reader.ReadLine();
            if (line == null)
            {
                Console.WriteLine("Empty file.");
                return null;
            }
            splittedArray = line?.Split(' ');

            int sizeI = Convert.ToInt32(splittedArray[0], CultureInfo.InvariantCulture);
            int sizeJ = Convert.ToInt32(splittedArray[1], CultureInfo.InvariantCulture);
            Surface surface = new (sizeI, sizeJ);

            int vertexNumber;
            int i, j;

            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                splittedArray = line?.Split(';');
                isFirst = true;
                i = j = vertexNumber = 0;

                foreach (var obj in splittedArray)
                {
                    row = obj[0] == ' ' ? obj.Remove(0, 1) : obj;
                    elements = row.Split(' ');

                    if (isFirst)
                    {
                        i = Convert.ToInt32(elements[0], CultureInfo.InvariantCulture);
                        j = Convert.ToInt32(elements[1], CultureInfo.InvariantCulture);
                        bool isActive = Convert.ToBoolean(Convert.ToInt32(elements[2], CultureInfo.InvariantCulture));
                        surface.GetQuad(i, j) = new()
                        {
                            isActive = isActive
                        };

                        if (!isActive)
                        {
                            surface.Size--;
                        }

                        isFirst = false;
                        continue;
                    }
                    else
                    {
                        var coords = new Vector3(
                            Convert.ToSingle(elements[0], CultureInfo.InvariantCulture),
                            Convert.ToSingle(elements[1], CultureInfo.InvariantCulture),
                            Convert.ToSingle(elements[2], CultureInfo.InvariantCulture));
                        surface.GetQuad(i, j).vertices[vertexNumber++] = coords;
                    }
                }
            }
            return surface;
        }
    }
}