using Graphics.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class GridBuilder : IGridBuilder
    {
        public Grid BuildGrid(string verticesPath, string propsPath, string fragShaderPath, string vertShaderPath, double[] color)
        {
            var grid = GridParser.Parse(verticesPath);
            grid.AddComponent(GridPropertiesParser.Parse(grid, propsPath));
            GridPropertiesLoader.Load(0, grid, grid.GetComponent<GridProperties>());
            grid.AddComponent(new RenderProps(vertShaderPath, fragShaderPath, color, OpenTK.Graphics.OpenGL.PrimitiveType.Triangles));
            grid.AddComponent(new Transform());
            grid.AddComponent(GridMeshCreator.Create(grid));
            grid.AddComponent(new MeshRenderer());
            return grid;
        }
    }
}
