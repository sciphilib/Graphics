
namespace Graphics
{
    internal class DirectionLight : Light
    {
        public DirectionLight(Shader _shader) : base(_shader)
        {

        }

        public override void Update()
        {
            _shader.Use();
            _shader.SetVec3("dirLight.ambient", new(GetAmbient().X, GetAmbient().Y, GetAmbient().Z));
            _shader.SetVec3("dirLight.diffuse", new(GetDiffuse().X, GetDiffuse().Y, GetDiffuse().Z));
            _shader.SetVec3("dirLight.specular", new(GetSpecular().X, GetSpecular().Y, GetSpecular().Z));
        }
    }
}
