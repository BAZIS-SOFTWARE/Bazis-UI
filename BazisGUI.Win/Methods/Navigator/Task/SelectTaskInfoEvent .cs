using BazisGUI.Win.Properties;
using BazisGUI.Win.PropertiesPanel;
using BazisGUI.Win.Utilities;
using Project.Interfaces.Tasks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        enum TaskPropertyKeys { Type, Kind, Materials, Functions, CheckCondValues }
        /// <summary>
        /// Показывает свойства задачи и настраивает общий диапазон проверки условий.
        /// </summary>
        private void navigator_SelectTaskEvent()
        {
            try
            {
                if (!project.HasProject)
                    return;

                checkPlayerControl.StopChecking();
                CheckPlayerControl_StopCheckingEvent(checkPlayerControl);
                var conditions = project.GetAllCondData().ToList();
                conditionCheckStartTime = conditions.Count == 0 ? 0 : conditions.Min(data => data.StartTime);
                conditionCheckStopTime = conditions.Count == 0 ? 0 : conditions.Max(data => data.StopTime);
                var duration = Math.Max(0, conditionCheckStopTime - conditionCheckStartTime);
                var steps = Math.Ceiling(duration);
                checkPlayerControl.StartValue = 0;
                checkPlayerControl.StopValue = checked((int)steps);
                checkPlayerControl.CurrentValue = 0;

                List<RowProperty> rows = new List<RowProperty>();

                var type = Converters.GetEnumNames<TaskType>();

                rows.Add(new RowProperty(TaskPropertyKeys.Type.ToString(),
                    Resources.Header_task_type,
                    new DropDownPropertyValue(project.TaskType, type)));

                rows.Add(new RowProperty(string.Empty,
                    Properties.Resources.Headers_task_kind, string.Empty, true));
                foreach (var taskKind in Enum.GetValues<TaskKind>()) 
                {
                    rows.Add(new RowProperty(TaskPropertyKeys.Kind.ToString(),
                        Indent(2, Converters.GetDisplayName(taskKind)),
                        project.TaskKind.HasFlag(taskKind)));
                }

                if(project.MaterialsDB != null)
                    rows.Add(new RowProperty(TaskPropertyKeys.Materials.ToString(),
                        Resources.Header_task_materials,
                        project.MaterialsDB.Name,true));

                if (project.FunctionsDB != null)
                    rows.Add(new RowProperty(TaskPropertyKeys.Functions.ToString(),
                        Resources.Header_task_functions,
                        project.FunctionsDB.Name,true));

                rows.Add(new RowProperty(TaskPropertyKeys.CheckCondValues.ToString(),
                    Resources.Header_task_checkCondValues, settingsConfig.CheckCondValue));
                
                propertiesPanel.DrawTable(rows);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }
    }
}
