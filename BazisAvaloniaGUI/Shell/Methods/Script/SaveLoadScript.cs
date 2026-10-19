using BazisAvaloniaGUI.Localization;
using System;
using System.Drawing;
using System.IO;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private void загрузитьgeoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var filePath = project.GetGeometryFilePath();
                var changed = Path.ChangeExtension(filePath, "gscript");

                project.LoadSMF(changed);

                console.PrintInfo($"{Resources.SaveLoadScript_GeoScript_Message} {changed} {Resources.SaveLoadScript_GeoScript_Executed_Message}", Color.Green);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }

        }

        private void сформироватьgeoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var filePath = project.GetGeometryFilePath();
                var changed = Path.ChangeExtension(filePath, "gscript");

                project.SaveSMF(changed);

                console.PrintInfo($"{Resources.SaveLoadScript_GeoScript_Message} {changed} {Resources.SaveLoadScript_GeoScript_Formed_Message}", Color.Green);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }
    }
}
