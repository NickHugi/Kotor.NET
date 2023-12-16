using System.Timers;
using System;
using Avalonia.Controls;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using KotorGL;
using Avalonia.Threading;
using Silk.NET.OpenGLES;

namespace MapBuilder.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        
        InitializeComponent();
    }
}


public class KotorGLControl : OpenGlControlBase
{
    private GL _gl;
    private Scene _scene;

    public KotorGLControl()
    {
        //GlVersion = new GlVersion(GlProfileType.OpenGL, 4, 0);
        System.Timers.Timer timer = new(33);
        timer.Elapsed += OnTimedEvent;
        timer.Start();
    }

    private bool init = false;
    protected override unsafe void OnOpenGlInit(GlInterface gl)
    {
        _scene = new(GL.GetApi(gl.GetProcAddress), new());
        _scene.Init();
        init = true;
    }

    protected override unsafe void OnOpenGlDeinit(GlInterface gl)
    {

    }

    protected override unsafe void OnOpenGlRender(GlInterface gl, int fb)
    {
        if (init)
            _scene.Render();
    }

    private void OnTimedEvent(Object source, ElapsedEventArgs e)
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            RequestNextFrameRendering();
        });
    }
}
