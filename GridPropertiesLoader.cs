using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class GridPropertiesLoader
    {
        static public void Load(int propertyNumber, Grid grid, Properties gridProperties)
        {
            int counter = 0;
            grid.MinProperty = grid.MaxProperty = gridProperties.properties[propertyNumber, 0];
            for (int i = 0; i < grid.SizeX; i++)
            {
                for (int j = 0; j < grid.SizeY; j++)
                {
                    for (int k = 0; k < grid.SizeZ; k++)
                    {
                        grid.GetCell(i, j, k).property = gridProperties.properties[propertyNumber, counter++];
                        var value = grid.GetCell(i, j, k).property;
                        grid.MaxProperty = value > grid.MaxProperty ? value : grid.MaxProperty;
                        grid.MinProperty = value < grid.MinProperty ? value : grid.MinProperty;
                    }
                }
            }
        }
    }
}
