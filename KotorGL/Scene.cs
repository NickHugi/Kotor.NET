using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using KotorGL.SceneObjects;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace KotorGL
{
    public class Scene
    {
        public Camera Camera = new();

        private List<SceneObject> _objects = new();
        private Graphics _graphics;
        private IBindingsContext _context;
        private Frame _frame = new();

        public Scene(Graphics graphics, IBindingsContext context)
        {
            _graphics = graphics;
            _context = context;
        }

        int VertexArrayObject;
        Shader shader;
        public void Init()
        {
            if (_context is not null)
                GL.LoadBindings(_context);

            GL.DebugMessageCallback(DebugMessageDelegate, IntPtr.Zero);
            GL.Enable(EnableCap.DebugOutput);

            _graphics.Shaders.Add("kotor", new Shader("kotor"));
            _graphics.Shaders.Add("test", new Shader("test"));

            CubeObject.InitializeVertexArray(_graphics);
            TriangleObject.InitializeVertexArray(_graphics);

            _objects.Add(new CubeObject());
            //_objects.Add(new TriangleObject());
        }

        public void Render()
        {
            foreach (var sceneObject in _objects)
            {
                sceneObject.GetRenderables(_graphics).ForEach(renderable => _frame.Add(renderable));
            }

            _frame.RenderToView(_graphics, Camera);
        }

        private static DebugProc DebugMessageDelegate = OnDebugMessage;
        private static void OnDebugMessage(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr pMessage, IntPtr pUserParam)
        {
            string message = Marshal.PtrToStringAnsi(pMessage, length);

            if (type == DebugType.DebugTypeError)
            {
                throw new Exception(message);
            }
        }
    }
}
