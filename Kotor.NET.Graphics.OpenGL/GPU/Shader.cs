using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.GPU;
using Kotor.NET.Graphics.OpenGL.Extensions;
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

    public void Dispose()
    {
        _gl.DeleteShader(ID);
    }

    public int GetUniformLocation(string name)
    {
        return _gl.GetUniformLocation(ID, name);
    }

    public void SetMatrix4x4(string name, Matrix4x4 value)
    {
        var location = GetUniformLocation(name);
        _gl.UniformMatrix4(location, false, value.ToDoubleArray());
    }

    public void SetMatrix4x4Array(string name, Matrix4x4[] value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            var location = GetUniformLocation($"{name}[{i}]");
            _gl.UniformMatrix4(location, false, value[i].ToDoubleArray());
        }
    }

    public void SetUniform1(string name, int value)
    {
        var location = GetUniformLocation(name);
        _gl.Uniform1(location, 0);
    }
}
