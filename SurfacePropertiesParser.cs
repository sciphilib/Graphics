using System.Globalization;
using System.Text;

namespace Graphics
{
    public class SurfacePropertiesParser
    {
        static public Properties? ParseHeightGradient(Surface surface, string filePath)
        {
            int propertiesCount = 1;
            float[,] properties = new float[1, surface.Capacity];

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

            int propertyNumber = 0;

            while (!reader.EndOfStream)
            {
                float prop = 0;
                line = reader.ReadLine();
                splittedArray = line?.Split(';');
                isFirst = true;

                foreach (var obj in splittedArray)
                {
                    row = obj[0] == ' ' ? obj.Remove(0, 1) : obj;
                    elements = row.Split(' ');

                    if (isFirst)
                    {
                        isFirst = false;
                        continue;
                    }
                    else
                    {
                        // take all Y coordinates of the quad
                        prop += Convert.ToSingle(elements[1], CultureInfo.InvariantCulture);
                    }
                }
                properties[0, propertyNumber++] = prop;
            }
            return new Properties(propertiesCount, properties);
        }
    }
}
