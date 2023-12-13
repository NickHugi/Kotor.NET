using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.FileFormats.KotorMDL;
using OpenTK.Graphics.OpenGL4;

namespace KotorGL
{
    public class VertexArray
    {
        public int ID { get; }
        public int FaceCount { get; }

        //public VertexArray(IEnumerable<Vector3> points, IEnumerable<Vector3> normals, IEnumerable<Vector2> uvs, IEnumerable<int> indexes)
        //{

        //    GL.GenVertexArrays(1, out int vao);
        //    GL.GenBuffers(1, out int vbo);
        //    GL.GenBuffers(1, out int ebo);

        //    GL.BindVertexArray(ID)

        //    GL.BindBuffer(BufferTarget.ArrayBuffer, vbo)
        //    GL.BufferData(BufferTarget.ArrayBuffer, len(vertex_data), vertex_data, GL_STATIC_DRAW)

        //    GL.BindBuffer(BufferTarget.ElementArrayBuffer, self._ebo)
        //    GL.BufferData(BufferTarget.ElementArrayBuffer, len(element_data), element_data, GL_STATIC_DRAW)
        //    self._face_count = len(element_data) // 2

        //    if data_bitflags & 0x0001:
        //        glEnableVertexAttribArray(1)
        //        glVertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, block_size, ctypes.c_void_p(vertex_offset))

        //    if data_bitflags & 0x0020 and texture != "" and texture != "NULL":
        //        glEnableVertexAttribArray(3)
        //        GL.VertexAttribPointer(3, 2, VertexAttribPointerType.Float, false, block_size, ctypes.c_void_p(texture_offset))
        //        self.texture = texture

        //    if data_bitflags & 0x0004 and lightmap != "" and lightmap != "NULL":
        //        GL.EnableVertexAttribArray(4)
        //        GL.VertexAttribPointer(4, 2, VertexAttribPointerType.Float, false, block_size, ctypes.c_void_p(lightmap_offset))
        //        self.lightmap = lightmap

        //    GL.BindBuffer(BufferTarget.ArrayBuffer, 0)
        //    GL.BindVertexArray(0);

        //    ID = vao;
        //}

        public VertexArray(byte[] vertexData, byte[] elementData, int positionsOffset, int normalsOffset, int uv1Stride, int uv2Stride, int blockSize, int flags)
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

            ID = vao;
        }

        public void Draw()
        {
            GL.BindVertexArray(ID);
            GL.DrawElements(BeginMode.Triangles, FaceCount, DrawElementsType.UnsignedShort, 0);
        }
    }
}
