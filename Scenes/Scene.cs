using Graphics.Components;
using Graphics.ECS;

namespace Graphics.Scenes
{
    public class Scene
    {
        public List<Entity> Objects { get; }
        public List<Entity> Lights { get; }

        public Scene()
        {
            Objects = new();
            Lights = new();
        }

        public void AddObject(Entity entity)
        {
            if (entity == null)
                return;

            Objects.Add(entity);

            if (entity.children != null)
                foreach (Entity child in entity.children)
                    AddObject(child);

            TransformSystem.Register(entity.GetComponent<Transform>());
            MeshRendererSystem.Register(entity.GetComponent<Mesh>());
        }

        public void AddLight(Entity entity)
        {
            Lights.Add(entity);
        }

    }
}
