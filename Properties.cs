using Graphics.ECS;

namespace Graphics
{
    public class Properties : Component
    {
        public int propertiesCount;
        public float[,] properties;

        public Properties(int propertiesCount, float[,] properties)
        {
            this.propertiesCount = propertiesCount;
            this.properties = properties;
        }
    }
}
