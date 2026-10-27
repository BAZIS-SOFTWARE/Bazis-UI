using BazisGUI.Win.Navigator;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Windows.Forms;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        public void PresentGroupDataOnTree()
        {
            List<TreeNode> nodes;
            var search = navigator.TrySearchNodes(NodeName.Groups, out nodes);

            if (project.GetAllModelGroups().Count() != 0)
                if (search)
                {
                    PresentGroups(nodes.First());
                }
                else
                {
                    var rn = navigator.CreateRealNode(NodeName.Groups);
                    //navigator.SetContextMenu(rn);
                    PresentGroups(rn);
                    navigator.TrySearchNodes(NodeName.Project, out List<TreeNode> prNodes);
                    prNodes[0].Nodes.Add(rn);
                }
            else
            {
                if (search)
                    nodes.First().Remove();
            }
        }

        private void PresentGroups(TreeNode grNode)
        {
            navigator.BeginUpdate();
            grNode.Nodes.Clear();

            foreach (var item in project.GetAllModelGroups())
            {
                //var nodeName = Converters.ConvertToNavigatorNodeType(item.ObjType);
                NodeName nodeName;

                if (item.ObjType == ObjType.Узел)
                    nodeName = NodeName.NodesGroup;
                else
                    nodeName = NodeName.ElementsGroup;

                var r = navigator.CreateRealNode(nodeName, $"{item.Name} {item.Count}");
                //var ind = navigator.GetObjectImageIndex(nodeName);
                //r.ImageIndex = ind;
                //r.SelectedImageIndex = ind;

                grNode.Nodes.Add(r);
                //navigator.SetContextMenu(r);
            }

            navigator.EndUpdate();
            grNode.Expand();
        }
    }
}
