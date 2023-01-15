
namespace Graphics.ECS
{
    public class Entity
    {
        public ComponentManager componentManager;
        public List<Entity> children;
        public Entity? parent = null;

        public Entity()
        {
            componentManager = new ComponentManager();
            children = new List<Entity>();
        }

        public void AddComponent(Component component)
        {
            componentManager.AddComponent(component, this);
        }

        public T GetComponent<T>() where T : Component
        {
            if (componentManager.HasComponent<T>())
            {
                return componentManager.GetComponent<T>();
            }
            else
            {
                return null;
            }
        }

        public void AddChild(Entity child)
        {
            children.Add(child);
            child.parent = this;
        }
        
        public void RemoveChild(Entity child)
        {
            children.Remove(child);
            child.parent = null;
        }

        public void Update()
        {
            if (parent != null)
            {
                GetComponent<Transform>().position = parent.GetComponent<Transform>().position;
                GetComponent<Transform>().transform = parent.GetComponent<Transform>().transform;
            }
            foreach (Entity child in children)
            {
                child.Update();
            }
        }
    }
}