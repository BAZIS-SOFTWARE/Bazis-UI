using WinGUI.Navigator;
using WinGUI.Properties;
using Geometry;
using ResultDB.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UserControlsEx.Graph;

namespace WinGUI
{
    public partial class BaseForm
    {
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

                if (navigator.SelectedNode.Name != NodeName.Time.ToString())
                    throw new Exception(Resources.Result_BuildDiagram_SelectTime_Exception);

                var selNode = navigator.SelectedNode;
                var resDes = selNode.Parent.Text;
                var time = float.Parse(selNode.Text);

                var loader = new LoadResultsFileDB();
                var tables = new List<string>() { ResultType.nodes.ToString() };

                var pathPoints = new List<Point3D>();
                var path = 0.0f;
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
                        var grPoint = new GraphPoint(path, res);
                        grPoints.Add(grPoint);
                    }
                }
                    

                if (grPoints.Count != 0)
                {
                    var grData = new GraphData(resDes, Color.Orange, Resources.Result_BuildDiagram_GraphData_XUnit, resDes, grPoints.ToArray());
                    var grContainer = new GraphContainer();

                    grContainer.CreateGraphData(Resources.Result_BuildDiagram_DistanceResultSet_Header, new List<GraphData>() { grData }, new AxisFormat(), new AxisFormat());
                    grContainer.Dock = DockStyle.Fill;
                    var form = new Form
                    {
                        Owner = Application.OpenForms[0],
                        TopMost = true,
                        Text = $"{Resources.Result_BuildDiagram_Text_Part1} {resDes} - {Resources.Result_BuildDiagram_Text_Part2}",
                        ShowIcon = false,
                        ClientSize = grContainer.Size
                    };
                    form.Controls.Add(grContainer);
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
