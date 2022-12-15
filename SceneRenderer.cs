using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class SceneRenderer
    {
        //private Window window;
        //public SceneRenderer(Window window)
        //{
        //    this.window = window;
        //    window.BindRenderCallback(OnRender);
        //}
        public static void Render(Scene scene, CameraContext cameraContext)
        {
            foreach(var obj in scene.objects)
            {
                obj.GetComponent<MeshRenderer>().Render(cameraContext);
            }
        }
        //public void OnRender()
        //{

        //}
    }
}
