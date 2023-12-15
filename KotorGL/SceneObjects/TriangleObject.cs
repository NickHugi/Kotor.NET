using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace KotorGL.SceneObjects
{
    public class TriangleObject : SceneObject
    {
        public override List<IRenderable> GetRenderables(Graphics graphics)
        {
            return new() { new Renderable(graphics.GetVAO(":triangle"), graphics.GetShader("test"), null, null) };
        }

        public static void InitializeVertexArray(Graphics graphics)
        {
            float[] vertices =
            {
                -0.5f, -0.5f, 0.0f,
                 0.5f, -0.5f, 0.0f,
                 0.0f,  0.5f, 0.0f
             };

            short[] indices =
            {
                //0, 1, 3,   // first triangle
                //1, 2, 3    // second triangle
                0,1,2
            };

            int VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            int VertexBufferObject = GL.GenBuffer();
            int ElementBufferObject = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(1);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(short), indices, BufferUsageHint.StaticDraw);

            graphics.SetVAO(":triangle", new VertexArray(VertexArrayObject, indices.Length));
        }
    }
}
