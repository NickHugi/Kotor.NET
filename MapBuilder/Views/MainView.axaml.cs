using System.Timers;
using System;
using Avalonia.Controls;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using KotorGL;
using Avalonia.Threading;
//using static MapBuilder.Views.OpenGLControl;

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
    private Scene _scene;

    public KotorGLControl()
    {
        //GlVersion = new GlVersion(GlProfileType.OpenGL, 4, 0);
        System.Timers.Timer timer = new(33);
        timer.Elapsed += OnTimedEvent;
        timer.Start();
    }

    private static void CheckError(GlInterface gl)
    {
        int err;
        //while ((err = gl.GetError()) != GL_NO_ERROR
    }

    private bool init = false;
    protected override unsafe void OnOpenGlInit(GlInterface gl)
    {
        
        var graphics = new Graphics();
        var context = new BindingsContext(gl);
        _scene = new(graphics, context);
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
