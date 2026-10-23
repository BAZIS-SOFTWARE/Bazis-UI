using WinGUI.Navigator;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinGUI
{
    public partial class BaseForm
    {
        public void PresentCompDataOnTree()
        {
            try
            {
                List<TreeNode> comp;
                var search = navigator.TrySearchNodes(NodeName.Calculations, out comp);

                if (preProc.Any())
                    if(search)
                    {
                        NewMethod(comp.First());
                    }
                    else
                    {
                        var rn = navigator.CreateRealNode(NodeName.Calculations);
                        //navigator.SetContextMenu(rn);
                        NewMethod(rn);
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

        private void NewMethod(TreeNode compNode)
        {
            navigator.BeginUpdate();
            compNode.Nodes.Clear();
            foreach (var parameters in preProc)
            {
                var r = navigator.CreateRealNode(NodeName.Calculation, parameters.ToString());

                compNode.Nodes.Add(r);
            }

            navigator.EndUpdate();
            compNode.Expand();
        }
    }
}
