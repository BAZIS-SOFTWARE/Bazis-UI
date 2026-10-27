using global::Avalonia.Controls;
using BazisGUI.Avalonia.GantChart;
using System;
using System.Drawing;
using System.Linq;

namespace BazisGUI.Avalonia.Shell
{
    internal partial class MainWindow
    {
        private void CloseGanttChart()
        {
            foreach (var name in TabButtonsService.GetNames().ToArray())
                if (TabButtonsService.GetControl(name) is cntrГант)
                    TabButtonsService.RemoveControl(name);
            показатьНаДиаграммеToolStripMenuItem.IsChecked = false;
        }

        private void показатьНаДиаграммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var btn = sender as MenuItem;
                // CheckOnClick: MenuItem с ToggleType = CheckBox переключает IsChecked до события Click.
                var name = (string)btn.Header;
                if (btn.IsChecked)
                {
                    var ganttContol = new cntrГант();
                    ganttContol.AddConds(project.GetAllCondData());
                    TabButtonsService.AddControl(name, ganttContol);
                }

                else
                    TabButtonsService.RemoveControl(name);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }
    }
}
