using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.ECS
{
    public abstract class BaseSystem<T> where T : Component
    {
        public static List<T> components = new();

        static public void Register(T component)
        {
            components.Add(component);
        }

    }
}
