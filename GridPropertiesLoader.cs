using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class GridPropertiesLoader
    {
        static public void Load(int propertyNumber, Grid grid, GridProperties gridProperties)
        {
            int counter = 0;
            for (int k = 0; k < grid.SizeZ; k++)
            {
                for (int j = 0; j < grid.SizeX; j++)
                {
                    for (int i = 0; i < grid.SizeY; i++)
                    {
                        grid.GetCell(j, i, k).property = gridProperties.properties[propertyNumber, counter++];
                    }
                }
            }
            //Console.WriteLine(counter);
        }
    }
}
