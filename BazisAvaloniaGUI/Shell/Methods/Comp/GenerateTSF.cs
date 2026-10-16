using BazisAvaloniaGUI.Localization;
using PreProc;
using PreProc.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private void сформироватьИнструкцииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //var pContr = (PinnedTaskPlannerControl)EmbeddedControls.Find("pinnedTaskPlannerControl", false)[0];

                var inputDir = Path.Combine(WorkingDir, "InputData");

                if (!Directory.Exists(inputDir))
                    Directory.CreateDirectory(inputDir);

                var oldTSF = Directory.GetFiles(inputDir);
                if (oldTSF.Length > 0) Array.ForEach(oldTSF, x => File.Delete(x));

                var procProp = new ProcessProperty()
                {
                    TaskKind = project.TaskKind,
                    CommonTaskType = ProcessType.Welding // убрать из препроцессора
                };

                var data = project.GetAllCondData();
                preProc.CalcCompDataV2(data, procProp, inputDir);

                var tsfFiles = Directory.GetFiles(inputDir, "*.tsf");

                var sortedFiles = preProc.SortCompDataByTimeAndType(tsfFiles);

                PresentCompDataOnTree(sortedFiles);

                console.PrintInfo($"{Resources.GenerateTSF_InstructionsFormed_Message} {inputDir}", Color.Green);

            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }
    }
}
