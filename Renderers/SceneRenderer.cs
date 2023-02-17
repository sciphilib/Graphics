using Graphics.CameraStuff;
using Graphics.ECS;
using Graphics.Scenes;

namespace Graphics.Renderers
{
    public class SceneRenderer
    {
        public static void Render(Scene scene, CameraContext cameraContext)
        {
            foreach (var obj in scene.Objects)
            {
                MeshRendererSystem.Render(cameraContext);
            }
        }
    }
}
