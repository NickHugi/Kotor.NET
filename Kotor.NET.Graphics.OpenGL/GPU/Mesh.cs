using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.GPU;
using Silk.NET.OpenGL;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Kotor.NET.Graphics.OpenGL.GPU;

public class Mesh : IVertexArrayObject
{
    public uint VertexArrayID { get; private init; }
    public uint VertexBufferID { get; private init; }
    public uint ElementBufferID { get; private init; }
    public uint ElementCount { get; private init; }

    private GL _gl;

    public Mesh(GL gl, uint vertexArrayID, uint vertexBufferID, uint elementBufferID, uint elementCount)
    {
        _gl = gl;

        VertexArrayID = vertexArrayID;
        VertexBufferID = vertexBufferID;
        ElementBufferID = elementBufferID;
        ElementCount = elementCount;
    }

    public unsafe void Draw()
    {
        _gl.BindVertexArray(VertexArrayID);
        _gl.DrawElements(PrimitiveType.Triangles, ElementCount, DrawElementsType.UnsignedShort, (void*)0); // TODO - make safe?
    }
}
