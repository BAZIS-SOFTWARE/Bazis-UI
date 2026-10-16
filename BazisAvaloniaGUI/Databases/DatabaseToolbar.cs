using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using System;
using System.Collections.Generic;

namespace BazisAvaloniaGUI.Databases;

// ToolStrip's overflow keeps commands accessible without wrapping the source toolbar.
internal sealed class DatabaseToolbar : UserControl
{
    private readonly StackPanel items = new() { Orientation = Orientation.Horizontal };
    private readonly Button overflow = new() { Name = "ToolbarOverflow", Width = 16, MinWidth = 0, Padding = new Thickness(2), Margin = new Thickness(0),
        Content = new Avalonia.Controls.Shapes.Path { Data = Avalonia.Media.Geometry.Parse("M0,0 L6,0 L3,3 Z"), Fill = Brushes.Black, Width = 6, Height = 3 } };
    private readonly List<(Control control, double width, Func<Control> menu)> entries = new();

    public DatabaseToolbar()
    {
        Height = 34; Background = Brush.Parse("#DCDCDC");
        var layout = new Grid { ColumnDefinitions = new ColumnDefinitions("*,Auto"), ClipToBounds = true };
        layout.Children.Add(items); Grid.SetColumn(overflow, 1); layout.Children.Add(overflow);
        Content = layout;
        overflow.Classes.Add("database-overflow");
        ToolTip.SetTip(overflow, "More commands");
        overflow.Click += (_, _) => { UpdateOverflow(); overflow.ContextMenu?.Open(overflow); };
        SizeChanged += (_, _) => UpdateOverflow();
    }

    public void Add(Button button, double width = 27)
    {
        Add(button, width, () =>
        {
            var menu = new MenuItem { Header = ToolTip.GetTip(button) ?? button.Name, IsEnabled = button.IsEnabled };
            var image = button.Content as Image;
            if (image == null && button.Content is Grid grid)
                foreach (var child in grid.Children) if (child is Image icon) { image = icon; break; }
            if (image != null) menu.Icon = new Image { Source = image.Source, Width = 16, Height = 16 };
            if (button.ContextMenu != null)
            {
                foreach (var item in button.ContextMenu.Items)
                    if (item is MenuItem child) menu.Items.Add(CopyMenuItem(child));
            }
            else menu.Click += (_, _) => button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            return menu;
        });
    }

    private MenuItem CopyMenuItem(MenuItem source)
    {
        object CopyContent(object content) => content is Image image ? new Image { Source = image.Source, Width = image.Width, Height = image.Height } : content;
        var copy = new MenuItem { Header = CopyContent(source.Header), Icon = CopyContent(source.Icon), Name = source.Name,
            IsEnabled = source.IsEnabled, ToggleType = source.ToggleType, IsChecked = source.IsChecked, StaysOpenOnClick = source.StaysOpenOnClick };
        foreach (var item in source.Items) if (item is MenuItem child) copy.Items.Add(CopyMenuItem(child));
        copy.Click += (_, _) => { source.IsChecked = copy.IsChecked; source.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent)); };
        return copy;
    }

    public void Add(Control control, double width, Func<Control> menu)
    {
        entries.Add((control, width, menu)); items.Children.Add(control); UpdateOverflow();
    }

    private void UpdateOverflow()
    {
        if (Bounds.Width <= 0) return;
        var total = 0d;
        foreach (var entry in entries) total += entry.width;
        overflow.IsVisible = total > Bounds.Width;
        var available = Bounds.Width - (overflow.IsVisible ? overflow.Width : 0);
        var contextMenu = new ContextMenu();
        var used = 0d;
        var overflowing = false;
        foreach (var entry in entries)
        {
            overflowing |= used + entry.width > available;
            entry.control.IsVisible = !overflowing;
            if (overflowing) contextMenu.Items.Add(entry.menu());
            else used += entry.width;
        }
        overflow.ContextMenu = contextMenu;
    }
}
