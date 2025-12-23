using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Graphics.GPU;

public interface IShader
{
    public uint ID { get; }

    public void Activate();
    int GetUniformLocation(string name);
    void SetMatrix4x4(string name, Matrix4x4 value);
    void SetUniform1(string name, double value);
}
