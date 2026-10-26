using Avalonia.Platform.Storage;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace AvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private async void btnMakeScreenShot_Click()
        {
            try
            {
                await MakeScreenShotAsync();
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }

        private async Task MakeScreenShotAsync()
        {
            string path;
            if (!string.IsNullOrEmpty(projectFilePath))
                path = Path.Combine(WorkingDir, "screenShot.png");
            else
            {
                var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
                {
                    SuggestedFileName = "screenShot.png",
                    DefaultExtension = "png",
                    FileTypeChoices = FileTypes("PNG (*.png)|*.png")
                });
                if (file == null)
                    return;

                path = file.TryGetLocalPath();
                if (string.IsNullOrWhiteSpace(path))
                    return;
                if (!string.Equals(Path.GetExtension(path), ".png", StringComparison.OrdinalIgnoreCase))
                    path += ".png";
            }

            scene.Surface.RequestScreenShot(path);
        }
    }
}
