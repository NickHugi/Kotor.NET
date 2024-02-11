using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Formats.KotorTPC;
using Kotor.NET.Graphics.SceneObjects;
using Silk.NET.OpenGLES;

namespace Kotor.NET.Graphics
{
    public class Scene
    {
        public Camera Camera = new();

        private List<SceneObject> _objects = new();
        private Graphics _graphics;
        private Frame _frame = new();
        private GL _gl;

        public Scene(GL gl, Graphics graphics)
        {
            _gl = gl;
            _graphics = graphics;
        }

        int VertexArrayObject;
        Shader shader;
        public void Init()
        {
            _gl.DebugMessageCallback(DebugMessageDelegate, IntPtr.Zero);
            _gl.Enable(EnableCap.DebugOutput);
            _gl.Enable(EnableCap.DepthTest);

            _graphics.Shaders.Add("kotor", new Shader(_gl, "kotor"));
            _graphics.Shaders.Add("test", new Shader(_gl, "test"));

            _graphics.Textures.Add("lda_grass01", new Texture(_gl, new TGABinaryReader(File.ReadAllBytes(@"Assets/lda_grass01.tga")).Read()));
            _graphics.Textures.Add("plc_jnkspdr1", new Texture(_gl, new TGABinaryReader(File.ReadAllBytes(@"Assets/plc_jnkspdr1.tga")).Read() ));
            _graphics.Textures.Add("plc_spdrwin01", new Texture(_gl, new TGABinaryReader(File.ReadAllBytes(@"Assets/plc_spdrwin01.tga")).Read() ));

            var thingy = new KotorModelLoader(File.ReadAllBytes(@"Assets\plc_jnkspdr1.mdl"), File.ReadAllBytes(@"Assets\plc_jnkspdr1.mdx")).Read(_graphics);
            _objects.Add(thingy);

            CubeObject.InitializeVertexArray(_gl, _graphics);
            TriangleObject.InitializeVertexArray(_gl, _graphics);
        }

        public void Render(uint width, uint height)
        {
            if (width != Camera.Width || height != Camera.Height)
            {
                var scaling = 1.5f;
                Camera.Width = width;
                Camera.Height = height;
                _gl.Viewport(0, 0, (uint)(width * scaling), (uint)(height * scaling));
            }

            foreach (var sceneObject in _objects)
            {
                sceneObject.GetRenderables(_graphics).ForEach(renderable => _frame.Add(renderable));
            }

            _frame.RenderToView(_graphics, Camera);
        }

        public void AddObject(SceneObject sceneObject)
        {
            _objects.Add(sceneObject);
        }

        private static DebugProc DebugMessageDelegate = OnDebugMessage;
        private static void OnDebugMessage(GLEnum source, GLEnum type, int id, GLEnum severity, int length, nint pMessage, nint pUserParam)
        {
            string message = Marshal.PtrToStringAnsi(pMessage, length);

            if (type == GLEnum.DebugTypeError)
            {
                throw new Exception(message);
            }
        }
    }
}
