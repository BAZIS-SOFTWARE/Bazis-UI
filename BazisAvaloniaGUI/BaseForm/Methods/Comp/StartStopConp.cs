using BazisAvaloniaGUI.Navigator;
using BazisAvaloniaGUI.Localization;
using PreProc;
using PreProc.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazisAvaloniaGUI
{
    internal partial class BaseForm
    {
        private async void запуститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!await SaveProjectAsync())
                    return;
                console.PrintInfo($"{Resources.StartStopConp_SaveProjectInto_Message} {WorkingDir}", Color.Black);

                if (!CheckProjectDataBeforeCreationTCF())
                    return;

                var compDir = Path.Combine(WorkingDir, "ComputationData");

                if (!Directory.Exists(compDir))
                    Directory.CreateDirectory(compDir);

                var result = new List<string>
            {
                $@"\\загрузка сетки и данных",
                $@"загрузить проект {projectFilePath}",
                /*
                $@"\\загрузка материалов",
                $@"загрузить материалы {project.Path}\{project.MaterialsDB}",
                $@"\\загрузка функций",
                $@"загрузить функции {project.Path}\{project.FunctionsDB}",
                */
                $@"\\расчет"
            };

                var tasks = new List<string>();
                navigator.TrySearchNodes(NodeName.Calculations, out List<TreeNode> task);
                foreach (TreeNode item in task[0].Nodes)
                    tasks.Add("расчет " + item.Text);

                result.AddRange(tasks);

                var cmdFile = Path.Combine(compDir, "computation.tcf");

                File.WriteAllLines(cmdFile, result);

                console.PrintInfo($"{Resources.StartStopConp_FormCommandFile_Message} {cmdFile}", Color.Green);

                StartComputation();
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }

        /// <summary>Проверяет наличие файла проекта и баз данных; при ошибке расчёт не запускается.</summary>
        private bool CheckProjectDataBeforeCreationTCF()
        {
            try
            {
                if (!File.Exists(projectFilePath))
                    throw new Exception($"{Resources.StartStopConp_ProjectDirectoryCheck_Message_Part1}: {WorkingDir} {Resources.StartStopConp_ProjectDataCheck_LackOfProjFile_Message}: {project.Name}.");

                var mat = Path.Combine(WorkingDir, project.MaterialsDB.Name);
                if (!File.Exists(mat))
                    throw new Exception($"{Resources.StartStopConp_ProjectDirectoryCheck_Message_Part1}: {WorkingDir} {Resources.StartStopConp_ProjectDataCheck_LackOfMaterials_Message}: {project.MaterialsDB.Name}.");

                var func = Path.Combine(WorkingDir, project.FunctionsDB.Name);
                if (!File.Exists(func))
                    throw new Exception($"{Resources.StartStopConp_ProjectDirectoryCheck_Message_Part1}: {WorkingDir} {Resources.StartStopConp_ProjectDataCheck_LackOfFunctions_Message}: {project.FunctionsDB.Name}.");

                return true;
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
                return false;
            }
        }

        public void StartComputation()
        {
            try
            {
                var myProcess = new Process();

                myProcess.StartInfo.FileName = settingsConfig.SolverPath;

                var cmdFile = Path.Combine(WorkingDir, "ComputationData", "computation.tcf");

                myProcess.StartInfo.ArgumentList.Add(cmdFile);
                myProcess.StartInfo.WorkingDirectory = WorkingDir;
                myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                myProcess.Start();
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }


        private void остановитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var runProc = Process.GetProcessesByName("BazisSolverCP");

                if (runProc.Length != 0)
                {
                    if (!OperatingSystem.IsWindows())
                    {
                        runProc[0].Kill();
                        return;
                    }

                    var process = new Process();
                    var startInfo = new ProcessStartInfo
                    {
                        WindowStyle = ProcessWindowStyle.Hidden,
                        FileName = "cmd.exe",
                        //Arguments = $"/C sc stop BazisSolver",
                        Arguments = $"/C taskkill /pid {runProc[0].Id} /f",
                        Verb = "runas"
                    };
                    process.StartInfo = startInfo;
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                // Процесс уже завершился или отказано в повышении прав
                console.PrintInfo(ex.Message, Color.Red);
            }
        }
    }
}
