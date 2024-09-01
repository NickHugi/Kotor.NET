using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Formats.BinaryMDL;
using Kotor.NET.Graphics.GPU;
using Kotor.NET.Graphics.OpenGL.GPU;
using Silk.NET.OpenGL;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Kotor.NET.Graphics.OpenGL.Factories;

public class MDLMeshFactory
{
    public unsafe IVertexArrayObject FromBinary(GL gl, byte[] vertexData, byte[] elementData, uint positionStride, uint normalStride, uint uv1Stride, uint uv2Stride, uint blockSize, uint flags)
    {
        var vertexArrayObjectID = gl.GenVertexArray();
        var vertexbufferObjectID = gl.GenBuffer();
        var elementBufferObjectID = gl.GenBuffer();

        gl.BindVertexArray(vertexArrayObjectID);

        gl.BindBuffer(BufferTargetARB.ArrayBuffer, vertexbufferObjectID);
        fixed (byte* buf = vertexData)
            gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)vertexData.Length, buf, BufferUsageARB.StaticDraw);

        gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, elementBufferObjectID);
        fixed (byte* buf = elementData)
            gl.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint)elementData.Length, buf, BufferUsageARB.StaticDraw);
        var elementCount = (uint)elementData.Count() / 2;

        var bitmask = (MDLBinaryMDXVertexBitmask)flags;
        if (MDLBinaryMDXVertexBitmask.Vertices.HasFlag(bitmask))
        {
            gl.EnableVertexAttribArray(1);
            gl.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, blockSize, (void*)positionStride);
        }

        if (MDLBinaryMDXVertexBitmask.UV1.HasFlag(bitmask))
        {
            gl.EnableVertexAttribArray(3);
            gl.VertexAttribPointer(3, 2, VertexAttribPointerType.Float, false, blockSize, (void*)uv1Stride);
        }

        if (MDLBinaryMDXVertexBitmask.UV2.HasFlag(bitmask))
        {
            gl.EnableVertexAttribArray(4);
            gl.VertexAttribPointer(4, 2, VertexAttribPointerType.Float, false, blockSize, (void*)uv2Stride);
        }

        gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
        gl.BindVertexArray(0);

        return new Mesh(gl, vertexArrayObjectID, vertexbufferObjectID, elementBufferObjectID, elementCount);
    }
}
