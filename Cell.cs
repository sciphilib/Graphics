using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Graphics
{
    public class Cell
    {
        public Vector3[] topCorners;
        public Vector3[] bottomCorners;
        public bool isActive;
        //public int[] _indices;

        //private int minIndex;

        public Cell()
        {
            topCorners = new Vector3[4];
            bottomCorners = new Vector3[4];
        }
    }
}
