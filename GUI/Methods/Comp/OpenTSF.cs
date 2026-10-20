using BazisGUI.Properties;
using System;
using System.Drawing;
using System.IO;

namespace BazisGUI
{
    public partial class BaseForm
    {
        /// <summary>
        /// Восстанавливает очередь инструкций через препроцессор и отображает её в навигаторе.
        /// </summary>
        private void открытьИнструкцииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(lblStatus.Text))
                    throw new InvalidOperationException("A project must be loaded before task files.");

                preProc.LoadInstructions(lblStatus.Text);
                PresentCompDataOnTree();

                var projectDirectory = Path.GetDirectoryName(lblStatus.Text)!;
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
