using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Layout;
using Avalonia.Media;
using System.Threading.Tasks;

namespace AvaloniaGUI
{
    internal enum MessageBoxButtons { OK, YesNo, OKCancel }
    internal enum DialogResult { None, OK, Yes, No, Cancel }

    /// <summary>
    /// Avalonia-аналог System.Windows.Forms.MessageBox. Окно модальное, но вызов асинхронный.
    /// </summary>
    internal static class MessageBox
    {
        public static Task<DialogResult> Show(Visual owner, string text, string caption = null, MessageBoxButtons buttons = MessageBoxButtons.OK)
        {
            var actions = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, HorizontalAlignment = HorizontalAlignment.Right };
            var dialog = new Window
            {
                Title = caption ?? string.Empty,
                Width = 420,
                MinHeight = 130,
                SizeToContent = SizeToContent.Height,
                CanResize = false,
                FontFamily = new FontFamily("Microsoft Sans Serif"),
                FontSize = 11,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Content = new StackPanel
                {
                    Margin = new Thickness(16),
                    Spacing = 16,
                    Children = { new SelectableTextBlock { Text = text, TextWrapping = TextWrapping.Wrap }, actions }
                }
            };
            if (buttons == MessageBoxButtons.YesNo)
            {
                actions.Children.Add(Button("Да", () => dialog.Close(DialogResult.Yes)));
                actions.Children.Add(Button("Нет", () => dialog.Close(DialogResult.No)));
            }
            else if (buttons == MessageBoxButtons.OKCancel)
            {
                actions.Children.Add(Button("OK", () => dialog.Close(DialogResult.OK)));
                actions.Children.Add(Button("Отмена", () => dialog.Close(DialogResult.Cancel)));
            }
            else
                actions.Children.Add(Button("OK", () => dialog.Close(DialogResult.OK)));

            // Контрол ещё не (или уже не) в окне, либо его окно скрыто — владельцем становится главное окно
            // приложения: иначе сообщение открывалось немодальным, а Yes/No-вызовы сразу получали None
            // (ShowDialog со скрытым владельцем к тому же бросает исключение).
            var window = owner == null ? null : TopLevel.GetTopLevel(owner) as Window;
            if (window == null || !window.IsVisible)
                window = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
            if (window != null && window.IsVisible)
                return dialog.ShowDialog<DialogResult>(window);

            // Видимых окон нет (запуск/закрытие приложения): сообщение показывается без ожидания ответа.
            dialog.Show();
            return Task.FromResult(DialogResult.None);
        }

        private static Button Button(string text, System.Action click)
        {
            var button = new Button { Content = text, MinWidth = 80 };
            button.Click += (_, _) => click();
            return button;
        }
    }
}
