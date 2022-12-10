using Graphics.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class OutlineBuilder : Builder
    {
        public OutlineBuilder(string fragShaderPath, string vertShaderPath)
            : base(fragShaderPath, vertShaderPath) 
        { }
        public Outline BuildGridOutline(Grid grid)
        {
            var outline = new Outline();
            var gridTransform = grid.GetComponent<Transform>();
            outline.AddComponent(new Transform(gridTransform.position, gridTransform.transform));
            outline.AddComponent(new OutlineMeshCreator().CreateGridOutlineMesh(grid));
            outline.AddComponent(new RenderProps(_fragShaderPath, _vertShaderPath, null, OpenTK.Graphics.OpenGL.PrimitiveType.Lines));
            outline.AddComponent(new MeshRenderer());
            outline.GetComponent<MeshRenderer>().Init();
            return outline;
        }

        public Outline BuildSliceOutline(GridSlice slice)
        {
            var outline = new Outline();
            var sliceTransform = slice.GetComponent<Transform>();
            outline.AddComponent(new Transform(sliceTransform.position, sliceTransform.transform));
            outline.AddComponent(new OutlineMeshCreator().CreateGridSliceOutlineMesh(slice));
            outline.AddComponent(new RenderProps(_fragShaderPath, _vertShaderPath, null, OpenTK.Graphics.OpenGL.PrimitiveType.Lines));
            outline.AddComponent(new MeshRenderer());
            outline.GetComponent<MeshRenderer>().Init();
            return outline;
        }
    }  
}
