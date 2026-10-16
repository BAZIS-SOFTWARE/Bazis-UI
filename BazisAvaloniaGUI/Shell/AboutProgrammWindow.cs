using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace BazisAvaloniaGUI.Shell
{
    /// <summary>Упрощённая замена AboutProgrammControl (баннер и сведения о лицензии не перенесены).</summary>
    internal sealed class AboutProgrammWindow : Window
    {
        public AboutProgrammWindow()
        {
            Width = 380;
            SizeToContent = SizeToContent.Height;
            CanResize = false;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            FontFamily = new FontFamily("Microsoft Sans Serif");
            FontSize = 11;

            var version = typeof(AboutProgrammWindow).Assembly.GetName().Version;
            var close = new Button { Content = "OK", MinWidth = 80, HorizontalAlignment = HorizontalAlignment.Right };
            close.Click += (_, _) => Close();
            Content = new StackPanel
            {
                Margin = new Thickness(16),
                Spacing = 12,
                Children =
                {
                    new TextBlock { Text = "BazisCAE", FontWeight = FontWeight.Bold },
                    new TextBlock { Text = $"{Localization.Resources.versionWordPrefix} {version?.Major}.{version?.Minor}.{version?.Build}" },
                    close
                }
            };
        }
    }
}
