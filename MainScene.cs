using Graphics.ECS;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
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
            var gridSliceBuilder = new GridSliceBuilder("Shaders\\FragmentSurfaceShader.glsl", "Shaders\\VertexSurfaceShader.glsl");

            Grid grid = gridBuilder.Build(palette2, 0);
            var gridMesh = grid.componentManager.GetComponent<Mesh>();
            grid.GetComponent<Transform>()?.Translate(-new OpenTK.Mathematics.Vector3(gridMesh.vertices[0], gridMesh.vertices[1], gridMesh.vertices[2]));
            grid.GetComponent<Transform>()?.RotateX(180);
            grid.GetComponent<Transform>()?.Scale(0.0025f);
            grid.AddChild(outlineBuilder.BuildGridOutline(grid));

            Grid grid2 = gridBuilder.Build(palette2, 1);
            grid2.GetComponent<Transform>()?.Translate(-new OpenTK.Mathematics.Vector3(4 * gridMesh.vertices[0], 4 * gridMesh.vertices[1], 4 * gridMesh.vertices[2]));
            grid2.GetComponent<Transform>()?.RotateX(180);
            grid2.GetComponent<Transform>()?.Scale(0.0025f);
            grid2.AddChild(outlineBuilder.BuildGridOutline(grid2));

            GridSlice gridSlice = gridSliceBuilder.Build(grid, palette, 1, 1, 5);
            gridSlice.GetComponent<Transform>()?.Translate(-new OpenTK.Mathematics.Vector3(4 * gridMesh.vertices[0], 4 * gridMesh.vertices[1], 4 * gridMesh.vertices[2]));
            gridSlice.GetComponent<Transform>()?.RotateX(180);
            gridSlice.GetComponent<Transform>()?.RotateY(180);
            gridSlice.GetComponent<Transform>()?.Scale(0.0025f);
            gridSlice.AddChild(outlineBuilder.BuildSliceOutline(gridSlice));

            AddObject(grid);
            AddObject(grid2);
            AddObject(gridSlice);
        }
    }
}
