﻿using Graphics.ImGUI;
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

        // scene variables
        private Shader _shader;
        private Shader _sunShader;
        private Shader _surfaceShader;
        private int VBO;
        private int VAO;
        private int sunVAO;
        private int surfaceEBO;
        private int surfaceVerticesVBO;
        private int surfaceColorsVBO;
        private int surfaceVAO;
        private int quadCount;
        private float minSurfaceHeight, maxSurfaceHeight;
        private bool IsMeshMode = false;
        private Vector3[] pointLightPositions;
        private DirectionLight _dirLight;
        private static int _pointLightCount = 2;
        private PointLight[] _pointLightsArray;
        private float[]? surfaceColorArray;
        private float[]? surfaceVertices;

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
        private System.Numerics.Vector3 palette2 = new(0.326f, 0.024f, 0.024f);
        private System.Numerics.Vector3 lastPalette1;
        private System.Numerics.Vector3 lastPalette2;
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
            _shader = new("Shaders\\VertexLightingShader.glsl", "Shaders\\FragmentLightingShader.glsl");
            _sunShader = new("Shaders\\VertexSunShader.glsl", "Shaders\\FragmentSunShader.glsl");
            _surfaceShader = new("Shaders\\VertexSurfaceShader.glsl", "Shaders\\FragmentSurfaceShader.glsl");

            //Parser.Parse("data\\20x20x6.txt", out surfaceVertices, out quadCount, out minSurfaceHeight, out maxSurfaceHeight);
            //Parser.Parse("data\\surface1.txt", out surfaceVertices, out quadCount, out minSurfaceHeight, out maxSurfaceHeight);
            //BufferGenerator.GenerateColor(surfaceVertices, quadCount, palette1, palette2, minSurfaceHeight, maxSurfaceHeight, out surfaceColorArray);


            //grid
            grid = GridParser.Parse("data\\grid.bin");
            GridProperties gridProperties = GridPropertiesParser.Parse(grid, "data\\grid.binprops.txt");
            GridPropertiesLoader.Load(0, grid, gridProperties);
            gridColors = ColorArrayCreator.CreateGridColorArray(grid, palette1, palette2);

            grid.AddComponent(new Transform());
            grid.AddComponent(GridMeshCreator.Create(grid));
            grid.AddComponent(new MeshRenderer("Shaders\\VertexSurfaceShader.glsl", "Shaders\\FragmentSurfaceShader.glsl", gridColors));
            var gridMesh = grid.componentManager.GetComponent<Mesh>();
            grid.GetComponent<Transform>()?.Translate(-new Vector3(gridMesh.vertices[0], gridMesh.vertices[1], gridMesh.vertices[2]));
            grid.GetComponent<Transform>()?.RotateX(180);
            grid.GetComponent<Transform>()?.Scale(0.0025f);

            //grid outline
            gridOutline = new();
            grid.AddChild(gridOutline);
            gridOutline.AddComponent(new Transform());
            gridOutline.AddComponent(new OutlineMeshCreator().CreateGridOutline(grid));
            gridOutline.AddComponent(new MeshRenderer("Shaders\\VertexOutlineShader.glsl", "Shaders\\FragmentOutlineShader.glsl"));
            gridOutline.componentManager.GetComponent<Mesh>().PrimitiveType = PrimitiveType.Lines;

            //grid slice
            gridSlice = new(grid, 0, 10, 4);
            var gridSliceColor = ColorArrayCreator.CreateGridSliceColorArray(grid, gridSlice, palette1, palette2);
            grid.AddChild(gridSlice);
            gridSlice.AddComponent(new Transform());
            gridSlice.AddComponent(new GridSliceMeshCreator(grid, gridSlice).Create());
            gridSlice.AddComponent(new MeshRenderer("Shaders\\VertexSurfaceShader.glsl", "Shaders\\FragmentSurfaceShader.glsl", gridSliceColor));
            
            //grid slice outline
            gridSliceOutline = new Outline();
            gridSlice.AddChild(gridSliceOutline);
            gridSliceOutline.AddComponent(new Transform());
            gridSliceOutline.AddComponent(new OutlineMeshCreator().CreateGridSliceOutline(gridSlice));
            gridSliceOutline.AddComponent(new MeshRenderer("Shaders\\VertexOutlineShader.glsl", "Shaders\\FragmentOutlineShader.glsl"));
            gridSliceOutline.componentManager.GetComponent<Mesh>().PrimitiveType = PrimitiveType.Lines;

            grid.GetComponent<MeshRenderer>()?.Init();
            gridOutline.GetComponent<MeshRenderer>()?.Init();
            gridSlice.GetComponent<MeshRenderer>()?.Init();
            gridSliceOutline.GetComponent<MeshRenderer>()?.Init();

            grid.Update();

            lastPalette1 = palette1;
            lastPalette2 = palette2;

            //int[]? surfaceIndices;
            //BufferGenerator.GenerateEBOelements(2, out surfaceIndices);

            IsMeshMode = false;
            GL.ClearColor(new Color4(0.5f, 0.5f, 0.5f, 1.0f));

            float[] vertices = new float[]
            {
                -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
                 0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
                 0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
                 0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
                -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
                -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,

                -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
                 0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
                 0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
                 0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
                -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
                -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,

                -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
                -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
                -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
                -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
                -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
                -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,

                 0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
                 0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
                 0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
                 0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
                 0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
                 0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,

                -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
                 0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
                 0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
                 0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
                -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
                -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,

                -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
                 0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
                 0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
                 0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
                -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
                -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f
            };

            // surface
            //surfaceVerticesVBO = GL.GenBuffer();
            //surfaceColorsVBO = GL.GenBuffer();
            //surfaceVAO = GL.GenVertexArray();
            //surfaceEBO = GL.GenBuffer();
            //// vertices vbo
            //GL.BindVertexArray(surfaceVAO);
            //GL.BindBuffer(BufferTarget.ArrayBuffer, surfaceVerticesVBO);
            //GL.BufferData(BufferTarget.ArrayBuffer, surfaceVertices.Length * sizeof(float), surfaceVertices, BufferUsageHint.StaticDraw);
            //// colors vbo
            //GL.BindBuffer(BufferTarget.ArrayBuffer, surfaceColorsVBO);
            //GL.BufferData(BufferTarget.ArrayBuffer, surfaceColorArray.Length * sizeof(float), surfaceColorArray, BufferUsageHint.DynamicDraw);
            //// ebo
            //GL.BindBuffer(BufferTarget.ElementArrayBuffer, surfaceEBO);
            //GL.BufferData(BufferTarget.ElementArrayBuffer, surfaceIndices.Length * sizeof(int), surfaceIndices, BufferUsageHint.StaticDraw);
            //// position attribute
            //GL.BindBuffer(BufferTarget.ArrayBuffer, surfaceVerticesVBO);
            //GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            //GL.EnableVertexAttribArray(0);
            //// color attribute
            //GL.BindBuffer(BufferTarget.ArrayBuffer, surfaceColorsVBO);
            //GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            //GL.EnableVertexAttribArray(1);

            // object
            VBO = GL.GenBuffer();
            //VAO = GL.GenVertexArray();
            //GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            // position attribute
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            //// normal attribute
            //GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            //GL.EnableVertexAttribArray(1);

            // sun
            sunVAO = GL.GenVertexArray();
            GL.BindVertexArray(sunVAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            pointLightPositions = new Vector3[]
            {
                new ( 0.7f,  0.2f,  -5.0f),
                new ( 2.3f, -3.3f, -4.0f),
            };

            Vector3 sunColor = Vector3.One;
            Vector3 objectColor = new(1.0f, 0.5f, 0.2f);
            _shader.Use();
            _shader.SetVec3("lightColor", sunColor);
            _shader.SetVec3("objectColor", objectColor);
            _shader.SetInt("shininess", 32);

            // direction light
            _dirLight = new(_shader);
            _dirLight.SetAmbient(0.05f, 0.05f, 0.05f);
            _dirLight.SetDiffuse(0.4f, 0.4f, 0.4f);
            _dirLight.SetSpecular(0.5f, 0.5f, 0.5f);

            _pointLightsArray = new PointLight[_pointLightCount];

            // point light 1
            _pointLightsArray[0] = new(_shader, 0);
            int index = _pointLightsArray[0].GetIndex();
            _pointLightsArray[0].SetPosition(
                pointLightPositions[index].X,
                pointLightPositions[index].Y,
                pointLightPositions[index].Z);
            _pointLightsArray[0].SetAmbient(0.05f, 0.05f, 0.05f);
            _pointLightsArray[0].SetDiffuse(0.8f, 0.8f, 0.8f);
            _pointLightsArray[0].SetSpecular(1.0f, 1.0f, 1.0f);
            _pointLightsArray[0].SetConstant(1.0f);
            _pointLightsArray[0].SetLinear(0.09f);
            _pointLightsArray[0].SetLinear(0.032f);

            // point light 2
            _pointLightsArray[1] = new(_shader, 1);
            index = _pointLightsArray[1].GetIndex();
            _pointLightsArray[1].SetPosition(
                pointLightPositions[index].X,
                pointLightPositions[index].Y,
                pointLightPositions[index].Z);
            _pointLightsArray[1].SetAmbient(0.05f, 0.05f, 0.05f);
            _pointLightsArray[1].SetDiffuse(0.8f, 0.8f, 0.8f);
            _pointLightsArray[1].SetSpecular(1.0f, 1.0f, 1.0f);
            _pointLightsArray[1].SetConstant(1.0f);
            _pointLightsArray[1].SetLinear(0.09f);
            _pointLightsArray[1].SetLinear(0.032f);
        }

        private void OnRender()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            //if (lastPalette1 != palette1 || lastPalette2 != palette2)
            //{
            //    BufferGenerator.GenerateColor(surfaceVertices, quadCount, palette1, palette2, minSurfaceHeight, maxSurfaceHeight, out surfaceColorArray);
            //    GL.BindBuffer(BufferTarget.ArrayBuffer, surfaceColorsVBO);
            //    GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)0, surfaceColorArray.Length * sizeof(float), surfaceColorArray);
            //}
            

            _shader?.Use();
            _shader?.SetVec3("dirLight.direction", -_sunPosition.X, -_sunPosition.Y, -_sunPosition.Z);
            _dirLight.Update();
            _pointLightsArray[0].Update();
            _pointLightsArray[1].Update();
            Matrix4 modelMatrix = Matrix4.Identity;
            Matrix4 viewMatrix;
            Matrix4 projectionMatrix;
            Matrix4 rotMatrix;
            Matrix4 transMatrix;

            Vector3 rotVec = new(_objectRot.X, _objectRot.Y, _objectRot.Z);
            rotMatrix = Matrix4.CreateRotationX(rotVec.X) * Matrix4.CreateRotationY(rotVec.Y) * Matrix4.CreateRotationZ(rotVec.Z);
            transMatrix = Matrix4.CreateTranslation(new Vector3(_objectPos.X, _objectPos.Y, _objectPos.Z));
            modelMatrix = rotMatrix * transMatrix;

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

            GL.LineWidth(2);
            GL.PolygonMode(MaterialFace.FrontAndBack, IsMeshMode ? PolygonMode.Line : PolygonMode.Fill);

            // object's shader settings
            //_shader?.SetMat4("model", modelMatrix);
            //_shader?.SetMat4("view", viewMatrix);
            //_shader?.SetMat4("projection", projectionMatrix);
            //_shader?.SetVec3("lightPos", _sunPosition.X, _sunPosition.Y, _sunPosition.Z);
            //_shader?.SetVec3("viewPos", _camera.CameraPosition);
            //GL.BindVertexArray(VAO);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            // surface
            //_surfaceShader?.Use();
            //// todo model matrix
            //Matrix4 surfaceModelMatrix = Matrix4.CreateTranslation(-new Vector3(surfaceVertices[0], surfaceVertices[1], surfaceVertices[2])) * Matrix4.CreateScale(0.00025f);
            //_surfaceShader?.SetMat4("model", surfaceModelMatrix);
            //_surfaceShader?.SetMat4("view", viewMatrix);
            //_surfaceShader?.SetMat4("projection", projectionMatrix);
            //GL.BindVertexArray(surfaceVAO);
            //GL.DrawElements(PrimitiveType.Triangles, 6 * quadCount, DrawElementsType.UnsignedInt, 0);

            // grid
            //GridSliceLoader newGridSlice;

            //if (isVisibleGrid)
            //    newGridSlice = new();
            //else
            //    newGridSlice = new(iSlice, jSlice, kSlice);

            //if (grid.slice != newGridSlice)
            //{
            //    if (isVisibleGrid)
            //        GridSliceLoader.MakeZeroSlice(grid, newGridSlice);
            //    else
            //        GridSliceLoader.MakeSlice(grid, newGridSlice);

            //    var newGridMesh = GridMesh.CreateGridOutline(grid);
            //    grid.GetComponent<Mesh>().vertices = newGridMesh.vertices;
            //    grid.GetComponent<Mesh>().indices = newGridMesh.indices;
            //    grid.GetComponent<MeshRenderer>().Color = GridProperties.CreateGridColorArray(grid, palette1, palette2);
            //    grid.GetComponent<MeshRenderer>().Init();

            //    var newOutlineMesh = GridOutlineMesh.CreateGridOutline(grid);
            //    gridOutline.GetComponent<Mesh>().vertices = newOutlineMesh.vertices;
            //    gridOutline.GetComponent<Mesh>().indices = newOutlineMesh.indices;
            //    gridOutline.GetComponent<MeshRenderer>().Init();
            //    Console.WriteLine("New meshes rendered");
            //}



            if (isVisibleGrid)
            {
                grid.SetVisible(true);
                Mesh mesh = GridMeshCreator.Create(grid);
                grid.GetComponent<Mesh>().vertices = mesh.vertices;
                grid.GetComponent<Mesh>().indices = mesh.indices;
                grid.GetComponent<MeshRenderer>().Init();
                var newOutlineMesh = new OutlineMeshCreator().CreateGridOutline(grid);
                gridOutline.GetComponent<Mesh>().vertices = newOutlineMesh.vertices;
                gridOutline.GetComponent<Mesh>().indices = newOutlineMesh.indices;
                gridOutline.GetComponent<MeshRenderer>().Init();
            }
            else
            {
                grid.SetVisible(false);
                Mesh mesh = GridMeshCreator.Create(grid);
                grid.GetComponent<Mesh>().vertices = mesh.vertices;
                grid.GetComponent<Mesh>().indices = mesh.indices;
                grid.GetComponent<MeshRenderer>().Init();
                var newOutlineMesh = new OutlineMeshCreator().CreateGridOutline(grid);
                gridOutline.GetComponent<Mesh>().vertices = newOutlineMesh.vertices;
                gridOutline.GetComponent<Mesh>().indices = newOutlineMesh.indices;
                gridOutline.GetComponent<MeshRenderer>().Init();
            }



            grid.GetComponent<MeshRenderer>()?.Render(viewMatrix, projectionMatrix);
            foreach (var gridChild in grid.children)
            {
                foreach (var child in gridChild.children)
                {
                    child.GetComponent<MeshRenderer>()?.Render(viewMatrix, projectionMatrix);
                }
                gridChild.GetComponent<MeshRenderer>()?.Render(viewMatrix, projectionMatrix);
            }


            //sun's shader settings
            _sunShader?.Use();
            Matrix4 SunModelMatrix = Matrix4.CreateTranslation(new Vector3(_sunPosition.X, _sunPosition.Y, _sunPosition.Z));
            _sunShader?.SetMat4("model", SunModelMatrix);
            _sunShader?.SetMat4("view", viewMatrix);
            _sunShader?.SetMat4("projection", projectionMatrix);
            GL.BindVertexArray(sunVAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            foreach (var ligthPos in _pointLightsArray)
            {
                Vector3 pos = new(ligthPos.GetPosition().X, ligthPos.GetPosition().Y, ligthPos.GetPosition().Z);
                SunModelMatrix = Matrix4.CreateScale(0.5f) * Matrix4.CreateTranslation(pos);
                _sunShader?.SetMat4("model", SunModelMatrix);
                _sunShader?.SetMat4("view", viewMatrix);
                _sunShader?.SetMat4("projection", projectionMatrix);
                GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            }
            lastPalette1 = palette1;
            lastPalette2 = palette2;
        }

        private void OnImGuiDraw()
        {
            ImGui.Text($"X: {Math.Round(_camera.CameraPosition.X)}, Y: {Math.Round(_camera.CameraPosition.Y)}, Z: {Math.Round(_camera.CameraPosition.Z)} \tCamera position");
            ImGui.SliderFloat3("Cube position", ref _objectPos, -5.0f, 5.0f);
            ImGui.SliderFloat3("Cube rotation", ref _objectRot, MathHelper.DegreesToRadians(0.0f), MathHelper.DegreesToRadians(360.0f));
            if (ImGui.CollapsingHeader("Directional light"))
            {
                ImGui.SliderFloat3("Direction", ref _sunPosition, -5.0f, 5.0f);
                ImGui.ColorEdit3("Ambient ", ref _dirLight.GetAmbient());
                ImGui.ColorEdit3("Diffuse ", ref _dirLight.GetDiffuse());
                ImGui.ColorEdit3("Specular ", ref _dirLight.GetSpecular());
            };
            if (ImGui.CollapsingHeader("Point light"))
            {
                string[] menu = new[] { "Point light 1", "Point light 2" };
                ImGui.ListBox("Source", ref currentItem, menu, _pointLightCount);
                ImGui.SliderFloat3("Position", ref _pointLightsArray[currentItem].GetPosition(), -5.0f, 5.0f);
                ImGui.ColorEdit3("Ambient", ref _pointLightsArray[currentItem].GetAmbient(), ImGuiColorEditFlags.Float);
                ImGui.ColorEdit3("Diffuse", ref _pointLightsArray[currentItem].GetDiffuse(), ImGuiColorEditFlags.Float);
                ImGui.ColorEdit3("Specular", ref _pointLightsArray[currentItem].GetSpecular(), ImGuiColorEditFlags.Float);
                ImGui.InputFloat("Constant", ref _pointLightsArray[currentItem].GetConstant(), 0.1f);
                ImGui.InputFloat("Linear", ref _pointLightsArray[currentItem].GetLinear(), 0.1f);
                ImGui.InputFloat("Quadratic", ref _pointLightsArray[currentItem].GetQuadratic(), 0.1f);
            };
            if (ImGui.CollapsingHeader("Surface colors"))
            {
                ImGui.ColorEdit3("Min color", ref palette1, ImGuiColorEditFlags.Float);
                ImGui.ColorEdit3("Max color", ref palette2, ImGuiColorEditFlags.Float);
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
            _shader?.Dispose();
            _sunShader?.Dispose();
            _surfaceShader?.Dispose();
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
