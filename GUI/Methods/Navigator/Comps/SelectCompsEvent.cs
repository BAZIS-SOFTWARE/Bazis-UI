using BazisGUI.Navigator;
using BazisGUI.Properties;
using BazisGUI.PropertiesPanel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BazisGUI
{
    public partial class BaseForm
    {
        // TODO добавить локализацию tasktype
        enum TaskTypePropertyKeys { все, термическая, механическая, химическая }
        private void Navigator_SelectCompsEvent()
        {
            try
            {
                var taskType = Enum.GetValues<TaskTypePropertyKeys>().Select(x => x.ToString()).ToList();

                if (selectInstruction == string.Empty)
                    selectInstruction = taskType[0];

                List<RowProperty> rows = new List<RowProperty>();
                rows.Add(new RowProperty(CompPropertyKeys.Type.ToString(), Resources.Header_comp_Type, new DropDownPropertyValue(selectInstruction, taskType)));

                foreach (var parameters in preProc)
                {
                    var isExe = parameters.Status == Project.TaskParameters.TaskStatus.выполнить;
                    var name = $"{parameters.FileStem}.tsf";

                    if (selectInstruction == "все")
                    {
                        rows.Add(new RowProperty(CompPropertyKeys.Execute.ToString(),$"{Resources.Execute} {name}", isExe));
                    }
                    else if (parameters.TaskKind.ToString() == selectInstruction)
                    {
                        rows.Add(new RowProperty(CompPropertyKeys.Execute.ToString(), $"{Resources.Execute} {name}", isExe));
                    }
                }

                propertiesPanel.DrawTable(rows);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }
    }
}
