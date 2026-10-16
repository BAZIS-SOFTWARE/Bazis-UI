using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using DrawingColor = System.Drawing.Color;

namespace BazisAvaloniaGUI.Properties;

/// <summary>Модальный выбор цвета без зависимости от WinForms.</summary>
internal sealed class ColorSelectionDialog : Window
{
    private readonly TextBox hex = new() { Width = 110 };
    private readonly TextBox alpha = new() { Width = 48 };
    private readonly TextBox red = new() { Width = 48 };
    private readonly TextBox green = new() { Width = 48 };
    private readonly TextBox blue = new() { Width = 48 };
    private readonly Border preview = new() { Height = 46, BorderThickness = new Thickness(1), BorderBrush = Brushes.Black };
    private readonly Button accept = new() { Content = "ОК", MinWidth = 70 };
    private DrawingColor selected;
    private bool updating;

    public ColorSelectionDialog(DrawingColor current)
    {
        selected = current;
        Title = "Выбор цвета";
        Width = 330;
        SizeToContent = SizeToContent.Height;
        MinWidth = 330;
        WindowStartupLocation = WindowStartupLocation.CenterOwner;
        FontFamily = new FontFamily("Microsoft Sans Serif");
        FontSize = 11;
        Background = Brush.Parse("#F0F0F0");

        var content = new StackPanel { Margin = new Thickness(12), Spacing = 9 };
        content.Children.Add(preview);
        var hexLine = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 6 };
        hexLine.Children.Add(new TextBlock { Text = "#AARRGGBB", Width = 90, VerticalAlignment = VerticalAlignment.Center });
        hexLine.Children.Add(hex);
        content.Children.Add(hexLine);
        content.Children.Add(Channel("A", alpha));
        content.Children.Add(Channel("R", red));
        content.Children.Add(Channel("G", green));
        content.Children.Add(Channel("B", blue));

        var presets = new WrapPanel { Orientation = Orientation.Horizontal };
        foreach (var color in new[] { DrawingColor.White, DrawingColor.Black, DrawingColor.Gray,
            DrawingColor.Red, DrawingColor.Orange, DrawingColor.Yellow, DrawingColor.Green,
            DrawingColor.Cyan, DrawingColor.Blue, DrawingColor.Magenta })
        {
            var candidate = color;
            var button = new Button { Width = 26, Height = 26, Margin = new Thickness(2),
                Padding = new Thickness(2), Content = new Border { Background = PropertiesPanelControl.ColorBrush(color),
                    BorderThickness = new Thickness(1), BorderBrush = Brushes.Black } };
            button.Click += (_, _) => { selected = candidate; SyncFields(); };
            presets.Children.Add(button);
        }
        content.Children.Add(presets);

        var actions = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8,
            HorizontalAlignment = HorizontalAlignment.Right };
        accept.Click += (_, _) => Close((DrawingColor?)selected);
        var cancel = new Button { Content = "Отмена", MinWidth = 70 };
        cancel.Click += (_, _) => Close((DrawingColor?)null);
        actions.Children.Add(accept);
        actions.Children.Add(cancel);
        content.Children.Add(actions);
        Content = content;

        hex.TextChanged += (_, _) => ReadHex();
        foreach (var field in new[] { alpha, red, green, blue })
            field.TextChanged += (_, _) => ReadChannels();
        SyncFields();
    }

    private static StackPanel Channel(string name, TextBox box)
    {
        var line = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 6 };
        line.Children.Add(new TextBlock { Text = name, Width = 90, VerticalAlignment = VerticalAlignment.Center });
        line.Children.Add(box);
        return line;
    }

    private void SyncFields()
    {
        updating = true;
        hex.Text = $"#{selected.A:X2}{selected.R:X2}{selected.G:X2}{selected.B:X2}";
        alpha.Text = selected.A.ToString();
        red.Text = selected.R.ToString();
        green.Text = selected.G.ToString();
        blue.Text = selected.B.ToString();
        preview.Background = PropertiesPanelControl.ColorBrush(selected);
        accept.IsEnabled = true;
        updating = false;
    }

    private void ReadHex()
    {
        if (updating) return;
        var value = hex.Text?.TrimStart('#');
        if (value?.Length == 8 && uint.TryParse(value, System.Globalization.NumberStyles.HexNumber,
            System.Globalization.CultureInfo.InvariantCulture, out var argb))
        {
            selected = DrawingColor.FromArgb((byte)(argb >> 24), (byte)(argb >> 16),
                (byte)(argb >> 8), (byte)argb);
            SyncFields();
        }
        else accept.IsEnabled = false;
    }

    private void ReadChannels()
    {
        if (updating) return;
        if (byte.TryParse(alpha.Text, out var a) && byte.TryParse(red.Text, out var r) &&
            byte.TryParse(green.Text, out var g) && byte.TryParse(blue.Text, out var b))
        {
            selected = DrawingColor.FromArgb(a, r, g, b);
            SyncFields();
        }
        else accept.IsEnabled = false;
    }
}
