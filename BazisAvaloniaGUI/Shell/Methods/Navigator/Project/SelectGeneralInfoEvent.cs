using BazisAvaloniaGUI.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private void navigator_SelectGeneralInfoEvent()
        {
            try
            {
                if (project == null)
                    return;


                List<RowProperty> rows = new List<RowProperty>();

                rows.Add(new RowProperty("Имя", project.Name, true));
                // TO DO добавить информацию для чтения
                /*
                 * Сколько модельных объектов
                 * Какая задача
                 */


                propertiesPanel.DrawTable(rows);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }
    }
}
