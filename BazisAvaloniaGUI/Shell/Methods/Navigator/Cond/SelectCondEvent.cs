using BazisAvaloniaGUI.Localization;
using BazisAvaloniaGUI.Properties;
using Project.Interfaces.Tasks;
using Project.Tasks;
using Project.Tasks.Materials;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        /// <summary>
        /// Показывает свойства и окрашивает группу выбранного условия, останавливая плеер.
        /// </summary>
        private void Navigator_SelectCondEvent(int arg1)
        {
            try
            {
                var data = project.GetCondData(arg1);

                var _funcs = project.FunctionsDB.Keys.ToList();
                _funcs.Add("*");
                var _mats = project.MaterialsDB.Keys.ToList();

                var groups = project.GetAllModelGroups();

                List<RowProperty> rows;

                switch (data.Kind)
                {
                    case DataKind.Материал:
                        rows = GetMatProperty((MatData)data, _mats, groups, _funcs);
                        break;

                    case DataKind.Среда:
                        rows = GetMediaProperty((MediaData)data, groups, _funcs);
                        break;

                    case DataKind.Нагрев:
                        rows = GetCondProperty((HeatData)data, groups, _funcs);
                        break;

                    case DataKind.Закрепление:
                        rows = GetClampProperty((ClampData)data, groups, _funcs);
                        break;

                    case DataKind.Нагрузка:
                        rows = GetLoadProperty((LoadData)data, _funcs, groups);
                        break;

                    default:
                        throw new NotImplementedException(Resources.UndefinedConditionTypeExc);
                }

                propertiesPanel.DrawTable(rows);

                checkPlayerControl.StopChecking();
                CheckPlayerControl_StopCheckingEvent(checkPlayerControl);
                var modelView = project;
                var color = settingsConfig.SelectGroupColor;
                var numbers = data.Group.Select(x => x.Number);
                using (modelView.BeginViewUpdate())
                {
                    modelView.ClearColor();
                    modelView.SetColor(data.Group.ObjType, numbers, color);
                }

            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }

        }
    }
}
