using Graphics.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class SurfaceBuilder : Builder
    {
        public string _verticesPath;
        public string _propsPath;

        public SurfaceBuilder(string verticesPath, string fragShaderPath, string vertShaderPath)
            : base(fragShaderPath, vertShaderPath)
        {
            _verticesPath = verticesPath;
        }
        public Surface Build(Vector3[] palette, int propertySet = 0)
        {
            var surface = SurfaceParser.Parse(_verticesPath);
            surface?.AddComponent(SurfacePropertiesParser.ParseHeightGradient(surface, _verticesPath));
            SurfacePropertiesLoader.Load(propertySet, surface, surface.GetComponent<Properties>());
            surface.AddComponent(
                new RenderProps(
                    _fragShaderPath,
                    _vertShaderPath,
                    ColorArrayCreator.CreateSurfaceColorArray(surface, palette),
                    OpenTK.Graphics.OpenGL.PrimitiveType.Triangles)
                );
            surface.AddComponent(new Transform());
            surface.GetComponent<Transform>()?.Translate(new OpenTK.Mathematics.Vector3(-60, 0, 40));
            surface.GetComponent<Transform>()?.RotateY(-35);
            surface.GetComponent<Transform>()?.Scale(0.00009f);
            surface.AddComponent(SurfaceMeshCreator.Create(surface));
            surface.AddComponent(new MeshRenderer());
            surface.GetComponent<MeshRenderer>().Init();
            return surface;
        }
    }
}
