using Avalonia.Controls;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using KotorGL;
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

    private static void CheckError(GlInterface gl)
    {
        int err;
        //while ((err = gl.GetError()) != GL_NO_ERROR
    }

    protected override unsafe void OnOpenGlInit(GlInterface gl)
    {
        var graphics = new Graphics();
        var context = new BindingsContext(gl);
        _scene = new(graphics, context);
        _scene.Init();
        RequestNextFrameRendering();
    }

    protected override unsafe void OnOpenGlDeinit(GlInterface gl)
    {

    }

    protected override unsafe void OnOpenGlRender(GlInterface gl, int fb)
    {
        _scene.Render();
    }
}
