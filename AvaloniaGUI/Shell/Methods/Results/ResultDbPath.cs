using AvaloniaGUI.Navigator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        string ResultDbPath { get; set; } = string.Empty;// в дереве

        private void ClearResultsState()
        {
            checkPlayerControl.StopChecking();
            checkPlayerControl.StartValue = 0;
            checkPlayerControl.StopValue = 0;
            checkingConditions = false;
            ResultDbPath = string.Empty;
            resultTimes = Array.Empty<float>();
            resultsController = new PostProc.PostProcController();
            var intervals = settingsConfig.Scale_Intervals == 0 ? 2 : settingsConfig.Scale_Intervals;
            resultsController.FillRange(settingsConfig.Scale_MinValue, settingsConfig.Scale_MaxValue, intervals, settingsConfig.Scale_Precision);
            if (navigator.TrySearchNodes(NodeName.Results, out List<TreeNode> nodes))
                foreach (var node in nodes.ToArray())
                {
                    node.Nodes.Clear();
                    node.Remove();
                }
            propertiesPanel.DrawTable(new());
        }
    }
}
