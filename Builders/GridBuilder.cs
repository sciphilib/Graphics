using Graphics.Components;
using Graphics.Creators;
using Graphics.ECS;
using Graphics.Entities;
using Graphics.Loaders;
using Graphics.Parsers;
using System.Numerics;

namespace Graphics.Builders
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
            grid.AddComponent(new Transform());
            grid.GetComponent<Transform>()?.RotateX(180);
            grid.GetComponent<Transform>()?.Scale(0.0025f);
            grid.AddComponent(GridMeshCreator.Create(grid));
            grid.AddComponent(new RenderProps(_fragShaderPath, _vertShaderPath, ColorArrayCreator.CreateGridColorArray(grid, palette), OpenTK.Graphics.OpenGL.PrimitiveType.Triangles, grid));
            return grid;
        }
    }
}