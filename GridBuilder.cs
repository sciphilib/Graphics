using Graphics.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class GridBuilder : Builder
    {
        public string _verticesPath;
        public string _propsPath;

        public GridBuilder(string verticesPath, string propsPath, string fragShaderPath, string vertShaderPath)
            : base(fragShaderPath, vertShaderPath)
        {
            _verticesPath = verticesPath;
            _propsPath = propsPath;
        }
        public Grid Build(Vector3[] palette, int propertySet = 0)
        {
            var grid = GridParser.Parse(_verticesPath);
            grid.AddComponent(GridPropertiesParser.Parse(grid, _propsPath));
            GridPropertiesLoader.Load(propertySet, grid, grid.GetComponent<Properties>());
            grid.AddComponent(new RenderProps(_fragShaderPath, _vertShaderPath, ColorArrayCreator.CreateGridColorArray(grid,palette), OpenTK.Graphics.OpenGL.PrimitiveType.Triangles));
            grid.AddComponent(new Transform());
            grid.AddComponent(GridMeshCreator.Create(grid));
            grid.AddComponent(new MeshRenderer());
            grid.GetComponent<MeshRenderer>().Init();
            return grid;
        }
    }
}
