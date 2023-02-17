using Graphics.ECS;
using System.Numerics;
using Graphics.Builders;
using Graphics.Entities;
using Graphics.Components;

namespace Graphics.Scenes
{
    public class MainScene : Scene
    {

        private Vector3[] palette;
        private Vector3[] palette2;

        private Vector3 paletteColor1 = new(0.211f, 0.884f, 1.000f);
        private Vector3 paletteColor2 = new(0.319f, 1.000f, 0.319f);
        private Vector3 paletteColor3 = new(0.755f, 0.394f, 0.033f);
        private Vector3 paletteColor4 = new(1.000f, 0.000f, 0.000f);

        public MainScene()
        {
            palette = new Vector3[2];
            palette[0] = paletteColor1;
            palette[1] = paletteColor4;
            palette2 = new Vector3[4];
            palette2[0] = paletteColor1;
            palette2[1] = paletteColor2;
            palette2[2] = paletteColor3;
            palette2[3] = paletteColor4;

            var gridBuilder = new GridBuilder("data\\grid.bin", "data\\grid.binprops.txt",
                "Shaders\\FragmentSurfaceShader.glsl", "Shaders\\VertexSurfaceShader.glsl");
            var outlineBuilder = new OutlineBuilder("Shaders\\FragmentOutlineShader.glsl", "Shaders\\VertexOutlineShader.glsl");

            Grid grid = gridBuilder.Build(palette2, 0);
            var gridMesh = grid.componentManager.GetComponent<Mesh>();
            grid.GetComponent<Transform>()?.Translate(new OpenTK.Mathematics.Vector3(0, 0, 0));
            grid.AddChild(outlineBuilder.BuildGridOutline(grid));

            AddObject(grid);
        }
    }
}
