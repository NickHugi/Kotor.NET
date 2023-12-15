using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.FileFormats.KotorERF;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace KotorGL
{
    public class Frame
    {
        // list of things to renderss

        private IList<IRenderable> _renderables = new List<IRenderable>();

        public void Add(IRenderable renderable)
        {
            _renderables.Add(renderable);
        }

        public void RenderToView(Graphics graphics, Camera camera)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(0, 0, 0.3f, 1);

            GL.Disable(EnableCap.CullFace);

            var shader = graphics.GetShader("kotor");
            shader.Use();
            shader.SetUniformMatrix4x4("view", camera.GetView());
            shader.SetUniformMatrix4x4("projection", camera.GetProjection());
            shader.SetUniformMatrix4x4("model", Matrix4.Identity);

            foreach (var renderable in _renderables)
            {
                renderable.Render();
            }
            _renderables.Clear();
        }
    }
}
