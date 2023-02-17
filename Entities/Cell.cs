using System.Numerics;
using Graphics.ECS;

namespace Graphics.Entities
{
    public class Cell : Entity
    {
        public Vector3[] TopCorners { get; }
        public Vector3[] BottomCorners { get; }
        public bool IsActive { get; set; }
        public double Property { get; set; }

        public Cell()
        {
            TopCorners = new Vector3[4];
            BottomCorners = new Vector3[4];
        }
    }
}
