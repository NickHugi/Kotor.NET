using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorGL.SceneObjects;

namespace KotorGL
{
    public class Scene
    {
        public List<SceneObject> _objects = new();

        private Graphics _graphics;
        private Frame _frame = new();

        public Scene(Graphics graphics)
        {
            _graphics = graphics;

            _objects.Add(new CubeObject());
        }

        public void Render()
        {
            foreach (var sceneObject in _objects)
            {
                sceneObject.GetRenderables(_graphics).ForEach(renderable => _frame.Add(renderable));
            }
        }
    }
}
