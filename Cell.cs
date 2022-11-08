using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Graphics.ECS;

namespace Graphics
{
    public class Cell
    {
        public Vector3[] topCorners;
        public Vector3[] bottomCorners;
        public bool isActive;
        public double property;

        public Cell()
        {
            topCorners = new Vector3[4];
            bottomCorners = new Vector3[4];
        }
    }
}
