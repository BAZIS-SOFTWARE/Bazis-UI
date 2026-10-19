using System;
using System.Drawing;
using System.Linq;

namespace BazisAvaloniaGUI
{
    internal partial class BaseForm
    {
        private void navigator_SelectGroupEvent(int grIndex)
        {
            try
            {
                var group = project.GetModelGroup(grIndex); // закраска объектов в выделяемой группе
                // Как в исходном BaseForm: объекты группы только подкрашиваются и не становятся
                // выделением, иначе «Удалить выбранное»/«Создать группу» применяются к группе
                using (project.BeginViewUpdate())
                {
                    project.ClearSelection();
                    project.ClearColor();
                    foreach (var objType in group.Select(x => x.ObjType).Distinct())
                    {
                        var numbers = group.Where(x => x.ObjType == objType).Select(x => x.Number);
                        project.SetColor(objType, numbers, settingsConfig.SelectGroupColor);
                    }
                }

                var rows = GetGroupProperty(group);
                propertiesPanel.DrawTable(rows);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }
    }
}
