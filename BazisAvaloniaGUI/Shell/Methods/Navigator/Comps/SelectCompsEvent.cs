using BazisAvaloniaGUI.Navigator;
using BazisAvaloniaGUI.Localization;
using BazisAvaloniaGUI.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        // TODO добавить локализацию tasktype
        enum TaskTypePropertyKeys { все, термическая, механическая, химическая }
        private void Navigator_SelectCompsEvent()
        {
            try
            {
                var tasks = new List<string>();
                navigator.TrySearchNodes(NodeName.Calculations, out List<TreeNode> task);
                foreach (TreeNode item in task[0].Nodes)
                    tasks.Add(item.Text);

                var taskType = Enum.GetValues<TaskTypePropertyKeys>().Select(x => x.ToString()).ToList();

                if (selectInstruction == string.Empty)
                    selectInstruction = taskType[0];

                List<RowProperty> rows = new List<RowProperty>();
                rows.Add(new RowProperty(CompPropertyKeys.Type.ToString(), Resources.Header_comp_Type, new DropDownPropertyValue(selectInstruction, taskType)));

                for (var index = 0; index < tasks.Count; index++)
                {
                    var taskName = tasks[index];
                    var isExe = preProc.ElementAt(index).Status == Project.TaskParameters.TaskStatus.выполнить;
                    var name = taskName;

                    if (selectInstruction == "все")
                    {
                        rows.Add(new RowProperty(CompPropertyKeys.Execute.ToString(),$"{Resources.Execute} {name}", isExe));
                    }
                    else if (GetInstructionType(taskName) == selectInstruction)
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
