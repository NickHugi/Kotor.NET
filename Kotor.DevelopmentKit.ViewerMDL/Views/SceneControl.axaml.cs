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
using Avalonia.Threading;
using Kotor.DevelopmentKit.ViewerMDL.ViewModels;
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
    public MDLResourceViewerViewModel ViewModel => (MDLResourceViewerViewModel)DataContext;

    public SceneControl()
    {
        InitializeComponent();
    }

    protected unsafe override void OnOpenGlInit(GlInterface gl)
    {
        base.OnOpenGlInit(gl);

        ViewModel.Context = new AvaloniaSilkNativeContext(gl.GetProcAddress);
        ViewModel.GL = new GL(ViewModel.Context);
        ViewModel.GL.Enable(EnableCap.DepthTest);
        ViewModel.AssetManager = new AssetManager();

        var shader = new ShaderFactory(ViewModel.GL).FromFile("Assets/vertex.glsl", "Assets/fragment.glsl");
        ViewModel.AssetManager.AddShader("basic", shader);

        var texture0 = new TPCTextureFactory(ViewModel.GL).FromFile(@"C:\Users\hugin\Desktop\Modding\model_c_selkath\N_Selkath01.tpc");
        ViewModel.AssetManager.AddTexture("N_Selkath01", texture0);
    }

    protected override void OnOpenGlDeinit(GlInterface gl)
    {
        base.OnOpenGlDeinit(gl);
    }

    protected override void OnOpenGlRender(GlInterface gl, int fb)
    {
        var scale = TopLevel.GetTopLevel(this).RenderScaling;
        var width = (uint)(Bounds.Width * scale);
        var height = (uint)(Bounds.Height * scale);
        ViewModel.GL.Viewport(0, 0, width, height);

        ViewModel.GL.ClearColor(0.1f, 0.0f, 0.0f, 1.0f);
        ViewModel.GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

        var frame = new RenderFrame(ViewModel.AssetManager, []);

        var projectionLocation = ViewModel.AssetManager.GetShader("basic").GetUniformLocation("projection");
        var viewLocation = ViewModel.AssetManager.GetShader("basic").GetUniformLocation("view");
        var modelLocation = ViewModel.AssetManager.GetShader("basic").GetUniformLocation("model");
        var textureLocation = ViewModel.AssetManager.GetShader("basic").GetUniformLocation("texture1");

        var identity = Matrix4x4.Identity.ToDoubleArray();
        var projection = Matrix4x4.CreatePerspectiveFieldOfView((float)Math.PI/3f, width / (float)height, 0.001f, 1000).ToDoubleArray();
        var view = Matrix4x4.CreateLookAt(new(1, 3, 2), new(0, 0, 1), new(0, 0, 1)).ToDoubleArray();
        ViewModel.AssetManager.GetShader("basic").Activate();
        ViewModel.GL.UniformMatrix4(projectionLocation, false, projection);
        ViewModel.GL.UniformMatrix4(viewLocation, false, view);
        ViewModel.GL.UniformMatrix4(modelLocation, false, identity);
        ViewModel.GL.Uniform1(textureLocation, 0);

        while(ViewModel.ModelBuffer.Count > 0)
        {
            var name = ViewModel.ModelBuffer.Keys.First();
            ViewModel.ModelBuffer.TryRemove(name, out var getModel);
            ViewModel.AssetManager.AddModel(name, getModel());
        }

        if (ViewModel.AssetManager.HasModel("model"))
            ViewModel.AssetManager.GetModel("model").Render(frame);

        frame.Render();
        RequestNextFrameRendering();
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
