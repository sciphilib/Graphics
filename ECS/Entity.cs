using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.ECS
{
    public class Entity
    {
        public ComponentManager componentManager;

        public Entity()
        {
            componentManager = new ComponentManager();
        }

        public void AddComponent(Component component)
        {
            componentManager.AddComponent(component, this);
        }

        public T GetComponent<T>() where T : Component
        {
            return componentManager.GetComponent<T>();
        }
    }
}
