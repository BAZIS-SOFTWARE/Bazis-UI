using BazisAvaloniaGUI.Localization;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private void сформироватьИнструкцииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var data = project.GetAllCondData();
                var parameters = preProc.CalcCompDataV2(data, project.TaskKind, synchronizeTimeSteps: true);
                var instructions = new List<string>();
                foreach (var parameter in parameters)
                    instructions.Add(parameter.ToString());

                PresentCompDataOnTree(instructions);
                console.PrintInfo(Resources.GenerateTSF_InstructionsFormed_Message, Color.Green);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }
    }
}
