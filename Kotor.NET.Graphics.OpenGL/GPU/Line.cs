using Kotor.NET.Graphics.GPU;
using Kotor.NET.Graphics.Renderers.Descriptors;
using Silk.NET.OpenGL;

namespace Kotor.NET.Graphics.OpenGL.GPU;

public class Line : ILine
{
    public uint VertexArrayObjectID { get; private init; }
    public uint QuadVertexBufferObjectID { get; private init; }
    public uint InstanceVertexBufferObjectID { get; private init; }

    private GL _gl;

    public Line(GL gl, uint vertexArrayObjectID, uint quadVertexBufferObjectID, uint instanceVertexBufferObjectID)
    {
        _gl = gl;

        VertexArrayObjectID = vertexArrayObjectID;
        QuadVertexBufferObjectID = quadVertexBufferObjectID;
        InstanceVertexBufferObjectID = instanceVertexBufferObjectID;
    }

    public unsafe void Draw(IEnumerable<LineDescriptor> lines)
    {
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, InstanceVertexBufferObjectID);
        Span<float> data = new float[lines.Count() * 11];
        for (int i = 0; i < lines.Count(); i++)
        {
            int o = i * 11;

            var l = lines.ElementAt(i);
            data[o + 0] = l.Start.X;
            data[o + 1] = l.Start.Y;
            data[o + 2] = l.Start.Z;

            data[o + 3] = l.End.X;
            data[o + 4] = l.End.Y;
            data[o + 5] = l.End.Z;

            data[o + 6] = l.Color.X;
            data[o + 7] = l.Color.Y;
            data[o + 8] = l.Color.Z;
            data[o + 9] = l.Color.W;

            data[o + 10] = l.Thickness;
        }
        _gl.BufferData(BufferTargetARB.ArrayBuffer, (ReadOnlySpan<float>)data, BufferUsageARB.DynamicDraw);

        _gl.BindVertexArray(VertexArrayObjectID);
        _gl.DrawArraysInstanced(PrimitiveType.Triangles, 0, 6, (uint)lines.Count());
    }

    public void Dispose()
    {
        _gl.DeleteVertexArray(VertexArrayObjectID);
    }
}
