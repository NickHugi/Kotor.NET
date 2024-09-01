using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.GPU;

namespace Kotor.NET.Graphics;

public interface IRenderObject
{
    public IShader Shader { get; }
    public ITexture Texture { get; }
    public IVertexArrayObject VAO { get; }
    public Matrix4x4 Transformation { get; }
}
