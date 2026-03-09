using System;
using System.Collections.Generic;
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
    void SetUniform1(string name, uint value);
}
