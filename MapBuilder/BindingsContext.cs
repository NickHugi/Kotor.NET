using Avalonia.OpenGL;
using OpenTK;
//using static MapBuilder.Views.OpenGLControl;

namespace MapBuilder;

public class BindingsContext : IBindingsContext
{
    private GlInterface _gl;

    public BindingsContext(GlInterface gl)
    {
        _gl = gl;
    }

    public nint GetProcAddress(string procName)
    {
        return _gl.GetProcAddress(procName);
    }
}
