
namespace Graphics
{
    abstract class Light
    {
        protected Shader _shader;

        protected System.Numerics.Vector3 _ambient;
        protected System.Numerics.Vector3 _diffuse;
        protected System.Numerics.Vector3 _specular;
        public Light(Shader shader)
        {
            _shader = shader;
            _ambient = new();
            _diffuse = new();
            _specular = new();
        }

        public abstract void Update();

        public void SetAmbient(float a, float b, float c)
        {
            _ambient.X = a;
            _ambient.Y = b;
            _ambient.Z = c;
        }
        public ref System.Numerics.Vector3 GetAmbient()
        {
            return ref _ambient;
        }
        public void SetDiffuse(float a, float b, float c)
        {
            _diffuse.X = a;
            _diffuse.Y = b;
            _diffuse.Z = c;
        }
        public ref System.Numerics.Vector3 GetDiffuse()
        {
            return ref _diffuse;
        }
        public void SetSpecular(float a, float b, float c)
        {
            _specular.X = a;
            _specular.Y = b;
            _specular.Z = c;
        }
        public ref System.Numerics.Vector3 GetSpecular()
        {
            return ref _specular;
        }

    }
}
