using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Graphics
{
    public class GridPropertiesParser
    {
        static public Properties Parse(Grid grid, string filePath)
        {
            StreamReader reader = new(filePath, Encoding.UTF8);
            string? line;

            int propertiesCount;
            float[,] properties;

            line = reader.ReadLine();
            propertiesCount = Convert.ToInt32(line, CultureInfo.InvariantCulture);
            properties = new float[propertiesCount, grid.Capacity];

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