using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.GPU;
using Silk.NET.OpenGL;

namespace Kotor.NET.Graphics.OpenGL.GPU;

public class Texture : ITexture
{
    public uint ID { get; private init; }

    private GL _gl;

    internal Texture(GL gl, uint id)
    {
        _gl = gl;

        ID = id;
    }

    public void Activate(int slot = 0)
    {
        _gl.ActiveTexture(GLEnum.Texture0 + slot);
        _gl.BindTexture(TextureTarget.Texture2D, ID);
    }

    public void Dispose()
    {
        _gl.DeleteTexture(ID);
    }
}
