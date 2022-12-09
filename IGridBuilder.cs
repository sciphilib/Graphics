using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public interface IGridBuilder
    {
        public Grid BuildGrid(string verticesPath, string propsPath, string fragShaderPath, string vertShaderPath, double[] color);
    }
}
