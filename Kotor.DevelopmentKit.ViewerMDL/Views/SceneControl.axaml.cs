using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using Kotor.NET.Graphics.OpenGL.Factories;
using Kotor.NET.Graphics.OpenGL.GPU;
using Silk.NET.Core.Contexts;
using Silk.NET.OpenGL;

namespace Kotor.DevelopmentKit.ViewerMDL.Views;

public partial class SceneControl : OpenGlControlBase
{
    private AvaloniaSilkNativeContext _context;
    private GL _silk;

    public SceneControl()
    {
        InitializeComponent();
    }

    protected override void OnOpenGlInit(GlInterface gl)
    {
        base.OnOpenGlInit(gl);

        _context = new AvaloniaSilkNativeContext(gl.GetProcAddress);
        _silk = new GL(_context);

        gl.ClearColor(1.0f, 0.0f, 0.0f, 1.0f);


        float[] vertices =
            [
                0,0,0,
                0,1,0,
                1,0,0,
                1,1,0
            ];
        int[] indices =
            [
                0,1,2,
                1,2,3
            ];

        //var VAO = new VertexArrayObjectFactory().FromBinary(gl, [], [], 12, 0, 0, 0, 12, 0);
    }

    protected override void OnOpenGlDeinit(GlInterface gl)
    {
        base.OnOpenGlDeinit(gl);
    }

    protected override void OnOpenGlRender(GlInterface gl, int fb)
    {
        gl.Clear(0x00004000);
        //gl.ClearColor(0.1f, 0.2f, 0.3f, 1.0f);
        _silk.ClearColor(0.0f, 1.0f, 1.0f, 1.0f);
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

    //public IntPtr GetProcAddress(string procName)
    //    => _getProcAddress(procName);

    //public bool TryGetProcAddress(string proc, out nint address)
    //{
    //    address = _getProcAddress(proc);
    //    return address != IntPtr.Zero;
    //}
}
