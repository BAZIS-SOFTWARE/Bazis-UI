using Avalonia.OpenGL;
using OpenTK;

namespace BazisAvaloniaGUI.Scene;

internal class AvaloniaGlBindingsContext : IBindingsContext
{
    private readonly GlInterface gl;

    public AvaloniaGlBindingsContext(GlInterface gl)
    {
        this.gl = gl;
    }

    public IntPtr GetProcAddress(string procName) => gl.GetProcAddress(procName);
}
