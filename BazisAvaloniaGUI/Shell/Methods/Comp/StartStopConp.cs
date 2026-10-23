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

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private Process solverProcess;

        private async void запуститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (solverProcess != null && !solverProcess.HasExited)
                    throw new InvalidOperationException("Solver is already running.");

                if (!await SaveProjectAsync())
                    return;
                console.PrintInfo($"{Resources.StartStopConp_SaveProjectInto_Message} {WorkingDir}", Color.Black);

                if (!CheckProjectDataBeforeCreationTCF())
                    return;

                var cmdFile = preProc.WriteInstructions(projectFilePath);

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
                if (solverProcess != null)
                {
                    if (!solverProcess.HasExited)
                        throw new InvalidOperationException("Solver is already running.");

                    solverProcess.Dispose();
                    solverProcess = null;
                }

                var myProcess = new Process();
                try
                {
                    myProcess.StartInfo.FileName = settingsConfig.SolverPath;

                    var cmdFile = Path.Combine(WorkingDir, "ComputationData", "computation.tcf");

                    myProcess.StartInfo.ArgumentList.Add(cmdFile);
                    myProcess.StartInfo.WorkingDirectory = WorkingDir;
                    myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                    if (!myProcess.Start())
                        throw new InvalidOperationException("Solver process did not start.");

                    solverProcess = myProcess;
                }
                catch
                {
                    myProcess.Dispose();
                    throw;
                }
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
                if (solverProcess == null || solverProcess.HasExited)
                    return;

                solverProcess.Kill(entireProcessTree: true);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }

        /// <summary>Выводит в консоль текущее состояние запущенного решателя.</summary>
        private void статусToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Panel2Collapsed = false;

                if (solverProcess == null)
                {
                    console.PrintInfo(resources.GetString("solverStatus.NotStarted"), Color.Black);
                    return;
                }

                if (!solverProcess.HasExited)
                {
                    console.PrintInfo(resources.GetString("solverStatus.Running"), Color.Green);
                    return;
                }

                var exitCode = solverProcess.ExitCode;
                var message = string.Format(resources.GetString("solverStatus.Exited"), exitCode);
                console.PrintInfo(message, exitCode == 0 ? Color.Green : Color.Red);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }
    }
}
