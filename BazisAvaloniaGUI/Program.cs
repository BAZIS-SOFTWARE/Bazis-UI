using Avalonia;
using Avalonia.OpenGL;
using System.Diagnostics;
using System.Globalization;

namespace BazisAvaloniaGUI;

internal class Program
{
    /// <summary>
    /// Как в WinForms (GUI/Program.cs): числа в консоли, скриптах и полях свойств разбираются
    /// с точкой независимо от региональных настроек ОС. Default* — чтобы фоновые потоки
    /// (команды консоли выполняются в Task) получали ту же культуру.
    /// </summary>
    internal static void ConfigureCulture()
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
    }

    // Точка входа .NET должна быть статической.
    // STAThread обязателен для Win32: буфер обмена (OLE) и диалоги требуют STA-потока,
    // иначе копирование текста падает с CO_E_NOTINITIALIZED (как [STAThread] в GUI/Program.cs).
    [STAThread]
    private static void Main(string[] args)
    {
        ConfigureCulture();

        // Исключения незамеченных задач не должны ронять процесс — только лог.
        TaskScheduler.UnobservedTaskException += (_, e) =>
        {
            Trace.TraceError($"Необработанное исключение в задаче: {e.Exception}");
            e.SetObserved();
        };

        var builder = AppBuilder.Configure<App>().UsePlatformDetect();
        var profiles = new[]
        {
            new GlVersion(GlProfileType.OpenGL, 4, 6, isCompatibilityProfile: true),
            new GlVersion(GlProfileType.OpenGL, 3, 3, isCompatibilityProfile: true)
        };

        if (OperatingSystem.IsWindows())
            builder = builder.With(new Win32PlatformOptions
            {
                RenderingMode = [Win32RenderingMode.Wgl],
                WglProfiles = profiles
            });
        else if (OperatingSystem.IsLinux())
            builder = builder.With(new X11PlatformOptions
            {
                RenderingMode = [X11RenderingMode.Glx, X11RenderingMode.Egl],
                GlProfiles = profiles
            });

        builder.StartWithClassicDesktopLifetime(args);
    }
}
