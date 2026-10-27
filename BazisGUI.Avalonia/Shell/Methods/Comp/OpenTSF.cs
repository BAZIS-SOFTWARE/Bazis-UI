using BazisGUI.Avalonia.Localization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace BazisGUI.Avalonia.Shell
{
    internal partial class MainWindow
    {
        /// <summary>
        /// Восстанавливает очередь инструкций через препроцессор и отображает её в навигаторе.
        /// </summary>
        private void открытьИнструкцииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(projectFilePath))
                    throw new InvalidOperationException("A project must be loaded before task files.");

                preProc.LoadInstructions(projectFilePath);

                var instructions = new List<string>();
                foreach (var parameters in preProc)
                    instructions.Add(parameters.ToString());
                PresentCompDataOnTree(instructions);
                var projectDirectory = Path.GetDirectoryName(projectFilePath)!;
                var tcfPath = Path.Combine(projectDirectory, "ComputationData", "computation.tcf");
                console.PrintInfo($"{Resources.OpenTSF_OpenInstructions_Message} {tcfPath}", Color.Green);

            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }
    }
}
