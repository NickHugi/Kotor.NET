using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Graphics.GPU;

public interface IShader : IDisposable
{
    public uint ID { get; }

    public void Activate();

    public int GetUniformLocation(string name);

    public void SetMatrix4x4(string name, Matrix4x4 value);

    public void SetMatrix4x4Array(string name, Matrix4x4[] value);

    public void SetUniform1(string name, int value);
    public void SetUniform1(string name, uint value);

    public void SetUniform3(string name, Vector3 value);
    public void SetUniform3(string name, Color value);

    public void SetUniform4(string name, Vector4 value);
    public void SetUniform4(string name, Color value);
}
