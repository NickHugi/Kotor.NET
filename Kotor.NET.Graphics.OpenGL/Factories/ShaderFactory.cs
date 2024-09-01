using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.Factories;
using Kotor.NET.Graphics.GPU;
using Silk.NET.OpenGL;

namespace Kotor.NET.Graphics.OpenGL.Factories;

public class ShaderFactory(GL _gl) : IShaderFactory
{
    public IShader FromEmbeddedResource(string vertexShader, string fragmentShader)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var vertexShaderStream = assembly.GetManifestResourceStream(vertexShader)!;
        using var fragmentShaderStream = assembly.GetManifestResourceStream(fragmentShader)!;
        return FromStream(vertexShaderStream, fragmentShaderStream);
    }
    public IShader FromFile(string vertexShader, string fragmentShader)
    {
        using var vertexShaderStream = File.OpenRead(vertexShader);
        using var fragmentShaderStream = File.OpenRead(vertexShader);
        return FromStream(vertexShaderStream, fragmentShaderStream);
    }
    public IShader FromStream(Stream vertexShader, Stream fragmentShader)
    {
        var vertexShaderSource = new StreamReader(vertexShader).ReadToEnd();
        var fragmentShaderSource = new StreamReader(fragmentShader).ReadToEnd();
        return FromSource(vertexShaderSource, fragmentShaderSource);
    }
    public IShader FromSource(string vertexShader, string fragmentShader)
    {
        int success;

        var vertexShaderID = _gl.CreateShader(ShaderType.VertexShader);
        var fragmentShaderID = _gl.CreateShader(ShaderType.FragmentShader);
        var programID = _gl.CreateProgram();

        _gl.ShaderSource(vertexShaderID, vertexShader);
        _gl.CompileShader(vertexShaderID);
        _gl.GetShader(vertexShaderID, GLEnum.CompileStatus, out success);
        if (success == 0)
        {
            string infoLog = _gl.GetShaderInfoLog(vertexShaderID);
            throw new Exception(infoLog);
        }

        _gl.ShaderSource(fragmentShaderID, fragmentShader);
        _gl.CompileShader(fragmentShaderID);
        _gl.GetShader(fragmentShaderID, GLEnum.CompileStatus, out success);
        if (success == 0)
        {
            string infoLog = _gl.GetShaderInfoLog(fragmentShaderID);
            throw new Exception(infoLog);
        }

        _gl.AttachShader(programID, vertexShaderID);
        _gl.AttachShader(programID, fragmentShaderID);
        _gl.LinkProgram(programID);
        _gl.GetProgramInfoLog(programID, out string info);
        if (info != "")
        {
            string infoLog = _gl.GetShaderInfoLog(programID);
            throw new Exception(infoLog);
        }

        return new GPU.Shader(_gl, programID, vertexShaderID, fragmentShaderID);
    }
}
