using BazisGUI.Win.PropertiesPanel;
using BazisGUI.Win.Utilities;
using Project.Interfaces.Tasks;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        private void navigator_SelectGeneralInfoEvent()
        {
            try
            {
                if (!project.HasProject)
                    return;

                
                List<RowProperty> rows = new List<RowProperty>();

                    rows.Add(new RowProperty("Имя", project.Name,true));
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
