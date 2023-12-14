using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace KotorGL
{
    public class Shader
    {
        public int ID { get; }

        public Shader(string shaderName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (var vShaderStream = new StreamReader(assembly.GetManifestResourceStream("KotorGL.Resources.kotor.vshader")!))
            using (var fShaderStream = new StreamReader(assembly.GetManifestResourceStream("KotorGL.Resources.kotor.fshader")!))
            {
                var vShaderSource = vShaderStream.ReadToEnd();
                var fShaderSource = fShaderStream.ReadToEnd();

                var vertexShader = GL.CreateShader(ShaderType.VertexShader);
                GL.ShaderSource(vertexShader, vShaderSource);
                GL.CompileShader(vertexShader);

                var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
                GL.ShaderSource(fragmentShader, fShaderSource);
                GL.CompileShader(fragmentShader);

                var program = GL.CreateProgram();
                GL.AttachShader(program, vertexShader);
                GL.AttachShader(program, fragmentShader);
                GL.LinkProgram(program);

                GL.DetachShader(program, vertexShader);
                GL.DetachShader(program, fragmentShader);
                GL.DeleteShader(vertexShader);
                GL.DeleteShader(fragmentShader);

                ID = program;
            }
        }
    }
}
