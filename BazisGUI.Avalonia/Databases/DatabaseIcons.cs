using global::Avalonia.Controls;
using global::Avalonia.Media.Imaging;
using global::Avalonia.Platform;
using System;

namespace BazisGUI.Avalonia.Databases;

internal static class DatabaseIcons
{
    public static Image Create(string assetName)
    {
        using var stream = AssetLoader.Open(new Uri($"avares://BazisGUI.Avalonia/Databases/Assets/{assetName}"));
        return new Image
        {
            Source = new Bitmap(stream),
            Width = 16,
            Height = 16
        };
    }
}
