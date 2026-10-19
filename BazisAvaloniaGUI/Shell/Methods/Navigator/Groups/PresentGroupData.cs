using BazisAvaloniaGUI.Navigator;
using Model.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
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
                NodeName nodeName;

                if (item.ObjType == ObjType.Узел)
                    nodeName = NodeName.NodesGroup;
                else
                    nodeName = NodeName.ElementsGroup;

                var r = navigator.CreateRealNode(nodeName, $"{item.Name} {item.Count}");

                grNode.Nodes.Add(r);
            }

            navigator.EndUpdate();
            grNode.Expand();
        }
    }
}
