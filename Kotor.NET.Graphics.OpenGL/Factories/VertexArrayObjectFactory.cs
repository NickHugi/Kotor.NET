using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Formats.BinaryMDL;
using Kotor.NET.Graphics.Factories;
using Kotor.NET.Graphics.GPU;
using Kotor.NET.Graphics.OpenGL.GPU;
using Silk.NET.Core.Contexts;
using Silk.NET.OpenGL;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Kotor.NET.Graphics.OpenGL.Factories;

public class VertexArrayObjectFactory : IVertexArrayObjectFactory
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
        if (!bitmask.Equals(0))
        {
            if (bitmask.HasFlag(MDLBinaryMDXVertexBitmask.Vertices))
            {
                gl.EnableVertexAttribArray(0);
                gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, blockSize, (void*)positionStride);
            }

            //if (bitmask.HasFlag(MDLBinaryMDXVertexBitmask.UV1))
            //{
            //    gl.EnableVertexAttribArray(3);
            //    gl.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, blockSize, (void*)uv1Stride);
            //}

            //if (bitmask.HasFlag(MDLBinaryMDXVertexBitmask.UV2))
            //{
            //    gl.EnableVertexAttribArray(4);
            //    gl.VertexAttribPointer(3, 2, VertexAttribPointerType.Float, false, blockSize, (void*)uv2Stride);
            //}
        }

        gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
        gl.BindVertexArray(0);

        return new VertexArrayObject(gl, vertexArrayObjectID, vertexbufferObjectID, elementBufferObjectID, elementCount);
    }

    public unsafe IVertexArrayObject FromXYZ(GL gl, float[] vertices, ushort[] indices)
    {
        var vao = gl.GenVertexArray();
        gl.BindVertexArray(vao);

        var vbo = gl.GenBuffer();
        gl.BindBuffer(BufferTargetARB.ArrayBuffer, vbo);
        gl.BufferData<float>(BufferTargetARB.ArrayBuffer, vertices, BufferUsageARB.StaticDraw);

        var ebo = gl.GenBuffer();
        gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, ebo);
        gl.BufferData<ushort>(BufferTargetARB.ElementArrayBuffer, indices, BufferUsageARB.StaticDraw);

        gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), (void*)0);
        gl.EnableVertexAttribArray(0);

        return new VertexArrayObject(gl, vao, vbo, ebo, 3);
    }
}
