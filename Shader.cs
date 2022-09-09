using OpenTK.Compute.OpenCL;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class Shader : IDisposable
    {
        public int Handle { get; }
        private bool disposedValue = false;

        public Shader(string vertexPath, string fragmentPath)
        {
            int VertexShader, FragmentShader;

            string VertexShaderSource;

            using (StreamReader reader = new(vertexPath, Encoding.UTF8))
            {
                VertexShaderSource = reader.ReadToEnd();
            }

            string FragmentShaderSource;

            using (StreamReader reader = new(fragmentPath, Encoding.UTF8))
            {
                FragmentShaderSource = reader.ReadToEnd();
            }

            VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, VertexShaderSource);

            FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, FragmentShaderSource);

            GL.CompileShader(VertexShader);
            GL.CompileShader(FragmentShader);


            GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int successV);
            if (successV == 0)
            {
                string infoLog = GL.GetShaderInfoLog(VertexShader);
                System.Console.WriteLine(infoLog);
            }
            GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out int successF);
            if (successF == 0)
            {
                string infoLog = GL.GetShaderInfoLog(FragmentShader);
                System.Console.WriteLine(infoLog);
            }

            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, VertexShader);
            GL.AttachShader(Handle, FragmentShader);
            GL.LinkProgram(Handle);

            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(Handle);
                Console.WriteLine(infoLog);
            }

            GL.DetachShader(Handle, VertexShader);
            GL.DetachShader(Handle, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue) return;
            if (disposing)
            {
                // free managed resources
            }
            GL.DeleteProgram(Handle);
            disposedValue = true;
        }

        ~Shader()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
