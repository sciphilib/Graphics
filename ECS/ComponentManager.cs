
namespace Graphics.ECS
{
    public class ComponentManager
    {
        private Dictionary<Type, Component> components;

        public ComponentManager()
        {
            components = new Dictionary<Type, Component>();
        }

        public void AddComponent(Component component, Entity owner)
        {
            component.SetOwner(owner);
            components[component.GetType()] = component;
        }

        public void RemoveComponent<T>() where T : Component
        {
            components.Remove(typeof(T));
        }

        public T GetComponent<T>() where T : Component
        {
            return (T)components[typeof(T)];
        }

        public bool HasComponent(Type componentType)
        {
            return components.ContainsKey(componentType);
        }

        public bool HasComponent<T>() where T : Component
        {
            return components.ContainsKey(typeof(T));
        }
    }
}