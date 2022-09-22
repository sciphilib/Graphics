using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    internal class PointLight : Light
    {
        private System.Numerics.Vector3 _position;
        private float _constant;
        private float _linear;
        private float _quadratic;
        private int _index;

        public PointLight(Shader shader, int index) : base(shader)
        {
            _position = new();
            _index = index;
        }

        public override void Update()
        {
            _shader.Use();
            _shader.SetVec3($"pointLights[{_index}].position",
                new(_position.X, _position.Y, _position.Z));
            _shader.SetVec3($"pointLights[{_index}].ambient", 
                new(_ambient.X, _ambient.Y, _ambient.Z));
            _shader.SetVec3($"pointLights[{_index}].diffuse",
                new(_diffuse.X, _diffuse.Y, _diffuse.Z));
            _shader.SetVec3($"pointLights[{_index}].specular",
                new(_specular.X, _specular.Y, _specular.Z));
            _shader.SetFloat($"pointLights[{_index}].constant", _constant);
            _shader.SetFloat($"pointLights[{_index}].linear", _linear);
            _shader.SetFloat($"pointLights[{_index}].quadratic", _quadratic);
        }

        public int GetIndex()
        {
            return _index;
        }
        public void SetPosition(float a, float b, float c)
        {
            _position.X = a;
            _position.X = b;
            _position.X = c;
        }
        public ref System.Numerics.Vector3 GetPosition()
        {
            return ref _position;
        }
        public void SetConstant(float value)
        {
            this._constant = value;
        }
        public ref float GetConstant()
        {
            return ref _constant;
        }
        public void SetLinear(float value)
        {
            this._linear = value;
        }
        public ref float GetLinear()
        {
            return ref _linear;
        }
        public void SetQuadratic(float value)
        {
            this._quadratic = value;
        }
        public ref float GetQuadratic()
        {
            return ref _quadratic;
        }
    }
}
