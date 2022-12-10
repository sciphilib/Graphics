using Graphics.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public abstract class Builder
    {
        public string _fragShaderPath;
        public string _vertShaderPath;

        public Builder(string fragShaderPath, string vertShaderPath)
        {
            _fragShaderPath = fragShaderPath;
            _vertShaderPath = vertShaderPath;
        }
    }
}
