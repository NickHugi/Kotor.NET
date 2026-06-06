using System;
using System.Numerics;
using Kotor.NET.Graphics;
using Kotor.NET.Graphics.Cameras;
using Kotor.NET.Graphics.Entities;
using Kotor.NET.Graphics.GPU;
using Kotor.NET.Graphics.Interface;
using Kotor.NET.Graphics.Model.Nodes;
using Kotor.NET.Graphics.OpenGL;
using Kotor.NET.Graphics.OpenGL.Factories;
using Kotor.NET.Graphics.Renderers;
using Kotor.NET.Graphics.Renderers.Descriptors;
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
        assets.Billboard = new VertexArrayObjectFactory().NewBillBoard(gl);
        assets.Line = new VertexArrayObjectFactory().GetLineQuad(gl);
        assets.AddShader("billboard", new ShaderFactory(gl).FromFile("Assets/billboard/vertex.glsl", "Assets/billboard/fragment.glsl"));
        assets.AddShader("image", new ShaderFactory(gl).FromFile("Assets/image/vertex.glsl", "Assets/image/fragment.glsl"));
        assets.AddShader("line", new ShaderFactory(gl).FromFile("Assets/line/vertex.glsl", "Assets/line/fragment.glsl"));
        assets.AddShader("basic", new ShaderFactory(gl).FromFile("Assets/standard/vertex.glsl", "Assets/standard/fragment.glsl"));
        assets.AddShader("picker", new ShaderFactory(gl).FromFile("Assets/picker/vertex.glsl", "Assets/picker/fragment.glsl"));
        assets.AddTexture("placeholder", new TPCTextureFactory(gl).FromPlaceholder());
        assets.AddTexture("test", new TPCTextureFactory(gl).FromFile(@"C:\Kits\sandral\lda_flr05.tpc"));

        //scene.AddControl(new SimpleImageControl()
        //{
        //    X = 10,
        //    Y = 10,
        //    Width = 256,
        //    Height = 256,
        //    Image = "test"
        //});

        scene.AddEntity(new LineEntity()
        {
            Start = new(100, 100, 0),
            End = new(-100, -100, 0),
            Color = new(1, 0, 0, 1),
            Thickness = 1
        });

        scene.AddEntity(new SimpleBillboardEntity()
        {
            DoRender = true,
            FixedSize = false,
            Image = "test",
            Location = new Vector3(10, 10, 0),
            Size = 1
        });

        var yaw = 0f;
        while (!glfw.WindowShouldClose(window))
        {
            glfw.PollEvents();

            yaw += 0.0001f;
            engine.Render(new OrbitCamera()
            {
                Target = Vector3.Zero,
                Pitch = (float)(Math.PI / 4),
                Yaw = yaw,
                Distance = 50
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

public class LineEntity : BaseEntity
{
    public required Vector3 Start { get; init; }
    public required Vector3 End { get; init; }
    public required Vector4 Color { get; init; }
    public float Thickness { get; init; } = 1;

    public override ICollection<IDrawCallDescriptor> GetMeshDescriptors(IAssetManager assets)
    {
        return
        [
            new LineDescriptor()
            {
                Start = Start,
                End = End,
                Color = Color,
                Thickness = Thickness,
            }
        ];
    }

    public override void Update(IAssetManager assetManager, float delta)
    {
    }
}

public class SimpleBillboardEntity : BaseEntity
{
    public required bool DoRender { get; init; }
    public required bool FixedSize { get; init; }
    public required string Image { get; init; }
    public required Vector3 Location { get; set; }
    public required float Size { get; init; }

    public override ICollection<IDrawCallDescriptor> GetMeshDescriptors(IAssetManager assets)
    {
        return
        [
            new BillboardDescriptor()
            {
                DoRender = DoRender,
                FixedSize = FixedSize,
                Image = Image,
                Location = Location,
                Size = Size
            }
        ];
    }

    public override void Update(IAssetManager assetManager, float delta)
    {
    }
}

