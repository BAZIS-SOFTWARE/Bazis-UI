using AvaloniaGUI.Localization;
using AvaloniaGUI.Navigator;
using AvaloniaGUI.Properties;
using ResultDB;
using ResultDB.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AvaloniaGUI.Shell
{
    // Из GUI/Methods/Navigator/Results/SelectResultsEvent.cs, Result/SelectResultEvent.cs,
    // Time/SelectTimeEvent.cs и обработчиков результатов из NavigatorEvents.cs.
    internal partial class MainWindow
    {
        enum ResultPropertyKeys { Result, ShowFields, ShowNodesValues, ShowElementsValues, MergeResultsValues, ShowScale, ResultScale, ClarifyValues, MaxScaleValue, MinScaleValue, ScalePrecision, ScaleIntervals, ScaleXPos, ScaleYPos }

        private void navigator_SelectResultsEvent()
        {
            var rows = GetResultsProperties();
            propertiesPanel.DrawTable(rows);
        }

        public List<RowProperty> GetResultsProperties()
        {
            List<RowProperty> rows = new List<RowProperty>
            {
                new RowProperty(ResultPropertyKeys.ShowFields.ToString(), Resources.Header_result_showFields, settingsConfig.ShowResultsField),
                new RowProperty(ResultPropertyKeys.ShowNodesValues.ToString(), Resources.Header_result_showNodesValues, settingsConfig.ShowNodeResultsValue),
                new RowProperty(ResultPropertyKeys.ShowElementsValues.ToString(), Resources.Header_result_showElementsValues, settingsConfig.ShowElementsResultsValue),
                new RowProperty(ResultPropertyKeys.MergeResultsValues.ToString(), Resources.Header_result_averageValues, settingsConfig.MergeResultsValue),
                new RowProperty(ResultPropertyKeys.ShowScale.ToString(), Resources.Header_result_showScale, settingsConfig.ShowResultsScale)
            };

            if (settingsConfig.ShowResultsScale)
            {
                rows.Add(new RowProperty(ResultPropertyKeys.ResultScale.ToString(), Resources.Header_result_resultScale, settingsConfig.Scale_scale));
                rows.Add(new RowProperty(ResultPropertyKeys.ClarifyValues.ToString(), Resources.Header_result_clarifyValues, settingsConfig.IsScaleMaxMinManual));

                if (settingsConfig.IsScaleMaxMinManual)
                {
                    rows.Add(new RowProperty(ResultPropertyKeys.MaxScaleValue.ToString(), Resources.Header_result_maxScaleValue, settingsConfig.Scale_MaxValue));
                    rows.Add(new RowProperty(ResultPropertyKeys.MinScaleValue.ToString(), Resources.Header_result_minScaleValue, settingsConfig.Scale_MinValue));
                }

                rows.Add(new RowProperty(ResultPropertyKeys.ScalePrecision.ToString(), Resources.Header_result_precision,
                    new NumericUpDownValue(settingsConfig.Scale_Precision, 0, 15, 0, 1)));

                rows.Add(new RowProperty(ResultPropertyKeys.ScaleIntervals.ToString(), Resources.Header_result_intervals,
                    new NumericUpDownValue(settingsConfig.Scale_Intervals, 2, 10, 0, 1)));

                rows.Add(new RowProperty(ResultPropertyKeys.ScaleXPos.ToString(), Resources.Header_result_xPos,
                    new NumericUpDownValue(settingsConfig.Scale_X_Coord, 0, 2000, 0, 1)));
                rows.Add(new RowProperty(ResultPropertyKeys.ScaleYPos.ToString(), Resources.Header_result_yPos,
                    new NumericUpDownValue(settingsConfig.Scale_Y_Coord, 0, 2000, 0, 1)));
            }

            return rows;
        }

        private void navigator_SelectResultEvent(string arg2)
        {
            try
            {
                checkPlayerControl.StopChecking();
                CheckPlayerControl_StopCheckingEvent(checkPlayerControl);
                var row = new RowProperty(ResultPropertyKeys.Result.ToString(), Resources.Header_result_result, arg2);
                propertiesPanel.DrawTable(new List<RowProperty>() { row });

                var loader = new LoadResultsFileDB();
                var times = loader.GetValues($@"{ResultDbPath}", "nodes", "Time");
                checkPlayerControl.StartValue = 0;
                // Пустая БД результатов: StopValue = -1 < StartValue бросил бы исключение
                checkPlayerControl.StopValue = Math.Max(0, times.Count() - 1);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }

        private void navigator_SelectTimeEvent(string arg1, double arg2)
        {
            var loader = new LoadResultsFileDB();
            var tables = new List<string>()
            {
                ResultType.nodes.ToString(),
                ResultType.elements.ToString()
            };
            var res = loader.GetResult(ResultDbPath, tables, (float)arg2);
            ShowResults(res, arg1);
        }

        public void ShowResults(Result result, string resName)
        {
            try
            {
                var tableName = ResultType.nodes.ToString();

                if (settingsConfig.MergeResultsValue)
                    MergeResults(result);

                if (settingsConfig.ShowResultsField)
                {
                    if (!settingsConfig.IsScaleMaxMinManual)
                    {
                        var res = GetMaxMin(result, tableName, resName);
                        var intervals = settingsConfig.Scale_Intervals;
                        var pre = settingsConfig.Scale_Precision;
                        resultsController.FillRange(res.Item2, res.Item1, intervals, pre);
                    }

                    ClearAllGeometryDataOnScene();
                    ClearAllMeshDataOnScene();

                    if (settingsConfig.ShowResultsScale)
                    {
                        HideGeometryObj("DisplaySceneScale");
                        var title = result.Name;
                        var info = $"{resName} {result.Time}";
                        DisplaySceneScale(title, info);
                    }

                    PresentResultsField(result, resName, tableName);
                }

                HideAllText3D();

                if (settingsConfig.ShowNodeResultsValue)
                {
                    ShowResultValue(ResultType.nodes, resName, result);
                }

                if (settingsConfig.ShowElementsResultsValue)
                {
                    ShowResultValue(ResultType.elements, resName, result);
                }

                RequestRedraw();
            }
            catch (Exception ex)
            {
                console.PrintInfo(Localization.Localization.GetErrorWithSource(ex), Color.Red);
            }
        }

        private void navigator_RemoveResultsEvent()
        {
            try
            {
                ClearResultsState();

                ClearAllDataOnScene();
                // В BaseForm представления создавались без добавления на сцену (CreateVBObject без AddVbo),
                // и модель после удаления результатов не возвращалась; здесь буферы модели пересоздаются.
                CreateVBObjects("Объекты");

                RequestRedraw();
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }

        private void navigator_HideResultsEvent()
        {
            try
            {
                ClearAllDataOnScene();

                CreateVBObjects("Объекты");

                FitObjectsToScreen();
                RequestRedraw();
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }

        private void navigator_GetResultInfoEvent(TreeNode node)
        {
            var times = resultTimes.Select(x => x.ToString());

            var childs = navigator.CreateRealNodes(NodeName.Time, times);
            node.Nodes.AddRange(childs);
        }
    }
}
