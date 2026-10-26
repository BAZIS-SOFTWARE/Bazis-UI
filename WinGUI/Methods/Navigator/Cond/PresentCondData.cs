using WinGUI.Extensions;
using WinGUI.Localization;
using WinGUI.Navigator;
using WinGUI.PropertiesPanel;
using Project.Interfaces.Tasks;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinGUI
{
    public partial class BaseForm
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
                        //navigator.SetContextMenu(rn);
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

                //var imgIndex = navigator.GetObjectImageIndex(nodeType);

                var child = navigator.CreateRealNode(nodeName, $"{Localization.Localization.GetNavigatorNodeNameLocalization(nodeName)} : {data.ToString().Split(" : ")[1]}");
                //child.ImageIndex = imgIndex;
                //child.SelectedImageIndex = imgIndex;

                //navigator.TrySearchNodes(NodeName.условия.ToString(), out List<TreeNode> nodes);
                taskNode.Nodes.Add(child);
                //navigator.SetContextMenu(child);
            }

            navigator.EndUpdate();
            taskNode.Expand();
        }
    }
}
