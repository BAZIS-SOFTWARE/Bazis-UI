using Avalonia.Platform.Storage;
using BazisAvaloniaGUI.Localization;
using PreProc;
using PreProc.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private async void открытьИнструкцииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var dialog = await StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions { AllowMultiple = false });

                if (dialog.Count == 0)
                    return;

                var inputDir = $@"{dialog[0].TryGetLocalPath()}";
                var tsfFiles = Directory.GetFiles(inputDir, "*.tsf");

                var sortedFiles = preProc.SortCompDataByTimeAndType(tsfFiles);

                PresentCompDataOnTree(sortedFiles);

                console.PrintInfo($"{Resources.OpenTSF_OpenInstructions_Message} {inputDir}", Color.Green);

            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }
    }
}
