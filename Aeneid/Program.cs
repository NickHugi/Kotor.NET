using System;
using System.Numerics;
using Kotor.NET.Graphics;
using Kotor.NET.Graphics.Cameras;
using Kotor.NET.Graphics.GPU;
using Kotor.NET.Graphics.Interface;
using Kotor.NET.Graphics.OpenGL;
using Kotor.NET.Graphics.OpenGL.Factories;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;

class Program
{
    private static Glfw glfw;
    private static GL gl;
    private static GLEngine engine;
    private static unsafe WindowHandle* window;
    private static AssetManager assets;


    static unsafe void Main()
    {
        glfw = Glfw.GetApi();
        assets = new();

        // Initialize GLFW
        if (!glfw.Init())
        {
            Console.WriteLine("Failed to initialize GLFW");
            return;
        }

        // Create window
        window = glfw.CreateWindow(
            800,
            600,
            "Silk.NET GLFW Example",
            null,
            null);

        if (window == null)
        {
            Console.WriteLine("Failed to create window");
            glfw.Terminate();
            return;
        }


        glfw.MakeContextCurrent(window);
        gl = GL.GetApi(new GlfwContext(glfw, window));

        var scene = new Scene();

        engine = new GLEngine()
        {
            GL = gl,
            Scene = scene,
            AssetManager = assets,
            Width = 800,
            Height = 600
        };

        assets.Quad = new VertexArrayObjectFactory().NewQuad(gl);
        assets.AddShader("image", new ShaderFactory(gl).FromFile("Assets/image/vertex.glsl", "Assets/image/fragment.glsl"));
        assets.AddShader("basic", new ShaderFactory(gl).FromFile("Assets/standard/vertex.glsl", "Assets/standard/fragment.glsl"));
        assets.AddShader("picker", new ShaderFactory(gl).FromFile("Assets/picker/vertex.glsl", "Assets/picker/fragment.glsl"));
        assets.AddTexture("placeholder", new TPCTextureFactory(gl).FromPlaceholder());
        assets.AddTexture("test", new TPCTextureFactory(gl).FromFile(@"C:\Kits\sandral\lda_flr05.tpc"));

        scene.AddControl(new SimpleImageControl()
        {
            X = 10,
            Y = 10,
            Width = 256,
            Height = 256,
            Image = "test"
        });

        while (!glfw.WindowShouldClose(window))
        {
            glfw.PollEvents();

            engine.Render(new OrbitCamera()
            {
                Target = Vector3.Zero,
                Distance = 5,
            });

            glfw.SwapBuffers(window);
        }

        // Cleanup
        glfw.DestroyWindow(window);
        glfw.Terminate();
    }
}

public class SimpleImageControl : BaseControl
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string Image { get; set; }

    public override ICollection<ImageDescriptor> GetImageDescriptors(IAssetManager assets)
    {
        return
        [
            new ImageDescriptor()
            {
                X = X,
                Y = Y,
                Width = Width,
                Height = Height,
                Image = Image,
                DoRender = true
            }
        ];
    }
}
