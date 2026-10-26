using Avalonia.Platform.Storage;
using AvaloniaGUI.Localization;
using System;

namespace AvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        // Avalonia: StorageProvider возвращает результат асинхронно, поэтому путь передаётся в обратный вызов.
        private async void LoadPythonFile(Action<string> loaded)
        {
            // async void: обратный вызов выполняется после await, поэтому любое его исключение
            // (ошибка загрузки скрипта, смена выделения) ловится здесь, а не роняет приложение.
            try
            {
                var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    Title = Resources.LoadPythonFile_ВыберитеPythonФайл,
                    AllowMultiple = false,
                    FileTypeFilter = [new FilePickerFileType("Python Files") { Patterns = ["*.py"] }]
                });

                loaded(files.Count != 0 ? files[0].TryGetLocalPath() ?? string.Empty : string.Empty);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, System.Drawing.Color.Red);
            }
        }
    }
}
