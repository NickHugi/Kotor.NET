using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Graphics.GPU;

public class RenderObject : IRenderObject
{
    public IShader Shader { get; }
    public ITexture Texture { get; }
    public IVertexArrayObject VAO { get; }
    public Matrix4x4 Transformation { get; }

    public RenderObject(IShader shader, ITexture texture, IVertexArrayObject vao, Matrix4x4 transformation)
    {
        Shader = shader;
        Texture = texture;
        VAO = vao;
        Transformation = transformation;
    }
}
