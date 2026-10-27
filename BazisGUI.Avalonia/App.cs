using Avalonia;
using global::Avalonia.Controls.ApplicationLifetimes;
using global::Avalonia.Themes.Fluent;
using global::Avalonia.Markup.Xaml.Styling;
using global::Avalonia.Styling;
using global::Avalonia.Threading;
using BazisGUI.Avalonia.Shell;
using System.Diagnostics;

namespace BazisGUI.Avalonia;

internal class App : Application
{
    public override void Initialize()
    {
        RequestedThemeVariant = ThemeVariant.Light;
        Resources.MergedDictionaries.Add(new ResourceInclude(new Uri("avares://BazisGUI.Avalonia/"))
        {
            Source = new Uri("avares://BazisGUI.Avalonia/Styles/ShellResources.axaml")
        });
        Styles.Add(new FluentTheme());
        Styles.Add(new StyleInclude(new Uri("avares://BazisGUI.Avalonia/"))
        {
            Source = new Uri("avares://BazisGUI.Avalonia/Styles/ShellStyles.axaml")
        });
        Styles.Add(new StyleInclude(new Uri("avares://BazisGUI.Avalonia/"))
        {
            Source = new Uri("avares://BazisGUI.Avalonia/Navigator/NavigatorStyles.axaml")
        });
        Styles.Add(new StyleInclude(new Uri("avares://BazisGUI.Avalonia/"))
        {
            Source = new Uri("avares://BazisGUI.Avalonia/Properties/PropertiesStyles.axaml")
        });
        Styles.Add(new StyleInclude(new Uri("avares://BazisGUI.Avalonia/"))
        {
            Source = new Uri("avares://BazisGUI.Avalonia/Console/ConsoleStyles.axaml")
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
