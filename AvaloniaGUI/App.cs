using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Themes.Fluent;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;
using Avalonia.Threading;
using AvaloniaGUI.Shell;
using System.Diagnostics;

namespace AvaloniaGUI;

internal class App : Application
{
    public override void Initialize()
    {
        RequestedThemeVariant = ThemeVariant.Light;
        Resources.MergedDictionaries.Add(new ResourceInclude(new Uri("avares://AvaloniaGUI/"))
        {
            Source = new Uri("avares://AvaloniaGUI/Styles/ShellResources.axaml")
        });
        Styles.Add(new FluentTheme());
        Styles.Add(new StyleInclude(new Uri("avares://AvaloniaGUI/"))
        {
            Source = new Uri("avares://AvaloniaGUI/Styles/ShellStyles.axaml")
        });
        Styles.Add(new StyleInclude(new Uri("avares://AvaloniaGUI/"))
        {
            Source = new Uri("avares://AvaloniaGUI/Navigator/NavigatorStyles.axaml")
        });
        Styles.Add(new StyleInclude(new Uri("avares://AvaloniaGUI/"))
        {
            Source = new Uri("avares://AvaloniaGUI/Properties/PropertiesStyles.axaml")
        });
        Styles.Add(new StyleInclude(new Uri("avares://AvaloniaGUI/"))
        {
            Source = new Uri("avares://AvaloniaGUI/Console/ConsoleStyles.axaml")
        });
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Страховочная сеть: необработанное исключение в обработчике UI (в т.ч. async void)
        // не должно завершать приложение — логируем и показываем сообщение.
        Dispatcher.UIThread.UnhandledException += OnUIThreadUnhandledException;

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = new MainWindow(desktop.Args ?? []);

        base.OnFrameworkInitializationCompleted();
    }

    private bool showingUnhandledError;

    private void OnUIThreadUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        e.Handled = true;
        Trace.TraceError($"Необработанное исключение в UI-потоке: {e.Exception}");

        // Не плодим окна, если исключения сыплются каскадом (например, на каждом тике таймера).
        if (showingUnhandledError)
            return;
        if ((ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow is not { } owner)
            return;

        showingUnhandledError = true;
        var message = e.Exception.GetBaseException().Message;
        Dispatcher.UIThread.Post(async () =>
        {
            try
            {
                await MessageBox.Show(owner, message, "Ошибка");
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Не удалось показать сообщение об ошибке: {ex}");
            }
            finally
            {
                showingUnhandledError = false;
            }
        });
    }
}
