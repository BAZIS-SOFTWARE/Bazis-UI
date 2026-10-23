using AvaloniaGUI.Extensions;
using AvaloniaGUI.Navigator;
using AvaloniaGUI.Properties;
using AvaloniaGUI.Utilities;
using Project.Interfaces.Tasks;
using System;
using System.Collections.Generic;

namespace AvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private void ChangeTaskProperties(PropertyChangedEventArgs obj)
        {
            var clearFlag = false;
            if (Enum.TryParse(obj.Key, out TaskPropertyKeys key))
            {
                switch (key)
                {
                    case TaskPropertyKeys.Type:
                        project.ChangeTaskType(obj.NewValue.ToEnum<TaskType>());
                        clearFlag = true;
                        break;

                    case TaskPropertyKeys.Kind:
                        var taskKind = Converters.GetTaskKind(obj.LocalizedHeader.TrimStart());
                        var updatedProjectKind = bool.Parse(obj.NewValue)
                            ? project.TaskKind | taskKind
                            : project.TaskKind & ~taskKind;

                        if (updatedProjectKind == 0)
                        {
                            navigator_SelectTaskEvent();
                            break;
                        }

                        project.ChangeTaskKind(updatedProjectKind);
                        clearFlag = true;
                        break;

                    case TaskPropertyKeys.CheckCondValues:
                        settingsConfig.CheckCondValue = bool.Parse(obj.NewValue);
                        break;
                }

                if (clearFlag)
                    if (navigator.TrySearchNodes(NodeName.Task, out List<TreeNode> tasks) && tasks.Count > 0)
                        tasks[0].Nodes.Clear();
            }
        }
    }
}
