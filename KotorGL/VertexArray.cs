using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.Common.Creature;
using KotorDotNET.FileFormats.KotorMDL;
using OpenTK.Graphics.OpenGL4;

namespace KotorGL
{
    public class VertexArray
    {
        public int ID { get; }
        public int FaceCount { get; }

        public VertexArray(IEnumerable<Vector3> points, IEnumerable<short> indexes, IEnumerable<Vector3>? normals = null, IEnumerable<Vector2>? uv1s = null, IEnumerable<Vector2>? uv2s = null)
        {
            if (points.Count() != normals.Count() && points.Count() != uv1s.Count() && points.Count() != uv2s.Count())
                throw new NotImplementedException();

            var flags = 0;
            var blockSize = 0;
            var positionStride = 0;
            var normalStride = 0;
            var uv1Stride = 0;
            var uv2Stride = 0;

            var vertexData = new List<byte>();
            for (int i = 0; i < points.Count(); i ++)
            {
                if (points is not null)
                {
                    flags |= 0x0001;
                    positionStride = blockSize;
                    blockSize += 12;
                    vertexData.AddRange(BitConverter.GetBytes(points.ElementAt(i).X));
                    vertexData.AddRange(BitConverter.GetBytes(points.ElementAt(i).Y));
                    vertexData.AddRange(BitConverter.GetBytes(points.ElementAt(i).Z));
                }
                if (normals is not null)
                {
                    //flags |= ;
                    //vertexData.AddRange(BitConverter.GetBytes(normals.ElementAt(i).X));
                    //vertexData.AddRange(BitConverter.GetBytes(normals.ElementAt(i).Y));
                    //vertexData.AddRange(BitConverter.GetBytes(normals.ElementAt(i).Z));
                }
                if (uv1s is not null)
                {
                    flags |= 0x0020;
                    uv1Stride = blockSize;
                    blockSize += 8;
                    vertexData.AddRange(BitConverter.GetBytes(uv1s.ElementAt(i).X));
                    vertexData.AddRange(BitConverter.GetBytes(uv1s.ElementAt(i).Y));
                }
                if (uv2s is not null)
                {
                    flags |= 0x0004;
                    uv2Stride = blockSize;
                    blockSize += 8;
                    vertexData.AddRange(BitConverter.GetBytes(uv2s.ElementAt(i).X));
                    vertexData.AddRange(BitConverter.GetBytes(uv2s.ElementAt(i).Y));
                }
            }

            var elementData = indexes.SelectMany(x => BitConverter.GetBytes(x));

            ID = Init(vertexData.ToArray(), elementData.ToArray(), positionStride, normalStride, uv1Stride, uv2Stride, blockSize, flags);
        }

        public VertexArray(byte[] vertexData, byte[] elementData, int positionsOffset, int normalsOffset, int uv1Stride, int uv2Stride, int blockSize, int flags)
        {
            ID = Init(vertexData, elementData, positionsOffset, normalsOffset, uv1Stride, uv2Stride, blockSize, flags);
        }

        public void Draw()
        {
            GL.BindVertexArray(ID);
            GL.DrawElements(BeginMode.Triangles, FaceCount, DrawElementsType.UnsignedShort, 0);
        }

        private int Init(byte[] vertexData, byte[] elementData, int positionsOffset, int normalsOffset, int uv1Stride, int uv2Stride, int blockSize, int flags)
        {
            GL.GenVertexArrays(1, out int vao);
            GL.GenBuffers(1, out int vbo);
            GL.GenBuffers(1, out int ebo);

            GL.BindVertexArray(ID);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertexData.Length, vertexData, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, elementData.Length, elementData, BufferUsageHint.StaticDraw);
            //self._face_count = elementData / 2

            if ((flags & 0x0001) > 0)
            {
                GL.EnableVertexAttribArray(1);
                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, blockSize, positionsOffset);
            }

            if ((flags & 0x0020) > 0)
            {
                GL.EnableVertexAttribArray(3);
                GL.VertexAttribPointer(3, 2, VertexAttribPointerType.Float, false, blockSize, uv1Stride);
            }

            if ((flags & 0x0004) > 0)
            {
                GL.EnableVertexAttribArray(4);
                GL.VertexAttribPointer(4, 2, VertexAttribPointerType.Float, false, blockSize, uv2Stride);
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            return vao;
        }
    }
}
