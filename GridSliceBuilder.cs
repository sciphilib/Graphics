using Graphics.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class GridSliceBuilder : Builder
    {
        public GridSliceBuilder(string fragShaderPath, string vertShaderPath)
            : base(fragShaderPath, vertShaderPath)
        { }
        public GridSlice Build(Grid grid, Vector3[] palette, int i, int j, int k)
        {
            var slice = new GridSlice(grid, i, j, k);
            slice.AddComponent(new RenderProps(_fragShaderPath, _vertShaderPath, ColorArrayCreator.CreateGridSliceColorArray(grid, slice, palette), OpenTK.Graphics.OpenGL.PrimitiveType.Triangles));
            slice.AddComponent(new Transform());
            slice.AddComponent(new GridSliceMeshCreator(grid, slice).Create());
            slice.AddComponent(new MeshRenderer());
            slice.GetComponent<MeshRenderer>().Init();
            return slice;
        }
    }
}
