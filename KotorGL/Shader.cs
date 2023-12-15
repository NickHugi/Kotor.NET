using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace KotorGL
{
    public class Shader
    {
        public int ID { get; }

        public Shader(string shaderName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (var vShaderStream = new StreamReader(assembly.GetManifestResourceStream($"KotorGL.Resources.{shaderName}.vshader")!))
            using (var fShaderStream = new StreamReader(assembly.GetManifestResourceStream($"KotorGL.Resources.{shaderName}.fshader")!))
            {
                int success;
                var vShaderSource = vShaderStream.ReadToEnd();
                var fShaderSource = fShaderStream.ReadToEnd();

                var vertexShader = GL.CreateShader(ShaderType.VertexShader);
                GL.ShaderSource(vertexShader, vShaderSource);
                GL.CompileShader(vertexShader);
                GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out success);
                if (success == 0)
                {
                    string infoLog = GL.GetShaderInfoLog(vertexShader);
                    throw new Exception(infoLog);
                }

                var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
                GL.ShaderSource(fragmentShader, fShaderSource);
                GL.CompileShader(fragmentShader);
                GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out success);
                if (success == 0)
                {
                    string infoLog = GL.GetShaderInfoLog(fragmentShader);
                    throw new Exception(infoLog);
                }

                var program = GL.CreateProgram();
                GL.AttachShader(program, vertexShader);
                GL.AttachShader(program, fragmentShader);
                GL.LinkProgram(program);
                GL.GetProgramInfoLog(program, out string info);
                if (info != "")
                {
                    string infoLog = GL.GetShaderInfoLog(vertexShader);
                    throw new Exception(infoLog);
                }

                ID = program;
            }
        }

        ~Shader()
        {
            //GL.DetachShader(program, vertexShader);
            //GL.DetachShader(program, fragmentShader);
            //GL.DeleteShader(vertexShader);
            //GL.DeleteShader(fragmentShader);
        }

        public void Use()
        {
            GL.UseProgram(ID);
        }

        public int GetUniformLocation(string name)
        {
            return GL.GetUniformLocation(ID, name);
        }
        
        public void SetUniformMatrix4x4(string name, Matrix4 matrix)
        {
            var location = GetUniformLocation(name);
            GL.UniformMatrix4(location, false, ref matrix);
        }
    }
}
