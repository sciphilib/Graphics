using Graphics.ImGUI;
using ImGuiNET;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Graphics.ECS;

namespace Graphics
{

    public class Renderer
    {
        private GameWindow _window;
        private Camera _camera;
        private bool IsMeshMode = false;

        // scene variables
        Grid grid;
        Outline gridOutline;
        GridSlice gridSlice;
        Outline gridSliceOutline;

        double[] gridColors;
        int iSlice, jSlice, kSlice;
        bool iSliceB = false, jSliceB = false, kSliceB = false, isVisibleGrid = true;

        // imgui variables
        private System.Numerics.Vector3 _objectPos = System.Numerics.Vector3.One;
        private System.Numerics.Vector3 _objectRot = System.Numerics.Vector3.Zero;
        private System.Numerics.Vector3 _sunPosition = new(0.0f, 0.0f, 3.0f);
        private System.Numerics.Vector3 palette1 = new(0.211f, 0.884f, 1.000f);
        private System.Numerics.Vector3 palette2 = new(0.319f, 1.000f, 0.319f);
        private System.Numerics.Vector3 palette3 = new(0.755f, 0.394f, 0.033f);
        private System.Numerics.Vector3 palette4 = new(1.000f, 0.000f, 0.000f);
        private System.Numerics.Vector3 lastPalette1;
        private System.Numerics.Vector3 lastPalette2;
        private System.Numerics.Vector3 lastPalette3;
        private System.Numerics.Vector3 lastPalette4;
        static int currentItem = 0;


        public Renderer(Window window)
        {
            _window = window;
            OnLoad();
            window.BindRenderCallback(OnRender);
            window.BindDrawGUICallback(OnImGuiDraw);
        }

        ~Renderer()
        {
            OnUnload();
        }
        private void OnLoad()
        {
            _camera = new(new Vector3(-3.0f, 1.0f, 5.0f), new Vector3(0.0f, 0.0f, 0.0f), (float)_window.Size.X / _window.Size.Y);


            var grid = new GridBuilder().BuildGrid("data\\grid.bin", "data\\grid.binprops.txt",
                "Shaders\\FragmentSurfaceShader.glsl", "Shaders\\VertexSurfaceShader.glsl",
                ColorArrayCreator.CreateGridColorArray(grid, palette1, palette2, palette3, palette4));

            ////grid
            //grid = GridParser.Parse("data\\grid.bin");
            //GridProperties gridProperties = GridPropertiesParser.Parse(grid, "data\\grid.binprops.txt");
            //GridPropertiesLoader.Load(0, grid, gridProperties);
            //gridColors = ColorArrayCreator.CreateGridColorArray(grid, palette1, palette2, palette3, palette4);

            //grid.AddComponent(new Transform());
            //grid.AddComponent(GridMeshCreator.Create(grid));
            //grid.AddComponent(new MeshRenderer("Shaders\\VertexSurfaceShader.glsl", "Shaders\\FragmentSurfaceShader.glsl", gridColors));
            //var gridMesh = grid.componentManager.GetComponent<Mesh>();
            //grid.GetComponent<Transform>()?.Translate(-new Vector3(gridMesh.vertices[0], gridMesh.vertices[1], gridMesh.vertices[2]));
            //grid.GetComponent<Transform>()?.RotateX(180);
            //grid.GetComponent<Transform>()?.Scale(0.0025f);

            ////grid outline
            //gridOutline = new();
            //grid.AddChild(gridOutline);
            //gridOutline.AddComponent(new Transform());
            //gridOutline.AddComponent(new OutlineMeshCreator().CreateGridOutline(grid));
            //gridOutline.AddComponent(new MeshRenderer("Shaders\\VertexOutlineShader.glsl", "Shaders\\FragmentOutlineShader.glsl"));
            //gridOutline.componentManager.GetComponent<Mesh>().PrimitiveType = PrimitiveType.Lines;

            ////grid slice
            //gridSlice = new(grid, 0, 10, 4);
            //var gridSliceColor = ColorArrayCreator.CreateGridSliceColorArray(grid, gridSlice, palette1, palette2, palette3, palette4);
            //grid.AddChild(gridSlice);
            //gridSlice.AddComponent(new Transform());
            //gridSlice.AddComponent(new GridSliceMeshCreator(grid, gridSlice).Create());
            //gridSlice.AddComponent(new MeshRenderer("Shaders\\VertexSurfaceShader.glsl", "Shaders\\FragmentSurfaceShader.glsl", gridSliceColor));
            
            ////grid slice outline
            //gridSliceOutline = new Outline();
            //gridSlice.AddChild(gridSliceOutline);
            //gridSliceOutline.AddComponent(new Transform());
            //gridSliceOutline.AddComponent(new OutlineMeshCreator().CreateGridSliceOutline(gridSlice));
            //gridSliceOutline.AddComponent(new MeshRenderer("Shaders\\VertexOutlineShader.glsl", "Shaders\\FragmentOutlineShader.glsl"));
            //gridSliceOutline.componentManager.GetComponent<Mesh>().PrimitiveType = PrimitiveType.Lines;

            //grid.GetComponent<MeshRenderer>()?.Init();
            //gridOutline.GetComponent<MeshRenderer>()?.Init();
            //gridSlice.GetComponent<MeshRenderer>()?.Init();
            //gridSliceOutline.GetComponent<MeshRenderer>()?.Init();

            //grid.Update();

            lastPalette1 = palette1;
            lastPalette2 = palette2;
            lastPalette3 = palette3;
            lastPalette4 = palette4;

            IsMeshMode = false;
            GL.ClearColor(new Color4(0.5f, 0.5f, 0.5f, 1.0f));
        }

        private void OnRender()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            //if (lastPalette1 != palette1 || lastPalette2 != palette2 || lastPalette3 != palette3 || lastPalette4 != palette4)
            //{
                
            //    gridColors = ColorArrayCreator.CreateGridColorArray(grid, palette1, palette2, palette3, palette4);
            //    grid.GetComponent<MeshRenderer>().Color = gridColors;
            //    grid.GetComponent<MeshRenderer>()?.Init();
            //}

            GL.LineWidth(2);
            GL.PolygonMode(MaterialFace.FrontAndBack, IsMeshMode ? PolygonMode.Line : PolygonMode.Fill);


            //if (isVisibleGrid)
            //{
            //    grid.SetVisible(true);
            //    Mesh mesh = GridMeshCreator.Create(grid);
            //    grid.GetComponent<Mesh>().vertices = mesh.vertices;
            //    grid.GetComponent<Mesh>().indices = mesh.indices;
            //    grid.GetComponent<MeshRenderer>().Init();
            //    var newOutlineMesh = new OutlineMeshCreator().CreateGridOutline(grid);
            //    gridOutline.GetComponent<Mesh>().vertices = newOutlineMesh.vertices;
            //    gridOutline.GetComponent<Mesh>().indices = newOutlineMesh.indices;
            //    gridOutline.GetComponent<MeshRenderer>().Init();
            //}
            //else
            //{
            //    grid.SetVisible(false);
            //    Mesh mesh = GridMeshCreator.Create(grid);
            //    grid.GetComponent<Mesh>().vertices = mesh.vertices;
            //    grid.GetComponent<Mesh>().indices = mesh.indices;
            //    grid.GetComponent<MeshRenderer>().Init();
            //    var newOutlineMesh = new OutlineMeshCreator().CreateGridOutline(grid);
            //    gridOutline.GetComponent<Mesh>().vertices = newOutlineMesh.vertices;
            //    gridOutline.GetComponent<Mesh>().indices = newOutlineMesh.indices;
            //    gridOutline.GetComponent<MeshRenderer>().Init();
            //}



            //grid.GetComponent<MeshRenderer>()?.Render(viewMatrix, projectionMatrix);
            //foreach (var gridChild in grid.children)
            //{
            //    foreach (var child in gridChild.children)
            //    {
            //        child.GetComponent<MeshRenderer>()?.Render(viewMatrix, projectionMatrix);
            //    }
            //    gridChild.GetComponent<MeshRenderer>()?.Render(viewMatrix, projectionMatrix);
            //}

            lastPalette1 = palette1;
            lastPalette2 = palette2;
            lastPalette3 = palette3;
            lastPalette4 = palette4;
        }

        private void OnImGuiDraw()
        {
            ImGui.Text($"X: {Math.Round(_camera.CameraPosition.X)}, Y: {Math.Round(_camera.CameraPosition.Y)}, Z: {Math.Round(_camera.CameraPosition.Z)} \tCamera position");
            ImGui.SliderFloat3("Cube position", ref _objectPos, -5.0f, 5.0f);
            ImGui.SliderFloat3("Cube rotation", ref _objectRot, MathHelper.DegreesToRadians(0.0f), MathHelper.DegreesToRadians(360.0f));
            if (ImGui.CollapsingHeader("Directional light"))
            {
                //ImGui.SliderFloat3("Direction", ref _sunPosition, -5.0f, 5.0f);
                //ImGui.ColorEdit3("Ambient ", ref _dirLight.GetAmbient());
                //ImGui.ColorEdit3("Diffuse ", ref _dirLight.GetDiffuse());
                //ImGui.ColorEdit3("Specular ", ref _dirLight.GetSpecular());
            };
            if (ImGui.CollapsingHeader("Point light"))
            {
                //string[] menu = new[] { "Point light 1", "Point light 2" };
                //ImGui.ListBox("Source", ref currentItem, menu, _pointLightCount);
                //ImGui.SliderFloat3("Position", ref _pointLightsArray[currentItem].GetPosition(), -5.0f, 5.0f);
                //ImGui.ColorEdit3("Ambient", ref _pointLightsArray[currentItem].GetAmbient(), ImGuiColorEditFlags.Float);
                //ImGui.ColorEdit3("Diffuse", ref _pointLightsArray[currentItem].GetDiffuse(), ImGuiColorEditFlags.Float);
                //ImGui.ColorEdit3("Specular", ref _pointLightsArray[currentItem].GetSpecular(), ImGuiColorEditFlags.Float);
                //ImGui.InputFloat("Constant", ref _pointLightsArray[currentItem].GetConstant(), 0.1f);
                //ImGui.InputFloat("Linear", ref _pointLightsArray[currentItem].GetLinear(), 0.1f);
                //ImGui.InputFloat("Quadratic", ref _pointLightsArray[currentItem].GetQuadratic(), 0.1f);
            };
            if (ImGui.CollapsingHeader("Surface colors"))
            {
                ImGui.ColorEdit3("1 color", ref palette1, ImGuiColorEditFlags.Float);
                ImGui.ColorEdit3("2 color", ref palette2, ImGuiColorEditFlags.Float);
                ImGui.ColorEdit3("3 color", ref palette3, ImGuiColorEditFlags.Float);
                ImGui.ColorEdit3("4 color", ref palette4, ImGuiColorEditFlags.Float);

            }
            if (ImGui.CollapsingHeader("Grid"))
            {
                ImGui.Checkbox("Full grid", ref isVisibleGrid);

                ImGui.Text("Grid slice");

                if (isVisibleGrid)
                    ImGui.BeginDisabled(true);

                ImGui.Checkbox("i", ref iSliceB);
                if (iSliceB)
                {
                    ImGui.SameLine();
                    ImGui.SliderInt("Slice i", ref iSlice, 0, grid.SizeX - 1);
                }

                ImGui.Checkbox("j", ref jSliceB);
                if (jSliceB)
                {
                    ImGui.SameLine();
                    ImGui.SliderInt("Slice j", ref jSlice, 0, grid.SizeY - 1);
                }

                ImGui.Checkbox("k", ref kSliceB);
                if (kSliceB)
                {
                    ImGui.SameLine();
                    ImGui.SliderInt("Slice k", ref kSlice, 0, grid.SizeZ - 1);
                }

                if (isVisibleGrid)
                    ImGui.EndDisabled();
            };
            Util.CheckGLError("End of frame");
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
        }
        private void OnUnload()
        {

        }

        public void ChangeMeshMode()
        {
            IsMeshMode = !IsMeshMode;
        }

        public Camera GetCamera()
        {
            return _camera;
        }
    }
}
