using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;
using System.Reflection;

namespace Graphics
{
    public class GridProperties
    {
        public int propertiesCount;
        public double[,] properties;

        public GridProperties(int propertiesCount, double[,] properties)
        {
            this.propertiesCount = propertiesCount;
            this.properties = properties;
        }

       

    }
}
