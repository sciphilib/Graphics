using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class GridPropertiesParser
    {
        static public GridProperties Parse(Grid grid, string filePath)
        {
            StreamReader reader = new(filePath, Encoding.UTF8);
            string? line;

            int propertiesCount;
            double[,] properties;

            line = reader.ReadLine();
            propertiesCount = Convert.ToInt32(line, CultureInfo.InvariantCulture);
            properties = new double[propertiesCount, grid.Capacity];

            for (int i = 0; i < propertiesCount; i++)
            {
                line = reader.ReadLine();
                for (int j = 0; j < grid.Size; j++)
                {
                    line = reader.ReadLine();
                    properties[i, j] = Convert.ToSingle(line, CultureInfo.InvariantCulture);
                }
            }

            return new(propertiesCount, properties);

        }
    }
}
