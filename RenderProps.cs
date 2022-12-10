using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graphics.ECS;
using OpenTK.Graphics.OpenGL;


namespace Graphics
{
    public class RenderProps : Component
    {
        public string _vertexShaderPath;
        public string _fragmentShaderPath;
        public double[]? _color;
        public Shader _shader;
        public PrimitiveType _primitiveType;

        public RenderProps(string fragmentShaderPath, string vertexShaderPath, double[]? color, PrimitiveType pType)
        {
            _fragmentShaderPath = fragmentShaderPath;
            _vertexShaderPath = vertexShaderPath;
            _shader = new(vertexShaderPath, fragmentShaderPath);
            _color = color;
            _primitiveType = pType;
        }

        ~RenderProps()
        {
            _shader.Dispose();
        }
    }
}
