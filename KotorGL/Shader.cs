using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using KotorGL.Extensions;
using Silk.NET.Maths;
using Silk.NET.OpenGLES;

namespace KotorGL
{
    public class Shader
    {
        public uint ID { get; }

        private GL _gl;

        public Shader(GL gl, string shaderName)
        {
            _gl = gl;

            var assembly = Assembly.GetExecutingAssembly();

            using (var vShaderStream = new StreamReader(assembly.GetManifestResourceStream($"KotorGL.Resources.{shaderName}.vshader")!))
            using (var fShaderStream = new StreamReader(assembly.GetManifestResourceStream($"KotorGL.Resources.{shaderName}.fshader")!))
            {
                int success;
                var vShaderSource = vShaderStream.ReadToEnd();
                var fShaderSource = fShaderStream.ReadToEnd();

                var vertexShader = _gl.CreateShader(ShaderType.VertexShader);
                _gl.ShaderSource(vertexShader, vShaderSource);
                _gl.CompileShader(vertexShader);
                _gl.GetShader(vertexShader, GLEnum.CompileStatus, out success);
                if (success == 0)
                {
                    string infoLog = _gl.GetShaderInfoLog(vertexShader);
                    throw new Exception(infoLog);
                }

                var fragmentShader = _gl.CreateShader(ShaderType.FragmentShader);
                _gl.ShaderSource(fragmentShader, fShaderSource);
                _gl.CompileShader(fragmentShader);
                _gl.GetShader(fragmentShader, GLEnum.CompileStatus, out success);
                if (success == 0)
                {
                    string infoLog = _gl.GetShaderInfoLog(fragmentShader);
                    throw new Exception(infoLog);
                }

                var program = _gl.CreateProgram();
                _gl.AttachShader(program, vertexShader);
                _gl.AttachShader(program, fragmentShader);
                _gl.LinkProgram(program);
                _gl.GetProgramInfoLog(program, out string info);
                if (info != "")
                {
                    string infoLog = _gl.GetShaderInfoLog(vertexShader);
                    throw new Exception(infoLog);
                }

                ID = program;
            }
        }

        ~Shader()
        {
            //_gl.DetachShader(program, vertexShader);
            //_gl.DetachShader(program, fragmentShader);
            //_gl.DeleteShader(vertexShader);
            //_gl.DeleteShader(fragmentShader);
        }

        public void Use()
        {
            _gl.UseProgram(ID);
        }

        public int GetUniformLocation(string name)
        {
            return _gl.GetUniformLocation(ID, name);
        }
        
        public void SetUniformMatrix4x4(string name, Matrix4x4 matrix)
        {
            var location = GetUniformLocation(name);
            var value = matrix.ToFloatSpan();
            _gl.UniformMatrix4(location, false, value);
        }

        public void SetUniformMatrix4x4(string name, Matrix4X4<float> matrix)
        {
            //var location = GetUniformLocation(name);
            //var value = matrix.ToFloatSpan();
            //_gl.UniformMatrix4(location, false, matrix);
        }
    }
}
