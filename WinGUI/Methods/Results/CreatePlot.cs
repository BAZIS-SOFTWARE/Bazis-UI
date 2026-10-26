using WinGUI.Navigator;
using WinGUI.Properties;
using Geometry;
using Model.Interfaces;
using ResultDB.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserControlsEx.Graph;

namespace WinGUI
{
    public partial class BaseForm
    {
        public async Task SelectContainerAsync(string message)
        {
            PressedKey = Keys.None;

            DisplayText2D(message, Color.Black, new Point2D(10, 10));
            RequestRedraw();
            await System.Threading.Tasks.Task.Run(() =>
            {
                while (true)
                {
                    if (PressedKey == Keys.E)
                        break;
                    if (PressedKey == Keys.Escape)
                    {
                        Invoke(new Action(() => console.PrintInfo(Resources.CreatePlot_SelectContainerAsync_CancelOperation_Message, Color.Black)));
                        break;
                    }
                }
            });
            DisplayText2DEvent = null;
            RequestRedraw();
            PressedKey = Keys.None;
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
                    .Select(number => project.GetModelObject(ObjType.Узел, number));

                if (nodes.Count() == 0)
                    throw new Exception(Resources.Result_BuildDiagram_NoNodesSelectedException);

                await SelectContainerAsync(Resources.CreatePlot_BuildGraph_SelectContainerAsync_SelectResult_Message);

                if (navigator.SelectedNode.Name != NodeName.Result.ToString())
                    throw new Exception(Resources.CreatePlot_BuildGraph_SelectResult_Exception);

                var selNode = navigator.SelectedNode;
                var resDes = selNode.Text;

                var loader = new LoadResultsFileDB();

                // важно, так как если режим усреднения, то будет исключение
                var tables = new List<string>(){ResultType.nodes.ToString(), ResultType.elements.ToString() };
                var times = loader.GetValues(ResultDbPath, "nodes", "Time");

                var grDataAr = new List<GraphData>();
                Random random = new Random();

                foreach (var obj in nodes)
                {
                    var grPoints = new List<GraphPoint>();

                    console.PrintInfo($"{Resources.CreatePlot_BuildGraph_BuildingGraph_Text_Part1} {obj.ObjType} {obj.Number}, {Resources.CreatePlot_BuildGraph_BuildingGraph_Text_Part2}...", Color.Orange); ;

                    foreach (var time in times)
                    {
         
                        var result = loader.GetResult(ResultDbPath, tables, time);
                        if (settingsConfig.MergeResultsValue)
                            MergeResults(result);
                        var res = result.GetValue(ResultType.nodes.ToString(), obj.Number, resDes);

                        var grPoint = new GraphPoint(result.Time, res);
                        grPoints.Add(grPoint);
                    }

                    DisplayText3D($"{Resources.CreatePlot_DisplayText3D_Text}_{obj.Number}", Color.Black, obj.CalcCentr());
                    var color = Color.FromArgb(random.Next(255), random.Next(255), random.Next(255));
                    var grData = new GraphData($"{Resources.CreatePlot_GraphData_Header_Part1}_{obj.Number}", color, Resources.CreatePlot_GraphData_XUnit, resDes, grPoints.ToArray());
                    grDataAr.Add(grData);
                }
                RequestRedraw();
                var grContainer = new GraphContainer();

                if (grDataAr.Count != 0)
                {
                    grContainer.CreateGraphData(Resources.CreatePlot_CreateGraphData_Header, grDataAr, new AxisFormat(), new AxisFormat());
                    grContainer.Dock = DockStyle.Fill;
                    var form = new Form
                    {
                        Owner = Application.OpenForms[0],
                        TopMost = true,
                        Text = $"{Resources.CreatePlot_GraphForm_Text_Part1} {resDes} - {Resources.CreatePlot_GraphForm_Text_Part2}",
                        ShowIcon = false,
                        ClientSize = grContainer.Size
                    };


                    form.Controls.Add(grContainer);
                    form.ClientSize = grContainer.Size;
                    form.Show();
                }

            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        } 
    }
}
