using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;
using System.Reflection;
using Graphics.ECS;

namespace Graphics
{
    public class Properties : Component
    {
        public int propertiesCount;
        public float[,] properties;

        public Properties(int propertiesCount, float[,] properties)
        {
            this.propertiesCount = propertiesCount;
            this.properties = properties;
        }
    }
}
