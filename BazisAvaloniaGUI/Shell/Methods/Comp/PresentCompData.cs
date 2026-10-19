using BazisAvaloniaGUI.Navigator;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        public void PresentCompDataOnTree(List<string> compData)
        {
            try
            {
                List<TreeNode> comp;
                var search = navigator.TrySearchNodes(NodeName.Calculations, out comp);

                if (compData.Count() != 0)
                    if(search)
                    {
                        NewMethod(compData, comp.First());
                    }
                    else
                    {
                        var rn = navigator.CreateRealNode(NodeName.Calculations);
                        //navigator.SetContextMenu(rn);
                        NewMethod(compData, rn);
                        navigator.TrySearchNodes(NodeName.Project, out List<TreeNode> prNodes);
                        prNodes[0].Nodes.Add(rn);
                    }
                else
                {
                    if (search)
                        comp.First().Remove();
                }
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }

        private void NewMethod(List<string> compData, TreeNode compNode)
        {
            navigator.BeginUpdate();
            compNode.Nodes.Clear();
            foreach (var item in compData)
            {
                //var nodeName = item.Split(' ')[0].ToEnum<NodeName>();
                var r = navigator.CreateRealNode(NodeName.Calculation, item);

                compNode.Nodes.Add(r);
            }

            navigator.EndUpdate();
            compNode.Expand();
        }
    }
}
