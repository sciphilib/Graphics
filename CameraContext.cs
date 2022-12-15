using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class CameraContext
    {
        public Camera _camera;
        public Matrix4 projectionMatrix;
        public Matrix4 viewMatrix;

        public CameraContext(Camera camera)
        {
            _camera = camera;
        }
        public void Update()
        {
            projectionMatrix = _camera.GetProjectionMatrix();
            viewMatrix = _camera.GetViewMatrix();
        }
    }
}
