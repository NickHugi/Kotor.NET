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
    public Matrix4x4 ModelTransform { get; }
    public Matrix4x4 EntityTransform { get; }
    public Matrix4x4[] FinalBoneMatrices { get; }

    public RenderObject(IShader shader, ITexture texture, IVertexArrayObject vao, Matrix4x4 modelTransform, Matrix4x4 entityTransform)
    {
        Shader = shader;
        Texture = texture;
        VAO = vao;
        ModelTransform = modelTransform;
        EntityTransform = entityTransform;
        FinalBoneMatrices = Enumerable.Repeat(Matrix4x4.Identity, 16).ToArray();
    }

    public RenderObject(IShader shader, ITexture texture, IVertexArrayObject vao, Matrix4x4 modelTransform, Matrix4x4 entityTransform, Matrix4x4[] finalBoneMatrices)
    {
        Shader = shader;
        Texture = texture;
        VAO = vao;
        ModelTransform = modelTransform;
        EntityTransform = entityTransform;
        FinalBoneMatrices = finalBoneMatrices;

        if (finalBoneMatrices.Length != 16)
            throw new ArgumentException();
    }
}
