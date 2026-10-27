using BazisGUI.Win.Navigator;
using BazisGUI.Win.Properties;
using BazisGUI.Win.Utilities;
using Microsoft.Scripting.Hosting.Shell;
using Project.Interfaces.Tasks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        private void создатьЗадачуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                List<TreeNode> tasks;
                var search = navigator.TrySearchNodes(NodeName.Task, out tasks);

                if (!project.HasProject)
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
