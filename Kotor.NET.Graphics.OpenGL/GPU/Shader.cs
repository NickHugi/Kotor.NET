using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.GPU;
using Silk.NET.OpenGL;

namespace Kotor.NET.Graphics.OpenGL.GPU;

public class Shader : IShader
{
    public uint ID { get; private init; }
    public uint VertexShaderID { get; private set; }
    public uint FragmentShaderID { get; private set; }

    private GL _gl;

    internal Shader(GL gl, uint programID, uint vertexShaderID, uint fragmentShaderID)
    {
        _gl = gl;

        ID = programID;
        VertexShaderID = vertexShaderID;
        FragmentShaderID = fragmentShaderID;
    }

    public void Activate()
    {
        _gl.UseProgram(ID);
    }
}
