using BazisAvaloniaGUI.Navigator;
using Project.Interfaces.Tasks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        public void PresentCondDataOnTree()
        {
            try
            {
                List<TreeNode> tasks;
                var search = navigator.TrySearchNodes(NodeName.Task, out tasks);

                if (project.GetAllCondData().Count() != 0)
                    if (search)
                    {
                        PresentConds(tasks.First());
                    }
                    else
                    {
                        var rn = navigator.CreateRealNode(NodeName.Task);
                        PresentConds(rn);
                        navigator.TrySearchNodes(NodeName.Project, out List<TreeNode> prNodes);
                        prNodes[0].Nodes.Add(rn);
                    }
                else
                {
                    if (search)
                        tasks.First().Remove();
                }
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }

        private void PresentConds(TreeNode taskNode)
        {
            navigator.BeginUpdate();
            taskNode.Nodes.Clear();

            foreach (var data in project.GetAllCondData())
            {
                NodeName nodeName;

                if (data.Kind == DataKind.Материал)
                    nodeName = NodeName.Material;
                else if (data.Kind == DataKind.Среда)
                    nodeName = NodeName.Media;
                else if (data.Kind == DataKind.Нагрев)
                    nodeName = NodeName.Heat;
                else if (data.Kind == DataKind.Закрепление)
                    nodeName = NodeName.Clamp;
                else
                    nodeName = NodeName.Load;

                var child = navigator.CreateRealNode(nodeName, $"{Localization.Localization.GetNavigatorNodeNameLocalization(nodeName)} : {data.ToString().Split(" : ")[1]}");

                taskNode.Nodes.Add(child);
            }

            navigator.EndUpdate();
            taskNode.Expand();
        }
    }
}
