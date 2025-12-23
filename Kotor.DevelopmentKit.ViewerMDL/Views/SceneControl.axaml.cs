using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using Kotor.NET.Formats.BinaryMDL;
using Kotor.NET.Graphics;
using Kotor.NET.Graphics.GPU;
using Kotor.NET.Graphics.OpenGL;
using Kotor.NET.Graphics.OpenGL.Factories;
using Kotor.NET.Graphics.OpenGL.GPU;
using Silk.NET.Core.Contexts;
using Silk.NET.Core.Native;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Shader = Kotor.NET.Graphics.OpenGL.GPU.Shader;

namespace Kotor.DevelopmentKit.ViewerMDL.Views;

public partial class SceneControl : OpenGlControlBase
{
    private AvaloniaSilkNativeContext _context;
    private GL _silk;

    public SceneControl()
    {
        InitializeComponent();
        IAssetManager.Manager = new AssetManager();
    }

    protected unsafe override void OnOpenGlInit(GlInterface gl)
    {
        base.OnOpenGlInit(gl);

        float[] vertices =
            [
                -1f, -1f, 0,
                1f, -1f, 0,
                1f, 1f, 0,
                -1f, 1f, 0
            ];
        ushort[] indices =
            [
                0,1,3,
                1,2,3
            ];

        byte[] vertexData = vertices.SelectMany(BitConverter.GetBytes).ToArray();
        byte[] indexData = indices.SelectMany(BitConverter.GetBytes).ToArray();

        _context = new AvaloniaSilkNativeContext(gl.GetProcAddress);
        _silk = new GL(_context);
        _silk.Enable(EnableCap.DepthTest);

        _shader = new ShaderFactory(_silk).FromFile("Assets/vertex.glsl", "Assets/fragment.glsl");
        IAssetManager.Manager.AddShader("basic", _shader);

        var texture0 = new TPCTextureFactory(_silk).FromFile(@"C:\Users\hugin\Desktop\Modding\model_c_selkath\N_Selkath01.tpc");
        IAssetManager.Manager.AddTexture("N_Selkath01", texture0);

        var mdl = File.ReadAllBytes(@"C:\Users\hugin\Desktop\Modding\model_c_selkath\c_selkath.mdl");
        var mdx = File.ReadAllBytes(@"C:\Users\hugin\Desktop\Modding\model_c_selkath\c_selkath.mdx");
        _model = new ModelLoader().LoadModel(_silk, mdl, mdx);
        //_vao = new VertexArrayObjectFactory().FromBinary(_silk, vertexData, indexData, 0, 0, 0, 0, 12, (uint)(MDLBinaryMDXVertexBitmask.Vertices));
    }

    protected override void OnOpenGlDeinit(GlInterface gl)
    {
        base.OnOpenGlDeinit(gl);
    }

    private IModel _model;
    private IVertexArrayObject _vao;
    private IShader _shader;
    protected override void OnOpenGlRender(GlInterface gl, int fb)
    {
        var scale = TopLevel.GetTopLevel(this).RenderScaling;
        var width = (uint)(Bounds.Width * scale);
        var height = (uint)(Bounds.Height * scale);
        _silk.Viewport(0, 0, width, height);

        _silk.ClearColor(0.1f, 0.0f, 0.0f, 1.0f);
        _silk.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

        var frame = new RenderFrame([]);

        var projectionLocation = _shader.GetUniformLocation("projection");
        var viewLocation = _shader.GetUniformLocation("view");
        var modelLocation = _shader.GetUniformLocation("model");
        var textureLocation = _shader.GetUniformLocation("texture1");

        var identity = Matrix4x4.Identity.ToDoubleArray();
        var projection = Matrix4x4.CreatePerspectiveFieldOfView((float)Math.PI/3f, width / (float)height, 0.001f, 1000).ToDoubleArray();
        var view = Matrix4x4.CreateLookAt(new(0, 3, 1), new(0, 0, 1), new(0, 0, 1)).ToDoubleArray();
        _shader.Activate();
        _silk.UniformMatrix4(projectionLocation, false, projection);
        _silk.UniformMatrix4(viewLocation, false, view);
        _silk.UniformMatrix4(modelLocation, false, identity);
        _silk.Uniform1(textureLocation, 0);
        _model.Render(frame);

        frame.Render();
    }
}

public class AvaloniaSilkNativeContext : INativeContext
{
    private readonly Func<string, IntPtr> _getProcAddress;

    public AvaloniaSilkNativeContext(Func<string, IntPtr> getProcAddress)
    {
        _getProcAddress = getProcAddress;
    }

    public void Dispose()
    {

    }

    public nint GetProcAddress(string proc, int? slot = null)
    {
        return _getProcAddress(proc);
    }

    public bool TryGetProcAddress(string proc, out nint addr, int? slot = null)
    {
        addr = _getProcAddress(proc);
        return true;
    }

    public static Matrix4x4 CreateLookAtZUp(
        Vector3 eye,
        Vector3 target)
    {
        // Forward (camera looks toward target)
        Vector3 forward = Vector3.Normalize(eye - target);

        // Z is up
        Vector3 up = Vector3.UnitZ;

        // Right = Up × Forward
        Vector3 right = Vector3.Normalize(Vector3.Cross(up, forward));

        // Recompute orthogonal up
        Vector3 trueUp = Vector3.Cross(forward, right);

        return new Matrix4x4(
            right.X, trueUp.X, forward.X, 0f,
            right.Y, trueUp.Y, forward.Y, 0f,
            right.Z, trueUp.Z, forward.Z, 0f,
            -Vector3.Dot(right, eye),
            -Vector3.Dot(trueUp, eye),
            -Vector3.Dot(forward, eye),
            1f
        );
    }

    //public IntPtr GetProcAddress(string procName)
    //    => _getProcAddress(procName);

    //public bool TryGetProcAddress(string proc, out nint address)
    //{
    //    address = _getProcAddress(proc);
    //    return address != IntPtr.Zero;
    //}
}
