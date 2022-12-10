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
        Grid grid2;
        GridSlice gridSlice;

        int iSlice, jSlice, kSlice;
        bool iSliceB = false, jSliceB = false, kSliceB = false, isVisibleGrid = true;

        // imgui variables
        private System.Numerics.Vector3 _objectPos = System.Numerics.Vector3.One;
        private System.Numerics.Vector3 _objectRot = System.Numerics.Vector3.Zero;
        private System.Numerics.Vector3 _sunPosition = new(0.0f, 0.0f, 3.0f);
        private System.Numerics.Vector3[] palette;
        private System.Numerics.Vector3[] palette2;
        private System.Numerics.Vector3 paletteColor1 = new(0.211f, 0.884f, 1.000f);
        private System.Numerics.Vector3 paletteColor2 = new(0.319f, 1.000f, 0.319f);
        private System.Numerics.Vector3 paletteColor3 = new(0.755f, 0.394f, 0.033f);
        private System.Numerics.Vector3 paletteColor4 = new(1.000f, 0.000f, 0.000f);
        private System.Numerics.Vector3 lastPaletteColor1;
        private System.Numerics.Vector3 lastPaletteColor2;
        private System.Numerics.Vector3 lastPaletteColor3;
        private System.Numerics.Vector3 lastPaletteColor4;
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
            
            palette = new System.Numerics.Vector3[2];
            palette[0] = paletteColor1;
            palette[1] = paletteColor4;
            palette2 = new System.Numerics.Vector3[4];
            palette2[0] = paletteColor1;
            palette2[1] = paletteColor2;
            palette2[2] = paletteColor3;
            palette2[3] = paletteColor4;

            var gridBuilder = new GridBuilder("data\\grid.bin", "data\\grid.binprops.txt",
                "Shaders\\FragmentSurfaceShader.glsl", "Shaders\\VertexSurfaceShader.glsl");
            var outlineBuilder = new OutlineBuilder("Shaders\\FragmentOutlineShader.glsl", "Shaders\\VertexOutlineShader.glsl");
            var gridSliceBuilder = new GridSliceBuilder("Shaders\\FragmentSurfaceShader.glsl", "Shaders\\VertexSurfaceShader.glsl");

            grid = gridBuilder.Build(palette2, 0);
            var gridMesh = grid.componentManager.GetComponent<Mesh>();
            grid.GetComponent<Transform>()?.Translate(-new Vector3(gridMesh.vertices[0], gridMesh.vertices[1], gridMesh.vertices[2]));
            grid.GetComponent<Transform>()?.RotateX(180);
            grid.GetComponent<Transform>()?.Scale(0.0025f);
            grid.AddChild(outlineBuilder.BuildGridOutline(grid));

            grid2 = gridBuilder.Build(palette2, 1);
            grid2.GetComponent<Transform>()?.Translate(-new Vector3(4 * gridMesh.vertices[0], 4 * gridMesh.vertices[1], 4 * gridMesh.vertices[2]));
            grid2.GetComponent<Transform>()?.RotateX(180);
            grid2.GetComponent<Transform>()?.Scale(0.0025f);
            grid2.AddChild(outlineBuilder.BuildGridOutline(grid2));

            gridSlice = gridSliceBuilder.Build(grid, palette, 1, 1, 5);
            gridSlice.GetComponent<Transform>()?.Translate(-new Vector3(4 * gridMesh.vertices[0], 4 * gridMesh.vertices[1], 4 * gridMesh.vertices[2]));
            gridSlice.GetComponent<Transform>()?.RotateX(180);
            gridSlice.GetComponent<Transform>()?.RotateY(180);
            gridSlice.GetComponent<Transform>()?.Scale(0.0025f);
            gridSlice.AddChild(outlineBuilder.BuildSliceOutline(gridSlice));


            lastPaletteColor1 = paletteColor1;
            lastPaletteColor2 = paletteColor2;
            lastPaletteColor3 = paletteColor3;
            lastPaletteColor4 = paletteColor4;

            IsMeshMode = false;
            GL.ClearColor(new Color4(0.5f, 0.5f, 0.5f, 1.0f));
        }

        private void OnRender()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            //if (lastPalette1 != paletteColor1 || lastPalette2 != paletteColor2 || lastPalette3 != paletteColor3 || lastPalette4 != paletteColor4)
            //{
                
            //    gridColors = ColorArrayCreator.CreateGridColorArray(grid, paletteColor1, paletteColor2, paletteColor3, paletteColor4);
            //    grid.GetComponent<MeshRenderer>().Color = gridColors;
            //    grid.GetComponent<MeshRenderer>()?.Init();
            //}

            GL.LineWidth(2);
            GL.PolygonMode(MaterialFace.FrontAndBack, IsMeshMode ? PolygonMode.Line : PolygonMode.Fill);
            
            Matrix4 viewMatrix;
            Matrix4 projectionMatrix;
            if (_camera != null)
            {
                viewMatrix = _camera.GetViewMatrix();
                projectionMatrix = _camera.GetProjectionMatrix();
            }
            else
            {
                viewMatrix = Matrix4.Identity;
                projectionMatrix = Matrix4.Identity;
            }


            grid.GetComponent<MeshRenderer>().Render(viewMatrix, projectionMatrix);
            grid.children[0].GetComponent<MeshRenderer>().Render(viewMatrix, projectionMatrix);
            grid2.GetComponent<MeshRenderer>().Render(viewMatrix, projectionMatrix);
            grid2.children[0].GetComponent<MeshRenderer>().Render(viewMatrix, projectionMatrix);
            gridSlice.GetComponent<MeshRenderer>().Render(viewMatrix, projectionMatrix);
            gridSlice.children[0].GetComponent<MeshRenderer>().Render(viewMatrix, projectionMatrix);


            //if (isVisibleGrid)
            //{
            //    grid.SetVisible(true);
            //    Mesh mesh = GridMeshCreator.Create(grid);
            //    grid.GetComponent<Mesh>().vertices = mesh.vertices;
            //    grid.GetComponent<Mesh>().indices = mesh.indices;
            //    grid.GetComponent<MeshRenderer>().Init();
            //    var newOutlineMesh = new OutlineMeshCreator().CreateGridOutlineMesh(grid);
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
            //    var newOutlineMesh = new OutlineMeshCreator().CreateGridOutlineMesh(grid);
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

            lastPaletteColor1 = paletteColor1;
            lastPaletteColor2 = paletteColor2;
            lastPaletteColor3 = paletteColor3;
            lastPaletteColor4 = paletteColor4;
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
                ImGui.ColorEdit3("1 color", ref paletteColor1, ImGuiColorEditFlags.Float);
                ImGui.ColorEdit3("2 color", ref paletteColor2, ImGuiColorEditFlags.Float);
                ImGui.ColorEdit3("3 color", ref paletteColor3, ImGuiColorEditFlags.Float);
                ImGui.ColorEdit3("4 color", ref paletteColor4, ImGuiColorEditFlags.Float);

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
