using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;

namespace BazisAvaloniaGUI.Databases;

internal static class DatabaseIcons
{
    public static Image Create(string assetName)
    {
        using var stream = AssetLoader.Open(new Uri($"avares://BazisAvaloniaGUI/Databases/Assets/{assetName}"));
        return new Image
        {
            Source = new Bitmap(stream),
            Width = 16,
            Height = 16
        };
    }
}
