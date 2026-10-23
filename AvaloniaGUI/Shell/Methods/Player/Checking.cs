using AvaloniaGUI.Extensions;
using AvaloniaGUI.Localization;
using AvaloniaGUI.Navigator;
using Project.Interfaces.Tasks;
using ResultDB.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        float conditionCheckStartTime;
        float conditionCheckStopTime;
        bool checkingConditions;

        /// <summary>
        /// Показывает все условия, активные в текущий момент проверки задачи, или кадр результатов.
        /// </summary>
        private void CheckPlayerControl_CheckingEvent(object arg1, int arg2)
        {
            try
            {
                var name = navigator.SelectedNode?.Name;

                if (name != null && name.TryToEnum(out NodeName nodeName))
                {
                    if (nodeName == NodeName.Task)
                    {
                        HideAllGeometryObjects();
                        HideAllText3D();

                        var time = Math.Min(conditionCheckStartTime + arg2, conditionCheckStopTime);
                        var modelView = project;
                        var conditions = project.GetAllCondData();
                        using (modelView.BeginViewUpdate())
                        {
                            modelView.ClearColor();
                            foreach (var data in conditions)
                            {
                                if (time < data.StartTime || time > data.StopTime)
                                    continue;

                                if (data.LocalFrame != null)
                                    DisplayMRF(time, data);

                                var group = data.Group;
                                var localFrame = data.LocalFrame;
                                if (settingsConfig.CheckCondValue && data.Function != null && localFrame != null)
                                {
                                    foreach (var modelObject in group)
                                    {
                                        var center = modelObject.CalcCentr();
                                        var position = localFrame.Frame.GetCoordsInFrame(center);
                                        data.Function["X"].SetValue(position._x);
                                        data.Function["Y"].SetValue(position._y);
                                        data.Function["Z"].SetValue(position._z);
                                        var functionValue = data.Function.CalcValue();
                                        var value = data.Value * functionValue;
                                        DisplayText3D(value.ToString(), Color.Black, center);
                                    }
                                }

                                if (data.Direction != Direction.None)
                                    DisplayDirection(time, data, group);
                                var color = GetConditionColor(data.Kind);
                                var numbers = group.Select(modelObject => modelObject.Number);
                                modelView.SetColor(group.ObjType, numbers, color);
                            }
                        }
                        RequestRedraw();
                    }
                    else if (nodeName == NodeName.Result)
                    {
                        var loader = new LoadResultsFileDB();

                        var times = resultTimes.ToArray();
                        var tables = new List<string>()
                        {
                            ResultType.nodes.ToString(),
                            ResultType.elements.ToString()
                        };
                        var resName = navigator.SelectedNode.Text;

                        var res = loader.GetResult(ResultDbPath, tables, times[arg2]);
                        ShowResults(res, resName);
                    }
                }
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
                // Ошибка повторилась бы на каждом тике таймера: останавливаем плеер
                // (на следующем тике PlayerControl вызовет StopChecking и StopCheckingEvent).
                checkPlayerControl.Cancelation = true;
            }
        }

        /// <summary>
        /// Очищает обозначения и окраску завершённой проверки условий.
        /// </summary>
        private void CheckPlayerControl_StopCheckingEvent(object obj)
        {
            HideAllGeometryObjects();
            HideAllText3D();
            if (checkingConditions)
            {
                checkingConditions = false;
                project?.ClearColor();
            }
            RequestRedraw();
        }

        /// <summary>
        /// Разрешает проверку задачи целиком и просмотр результатов.
        /// </summary>
        private void CheckPlayerControl_StartCheckingEvent(object obj)
        {
            var name = navigator.SelectedNode?.Name;

            if (name == null || !name.TryToEnum(out NodeName nodeName))
            {
                checkPlayerControl.Cancelation = true;
                console.PrintInfo(Resources.Checking_StartCheckingEvent_SelectedDataIsNotCheckable_Message, Color.Orange);
                return;
            }

            checkingConditions = nodeName == NodeName.Task;
            if (nodeName != NodeName.Result && (!checkingConditions || !project.GetAllCondData().Any()))
            {
                checkPlayerControl.Cancelation = true;
                console.PrintInfo(Resources.Checking_StartCheckingEvent_SelectedDataIsNotCheckable_Message, Color.Orange);
            }
        }
    }
}
