using Graphics.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class Scene
    {
        public List<Entity> objects;
        public List<Entity> lights;

        public Scene()
        {
            objects = new();
            lights = new();
        }

        public void AddObject(Entity entity)
        {
            if (entity == null)
                return;
            
            objects.Add(entity);

            if (entity.children != null)
                foreach (Entity child in entity.children)
                    AddObject(child);
            
            TransformSystem.Register(entity.GetComponent<Transform>());
            MeshRendererSystem.Register(entity.GetComponent<MeshRenderer>());
            MeshSystem.Register(entity.GetComponent<Mesh>());
        }

        public void AddLight(Entity entity)
        {
            lights.Add(entity);
        }

    }
}
