using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
    public unsafe IMesh FromBinary(GL gl, byte[] vertexData, byte[] elementData, uint positionStride, uint normalStride, uint uv1Stride, uint uv2Stride, uint blockSize, uint flags)
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
            if (bitmask.HasFlag(MDLBinaryMDXVertexBitmask.Position))
            {
                gl.EnableVertexAttribArray(0);
                gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, blockSize, (void*)positionStride);
            }

            if (bitmask.HasFlag(MDLBinaryMDXVertexBitmask.UV1))
            {
                gl.EnableVertexAttribArray(2);
                gl.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, blockSize, (void*)uv1Stride);
            }

            if (bitmask.HasFlag(MDLBinaryMDXVertexBitmask.UV2))
            {
                gl.EnableVertexAttribArray(3);
                gl.VertexAttribPointer(3, 2, VertexAttribPointerType.Float, false, blockSize, (void*)uv2Stride);
            }
        }

        gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
        gl.BindVertexArray(0);

        return new VertexArrayObject(gl, vertexArrayObjectID, vertexbufferObjectID, elementBufferObjectID, elementCount);
    }
    public unsafe IMesh SkinFromBinary(
        GL gl,
        byte[] vertexData,
        byte[] elementData,
        uint positionStride,
        uint normalStride,
        uint uv1Stride,
        uint uv2Stride,
        uint blockSize,
        uint flags,
        uint weightValueStride,
        uint weightIndexStride)
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
            if (bitmask.HasFlag(MDLBinaryMDXVertexBitmask.Position))
            {
                gl.EnableVertexAttribArray(0);
                gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, blockSize, (void*)positionStride);
            }

            if (bitmask.HasFlag(MDLBinaryMDXVertexBitmask.UV1))
            {
                gl.EnableVertexAttribArray(2);
                gl.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, blockSize, (void*)uv1Stride);
            }

            if (bitmask.HasFlag(MDLBinaryMDXVertexBitmask.UV2))
            {
                gl.EnableVertexAttribArray(3);
                gl.VertexAttribPointer(3, 2, VertexAttribPointerType.Float, false, blockSize, (void*)uv2Stride);
            }

            gl.EnableVertexAttribArray(4);
            gl.VertexAttribPointer(4, 4, VertexAttribPointerType.Float, false, blockSize, (void*)weightIndexStride);

            gl.EnableVertexAttribArray(5);
            gl.VertexAttribPointer(5, 4, VertexAttribPointerType.Float, false, blockSize, (void*)weightValueStride);
        }

        gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
        gl.BindVertexArray(0);

        return new VertexArrayObject(gl, vertexArrayObjectID, vertexbufferObjectID, elementBufferObjectID, elementCount);
    }

    public unsafe IMesh New(GL gl, Vector3[] position, ushort[] indices, Vector2[] uvs)
    {
        uint stride = 5 * sizeof(float);
        var data = new float[position.Length * 5];

        for (int i = 0; i < position.Length; i++)
        {
            var offset = i * 5;

            data[offset + 0] = position[i].X;
            data[offset + 1] = position[i].Y;
            data[offset + 2] = position[i].Z;

            data[offset + 3] = uvs[i].X;
            data[offset + 4] = uvs[i].Y;
        }

        var vao = gl.GenVertexArray();
        gl.BindVertexArray(vao);

        var vbo = gl.GenBuffer();
        gl.BindBuffer(BufferTargetARB.ArrayBuffer, vbo);
        gl.BufferData<float>(BufferTargetARB.ArrayBuffer, data, BufferUsageARB.StaticDraw);

        var ebo = gl.GenBuffer();
        gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, ebo);
        gl.BufferData<ushort>(BufferTargetARB.ElementArrayBuffer, indices, BufferUsageARB.StaticDraw);

        gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, (void*)0);
        gl.EnableVertexAttribArray(0);

        gl.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, stride, (void*)(sizeof(float) * 3));
        gl.EnableVertexAttribArray(2);

        return new VertexArrayObject(gl, vao, vbo, ebo, (uint)indices.Length);
    }

    public unsafe IMesh NewQuad(GL gl)
    {
        return New(gl,
        [
            new Vector3(0.0f, 0.0f, 0.0f),
            new Vector3(1.0f, 0.0f, 0.0f),
            new Vector3(1.0f, 1.0f, 0.0f),
            new Vector3(0.0f, 1.0f, 0.0f)
        ],
        [
            0, 1, 2,
            2, 3, 0
        ],
        [
            new Vector2(0.0f, 0.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(0.0f, 1.0f)
        ]);
    }

    public unsafe IMesh NewBillBoard(GL gl)
    {
        float[] vertices =
        {
    -0.5f, -0.5f, // 0
     0.5f, -0.5f, // 1
     0.5f,  0.5f, // 2
    -0.5f,  0.5f  // 3
        };

        ushort[] elements =
        [
            0, 1, 2,
            2, 3, 0
        ];

        var vao = gl.GenVertexArray();
        gl.BindVertexArray(vao);

        var vbo = gl.GenBuffer();
        gl.BindBuffer(BufferTargetARB.ArrayBuffer, vbo);
        gl.BufferData<float>(BufferTargetARB.ArrayBuffer, vertices, BufferUsageARB.StaticDraw);

        var ebo = gl.GenBuffer();
        gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, ebo);
        gl.BufferData<ushort>(BufferTargetARB.ElementArrayBuffer, elements, BufferUsageARB.StaticDraw);

        gl.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
        gl.EnableVertexAttribArray(0);

        return new VertexArrayObject(gl, vao, vbo, ebo, (uint)elements.Length);
    }

    private static ILine? _lineQuad;
    public unsafe ILine GetLineQuad(GL gl)
    {
        if (_lineQuad is null)
        {
            float[] vertices =
            {
                0f, -1f,
                1f, -1f,
                0f,  1f,

                0f,  1f,
                1f, -1f,
                1f,  1f
            };

            var vao = gl.GenVertexArray();
            gl.BindVertexArray(vao);

            uint quadVbo = gl.GenBuffer();
            gl.BindBuffer(BufferTargetARB.ArrayBuffer, quadVbo);
            gl.BufferData(BufferTargetARB.ArrayBuffer, (ReadOnlySpan<float>)vertices, BufferUsageARB.StaticDraw);

            gl.EnableVertexAttribArray(0);
            gl.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);

            uint instanceVbo = gl.GenBuffer();
            gl.BindBuffer(BufferTargetARB.ArrayBuffer, instanceVbo);

            uint stride = sizeof(float) * (3 + 3 + 4 + 1);

            // Start (location 1)
            gl.EnableVertexAttribArray(1);
            gl.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, stride, 0);
            gl.VertexAttribDivisor(1, 1);

            // End (location 2)
            gl.EnableVertexAttribArray(2);
            gl.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, stride, sizeof(float) * 3);
            gl.VertexAttribDivisor(2, 1);

            // Color (location 3)
            gl.EnableVertexAttribArray(3);
            gl.VertexAttribPointer(3, 4, VertexAttribPointerType.Float, false, stride, sizeof(float) * 6);
            gl.VertexAttribDivisor(3, 1);

            // Thickness (location 4)
            gl.EnableVertexAttribArray(4);
            gl.VertexAttribPointer(4, 1, VertexAttribPointerType.Float, false, stride, sizeof(float) * 10);
            gl.VertexAttribDivisor(4, 1);

            _lineQuad = new Line(gl, vao, quadVbo, instanceVbo);
        }

        return _lineQuad;
    }
}
