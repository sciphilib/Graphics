namespace Graphics.Builders
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
