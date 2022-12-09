using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class SceneRenderer
    {
        private Window window;
        public SceneRenderer(Window window)
        {
            this.window = window;
            window.BindRenderCallback(OnRender);
        }
        public void Render(Scene scene)
        {
            foreach(var obj in scene.objects)
            {
                
            }
        }
        public void OnRender()
        {

        }
    }
}
