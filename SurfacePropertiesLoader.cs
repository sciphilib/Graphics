
namespace Graphics
{
    public class SurfacePropertiesLoader
    {
        static public void Load(int propertyNumber, Surface surface, Properties surfaceProperties) 
        {
            int counter = 0;
            surface.MinProperty = surface.MaxProperty = surfaceProperties.properties[propertyNumber, 0];
            for (int i = 0; i < surface.SizeX; i++)
            {
                for (int j = 0; j < surface.SizeY; j++)
                {
                    surface.GetQuad(i, j).property = surfaceProperties.properties[propertyNumber, counter++];
                    float value = surface.GetQuad(i, j).property;
                    surface.MaxProperty = value > surface.MaxProperty ? value : surface.MaxProperty;
                    surface.MinProperty = value < surface.MinProperty ? value : surface.MinProperty;
                }    
            }
        }
    }
}
