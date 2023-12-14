using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorGL.SceneObjects;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace KotorGL
{
    public class Scene
    {
        public List<SceneObject> _objects = new();

        private Graphics _graphics;
        private IBindingsContext _context;
        private Frame _frame = new();

        public Scene(Graphics graphics, IBindingsContext context)
        {
            _graphics = graphics;
            _context = context;

            _objects.Add(new CubeObject());
        }

        public void Init()
        {
            GL.LoadBindings(_context);

            _graphics.Shaders.Add("default", new Shader("kotor"));
            _objects.Add(new SceneObject());
        }

        public void Render()
        {
            GL.ClearColor(1, 1, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit);            

            foreach (var sceneObject in _objects)
            {
                sceneObject.GetRenderables(_graphics).ForEach(renderable => _frame.Add(renderable));
            }
        }
    }
}
