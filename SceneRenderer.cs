
namespace Graphics
{
    public class SceneRenderer
    {
        public static void Render(Scene scene, CameraContext cameraContext)
        {
            foreach(var obj in scene.objects)
            {
                obj.GetComponent<MeshRenderer>().Render(cameraContext);
            }
        }
    }
}
