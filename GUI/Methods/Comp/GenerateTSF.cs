using BazisGUI.Properties;
using Newtonsoft.Json;
using PreProc;
using PreProc.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace BazisGUI
{
    public partial class BaseForm
    {
        private void сформироватьИнструкцииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //var pContr = (PinnedTaskPlannerControl)EmbeddedControls.Find("pinnedTaskPlannerControl", false)[0];

                var inputDir = Path.Combine(WorkingDir, "InputData");

                if (!Directory.Exists(inputDir))
                    Directory.CreateDirectory(inputDir);

                var data = project.GetAllCondData();
                var parameters = preProc.CalcCompDataV2(data, project.TaskKind, synchronizeTimeSteps: true);
                var serializer = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    Formatting = Newtonsoft.Json.Formatting.Indented
                };
                var files = new Dictionary<string, string>();
                foreach (var parameter in parameters)
                {
                    var path = Path.Combine(inputDir, $"{parameter.FileStem}.tsf");
                    var contents = JsonConvert.SerializeObject(parameter, serializer);
                    files.Add(path, contents);
                }
                var oldTasks = Directory.GetFiles(inputDir, "*.tsf");
                foreach (var oldTask in oldTasks)
                    File.Delete(oldTask);
                foreach (var file in files)
                    File.WriteAllText(file.Key, file.Value, new UTF8Encoding(false));

                PresentCompDataOnTree();

                console.PrintInfo($"{Resources.GenerateTSF_InstructionsFormed_Message} {inputDir}", Color.Green);

            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }
    }
}
