using BazisAvaloniaGUI.Localization;
using BazisAvaloniaGUI.Navigator;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private void создатьЗадачуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                List<TreeNode> tasks;
                var search = navigator.TrySearchNodes(NodeName.Task, out tasks);

                if (project == null)
                    throw new Exception(Resources.CreateTaskWithoutProjectExc);

                if (!search)
                {
                    var rn = navigator.CreateRealNode(NodeName.Task);

                    navigator.TrySearchNodes(NodeName.Project, out List<TreeNode> prNodes);
                    prNodes[0].Nodes.Add(rn);
                }

            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }
    }
}
