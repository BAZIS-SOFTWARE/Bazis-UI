using BazisGUI.Properties;
using Project.Interfaces.Tasks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BazisGUI
{
    public partial class BaseForm
    {
        /// <summary>
        /// Восстанавливает очередь и статусы инструкций из TCF загруженного проекта.
        /// </summary>
        private void открытьИнструкцииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(lblStatus.Text))
                    throw new InvalidOperationException("A project must be loaded before task files.");

                var computationDirectory = Path.Combine(WorkingDir, "ComputationData");
                using var dialog = new OpenFileDialog
                {
                    Filter = "Calculation files (*.tcf)|*.tcf",
                    InitialDirectory = Directory.Exists(computationDirectory) ? computationDirectory : WorkingDir
                };
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                var tcfPath = Path.GetFullPath(dialog.FileName);
                var tcfDirectory = Path.GetDirectoryName(tcfPath)!;
                var projectPath = Path.GetFullPath(lblStatus.Text);
                var projectRoot = Path.GetFullPath(WorkingDir).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
                var instructions = new List<string>();
                var projectLoaded = false;
                foreach (var line in File.ReadLines(tcfPath))
                {
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("\\", StringComparison.Ordinal))
                        continue;

                    if (line.StartsWith("загрузить", StringComparison.Ordinal))
                    {
                        var command = Regex.Match(line, "^загрузить проект \"([^\"]+)\"$");
                        if (!command.Success || projectLoaded)
                            throw new FormatException("Invalid project command in TCF.");
                        var referencedProject = Path.GetFullPath(command.Groups[1].Value, tcfDirectory);
                        if (!string.Equals(referencedProject, projectPath, StringComparison.OrdinalIgnoreCase))
                            throw new InvalidDataException("The TCF refers to a different project.");
                        projectLoaded = true;
                        continue;
                    }

                    var calculation = Regex.Match(line, "^расчет (\\S+) \"([^\"]+)\" (выполнить|пропустить)$");
                    if (!projectLoaded || !calculation.Success)
                        throw new FormatException("Invalid calculation command in TCF.");
                    var kindText = calculation.Groups[1].Value;
                    if (!Enum.TryParse<TaskKind>(kindText, out var kind) || !Enum.IsDefined(kind) || kindText != kind.ToString())
                        throw new FormatException("Unsupported task kind in TCF.");
                    var reference = calculation.Groups[2].Value;
                    if (Path.IsPathRooted(reference))
                        throw new InvalidDataException("The TCF task path must be relative.");
                    var taskPath = Path.GetFullPath(reference, tcfDirectory);
                    if (!taskPath.StartsWith(projectRoot, StringComparison.OrdinalIgnoreCase))
                        throw new InvalidDataException("The TCF task file is outside the loaded project.");
                    if (!File.Exists(taskPath))
                        throw new FileNotFoundException("The TCF task file is missing from the loaded project.", taskPath);
                    var fileKind = Path.GetFileNameWithoutExtension(taskPath).Split('_')[0];
                    if (Path.GetExtension(taskPath) != ".tsf" || fileKind != kindText)
                        throw new InvalidDataException("The TCF task kind does not match its TSF file.");
                    var status = calculation.Groups[3].Value;
                    instructions.Add($"{kind} {taskPath} {status}");
                }
                if (!projectLoaded || instructions.Count == 0)
                    throw new InvalidDataException("The TCF does not contain a project and calculation queue.");

                PresentCompDataOnTree(instructions);

                console.PrintInfo($"{Resources.OpenTSF_OpenInstructions_Message} {tcfPath}", Color.Green);

            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }
    }
}
