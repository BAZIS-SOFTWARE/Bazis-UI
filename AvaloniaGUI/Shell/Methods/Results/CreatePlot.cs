using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using AvaloniaGUI.Databases;
using AvaloniaGUI.Localization;
using AvaloniaGUI.Navigator;
using Geometry;
using Model.Interfaces;
using ResultDB.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace AvaloniaGUI.Shell
{
    // Из GUI/Methods/Results/CreatePlot.cs и CreateDiagramm.cs.
    internal partial class MainWindow
    {
        public async Task SelectContainerAsync(string message)
        {
            var result = await AsyncMethodContainer(() => Tuple.Create(true, (object)true),
                () => console.PrintInfo(Resources.CreatePlot_SelectContainerAsync_CancelOperation_Message, Color.Black), message);
            if (result == null)
                throw new OperationCanceledException();
        }

        private async void построитьГрафикToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ClearAllDataOnScene();
                CreateVBObjects("Объекты");
                RequestRedraw();
                // выбор объектов
                await SelectContainerAsync(Resources.CreatePlot_BuildGraph_SelectContainerAsync_SelectNodes_Message);

                var nodes = project.GetSelected(ObjType.Узел)
                    .Select(number => project.GetModelObject(ObjType.Узел, number))
                    .ToList();

                if (nodes.Count == 0)
                    throw new Exception(Resources.Result_BuildDiagram_NoNodesSelectedException);

                await SelectContainerAsync(Resources.CreatePlot_BuildGraph_SelectContainerAsync_SelectResult_Message);

                if (navigator.SelectedNode?.Name != NodeName.Result.ToString())
                    throw new Exception(Resources.CreatePlot_BuildGraph_SelectResult_Exception);

                var selNode = navigator.SelectedNode;
                var resDes = selNode.Text;

                var loader = new LoadResultsFileDB();

                // важно, так как если режим усреднения, то будет исключение
                var tables = new List<string>() { ResultType.nodes.ToString(), ResultType.elements.ToString() };
                var times = loader.GetValues(ResultDbPath, "nodes", "Time");

                var grDataAr = new List<GraphData>();
                var random = new Random();

                foreach (var obj in nodes)
                {
                    var grPoints = new List<GraphPoint>();

                    console.PrintInfo($"{Resources.CreatePlot_BuildGraph_BuildingGraph_Text_Part1} {obj.ObjType} {obj.Number}, {Resources.CreatePlot_BuildGraph_BuildingGraph_Text_Part2}...", Color.Orange);

                    foreach (var time in times)
                    {
                        var result = loader.GetResult(ResultDbPath, tables, time);
                        if (settingsConfig.MergeResultsValue)
                            MergeResults(result);
                        var res = result.GetValue(ResultType.nodes.ToString(), obj.Number, resDes);

                        grPoints.Add(new GraphPoint(result.Time, res));
                    }

                    DisplayText3D($"{Resources.CreatePlot_DisplayText3D_Text}_{obj.Number}", Color.Black, obj.CalcCentr());
                    var color = Color.FromArgb(random.Next(255), random.Next(255), random.Next(255));
                    var grData = new GraphData($"{Resources.CreatePlot_GraphData_Header_Part1}_{obj.Number}", color, Resources.CreatePlot_GraphData_XUnit, resDes, grPoints.ToArray());
                    grDataAr.Add(grData);
                }
                RequestRedraw();

                if (grDataAr.Count != 0)
                    ShowGraphWindow($"{Resources.CreatePlot_GraphForm_Text_Part1} {resDes} - {Resources.CreatePlot_GraphForm_Text_Part2}",
                        Resources.CreatePlot_CreateGraphData_Header, grDataAr);
            }
            catch (OperationCanceledException)
            {
                // Отмена интерактивного выбора завершает текущую операцию.
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }

        private async void построитьДиаграммуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ClearAllDataOnScene();
                CreateVBObjects("Объекты");
                RequestRedraw();

                var objs = await CreatePathAsync();

                if (objs.Count() == 0)
                    throw new Exception(Resources.Result_BuildDiagram_NoNodesSelectedException);

                await SelectContainerAsync(Resources.Result_BuildDiagram_SelectContaimerAsync_SelectTime_Message);

                if (navigator.SelectedNode?.Name != NodeName.Time.ToString())
                    throw new Exception(Resources.Result_BuildDiagram_SelectTime_Exception);

                var selNode = navigator.SelectedNode;
                var resDes = selNode.Parent.Text;
                var time = float.Parse(selNode.Text);

                var loader = new LoadResultsFileDB();
                var tables = new List<string>() { ResultType.nodes.ToString() };

                var pathPoints = new List<Point3D>();
                var path = 0.0;
                var grPoints = new List<GraphPoint>();

                var result = loader.GetResult(ResultDbPath, tables, time);
                if (result != null)
                {
                    if (settingsConfig.MergeResultsValue)
                        MergeResults(result);

                    foreach (var obj in objs)
                    {
                        var point = obj.CalcCentr();

                        var delta = new Point3D();
                        if (pathPoints.Count > 0)
                            delta = point.Sub(pathPoints.Last());
                        path += Vector.GetVectorLength(delta);

                        pathPoints.Add(obj.CalcCentr());

                        var res = result.GetValue(ResultType.nodes.ToString(), obj.Number, resDes);
                        grPoints.Add(new GraphPoint((float)path, res));
                    }
                }

                if (grPoints.Count != 0)
                {
                    var grData = new GraphData(resDes, Color.Orange, Resources.Result_BuildDiagram_GraphData_XUnit, resDes, grPoints.ToArray());
                    ShowGraphWindow($"{Resources.Result_BuildDiagram_Text_Part1} {resDes} - {Resources.Result_BuildDiagram_Text_Part2}",
                        Resources.Result_BuildDiagram_DistanceResultSet_Header, new List<GraphData>() { grData });
                }
            }
            catch (OperationCanceledException)
            {
                // Отмена интерактивного выбора завершает текущую операцию.
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }

        /// <summary>Окно графика (в BaseForm — Form с GraphContainer).</summary>
        private void ShowGraphWindow(string title, string header, List<GraphData> data)
        {
            var grContainer = new GraphContainer();
            grContainer.CreateGraphData(header, data, new AxisFormat(), new AxisFormat());
            var form = new Window
            {
                Topmost = true,
                Title = title,
                Width = 700,
                Height = 480,
                ShowInTaskbar = false,
                FontFamily = FontFamily,
                FontSize = FontSize,
                Content = grContainer
            };
            form.Show(this);
        }
    }
}
