using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorGL;
using Silk.NET.Maths;
using Silk.NET.OpenGLES;
using Silk.NET.Windowing;

namespace ConsoleApp1
{
    public class Viewer
    {
        IWindow _window;
        Scene _scene;

        public Viewer()
        {
            WindowOptions options = WindowOptions.Default;
            options.Size = new Vector2D<int>(1920, 1080);
            options.Title = "My first Silk.NET program!";

            _window = Window.Create(options);

            _window.Load += OnLoad;
            _window.Update += OnUpdate;
            _window.Render += OnRender;

        }

        public void Run()
        {
            _window.Run();
        }

        private void OnLoad()
        {
            _scene = new(_window.CreateOpenGLES(), new());
            _scene.Init();
        }

        private void OnUpdate(double deltaTime)
        {
             
        }

        private void OnRender(double deltaTime)
        {
            _scene.Render(1920, 1080);
        }
    }
}
