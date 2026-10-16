using Avalonia.Platform.Storage;
using Avalonia.Threading;
using BazisAvaloniaGUI.Localization;
using Model.Interfaces;
using Model.Interfaces.MeshObjects;
using Project.Interfaces.Tasks;
using BazisAvaloniaGUI.Navigator;
using ResultDB;
using ResultDB.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BazisAvaloniaGUI.Shell
{
    // В BaseForm объявлен в GUI/Methods/Navigator/NavigatorMethods.cs.
    enum ResultType { nodes, elements }

    internal partial class MainWindow
    {
        IEnumerable<float> resultTimes;

        /// <summary>
        /// Обработчик нажатия пункта меню "Объединить БД результатов".
        /// Позволяет выбрать несколько файлов .db и последовательно объединяет их в один.
        /// </summary>
        private async void MergeDataBase_Click(object sender, EventArgs e)
        {
            var loader = new LoadResultsFileDB();
            var currentResultPath = string.Empty;
            string[] paths = [];

            try
            {
                var openDialog = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    Title = "Выберите файлы БД результатов",
                    FileTypeFilter = FileTypes("Results files (*.db)|*.db"),
                    AllowMultiple = true,
                });

                if (openDialog.Count == 0)
                    return;

                paths = openDialog.Select(file => file.TryGetLocalPath()).ToArray();
                if (paths.Length < 2 || paths.Any(string.IsNullOrEmpty))
                {
                    console.PrintInfo("Необходимо выбрать минимум два файла", Color.Orange);
                    return;
                }

                currentResultPath = loader.Merge(paths[0], paths[1]);

                for (int i = 2; i < paths.Length; i++)
                {
                    var nextPath = paths[i];
                    var newResultPath = loader.Merge(currentResultPath, nextPath);
                    TryDeleteFile(currentResultPath);
                    currentResultPath = newResultPath;
                    console.PrintInfo($"Объединено файлов {i + 1} из {paths.Length}", Color.Black);
                }

                console.PrintInfo("Объединение файлов завершено", Color.Green);
            }
            catch (Exception ex)
            {
                console.PrintInfo($"Ошибка при объединении файлов: {ex.Message}", Color.Red);
                // Промежуточный результат (создан Merge, а не выбран пользователем) не оставляем на диске.
                if (!string.IsNullOrEmpty(currentResultPath))
                    TryDeleteFile(currentResultPath);
            }

            /// <summary>
            /// Пытается удалить файл по указанному пути. Выполняет сборку мусора перед удалением,
            /// чтобы освободить возможные блокировки файла.
            /// </summary>
            void TryDeleteFile(string path)
            {
                try
                {
                    // Имя результата Merge строится из типа и времён и может совпасть с исходным файлом — его не трогаем.
                    if (paths.Any(source => string.Equals(Path.GetFullPath(source), Path.GetFullPath(path), StringComparison.OrdinalIgnoreCase)))
                        return;

                    if (File.Exists(path))
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        File.Delete(path);
                    }
                }
                catch (Exception ex)
                {
                    console.PrintInfo($"Не удалось удалить временный файл: {path}\n{ex.Message}", Color.Red);
                }
            }
        }

        private async void открытьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                var openDialog = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    AllowMultiple = false,
                    SuggestedStartLocation = await StorageProvider.TryGetFolderFromPathAsync(AppContext.BaseDirectory),
                    FileTypeFilter = FileTypes("Results files (*.db)|*.db")
                });
                if (openDialog.Count == 0)
                    return;

                await OpenResultsAsync(openDialog[0].TryGetLocalPath());
            }
            catch (Exception ex)
            {
                await MessageBox.Show(this, Localization.Localization.GetErrorWithStackMessage(ex), Localization.Localization.GetErrorCaption());
            }
        }

        private async Task OpenResultsAsync(string filePath)
        {
            try
            {
                FillingResultsData(filePath);
            }
            catch (Exception ex)
            {
                await MessageBox.Show(this, Localization.Localization.GetErrorWithStackMessage(ex), Localization.Localization.GetErrorCaption());
            }
        }

        private void FillingResultsData(string filePath)
        {
            filePath = Path.GetFullPath(filePath);
            if (!File.Exists(filePath))
                throw new FileNotFoundException(null, filePath);
            var loader = new LoadResultsFileDB();
            var scheme = loader.GetTablesSchemes(filePath).
                FirstOrDefault(x => x.Key == ResultType.nodes.ToString());
            if (scheme.Value == null)
                throw new InvalidDataException(Resources.Results_InvalidDatabase_Message);

            var times = loader.GetValues(filePath, scheme.Key, "Time").ToArray();
            if (times.Length == 0)
                throw new InvalidDataException(Resources.Results_InvalidDatabase_Message);
            var children = new List<TreeNode>();
            foreach (var desc in scheme.Value)
            {
                var rn = navigator.CreateRealNode(NodeName.Result, $"{desc}");
                rn.Nodes.Add(navigator.CreateVirtualNode(NodeName.Result));
                children.Add(rn);
            }

            List<TreeNode> results;

            if (!navigator.TrySearchNodes(NodeName.Results, out results))
            {
                var rn = navigator.CreateRealNode(NodeName.Results);

                navigator.TrySearchNodes(NodeName.Project, out List<TreeNode> prNodes);
                prNodes[0].Nodes.Add(rn);
                results.Add(rn);
            }
            else
                results[0].Nodes.Clear();

            checkPlayerControl.StopChecking();
            results[0].Nodes.AddRange(children.ToArray());
            ResultDbPath = filePath;
            resultTimes = times;
        }

        private void PresentResultsField(Result result, string resName, string tableName)
        {
            var scaleItems = resultsController.GetItems();
            resultsController.ResultsFieldsCreator.SetScaleItems(scaleItems.ToArray());
            resultsController.ResultsFieldsCreator.ScaleFactor = settingsConfig.Scale_scale;

            IEnumerable<ISurfaceElement> elems;

            if (project.TaskType == TaskType.Volume | project.TaskType == TaskType.Volume_mixed)
                elems = project.GetModelSurfaceElements(3);
            else
                elems = project.GetModelSurfaceElements(2);

            var resultFigures = resultsController.ResultsFieldsCreator.CreateSurfaceObjects(result, tableName, resName, elems).ToList();
            var colors = resultFigures.Select(resultFigure => resultFigure.Color).ToList();
            var pre = presentersCreator.CreateSurfaceObjectsPresenter(resultFigures, colors);
            pre.Name = resName;

            VBOController.DeleteAllVBObjects();
            var vb = CreateVBObject(pre);
            VBOController.AddVbo(vb);
        }

        /// <summary>
        /// BaseForm.DisplaySceneScale: шкала результатов в координатах окна, интервалы — из resultsController.
        /// </summary>
        public void DisplaySceneScale(string title, string info)
        {
            var items = resultsController.GetItems().ToList();
            var x = settingsConfig.Scale_X_Coord;
            var y = settingsConfig.Scale_Y_Coord;
            scene.Surface.Invoke(sceneController => sceneController.DisplaySceneScale(title, info, items, x, y));
        }

        private Tuple<float, float> GetMaxMin(Result result, string tableName, string resName)
        {
            var max = (float)result.Data.Tables[tableName].Compute($"Max({resName})", "");
            var min = (float)result.Data.Tables[tableName].Compute($"Min({resName})", "");

            return new Tuple<float, float>(max, min);
        }

        public void MergeResults(Result result)
        {
            try
            {
                Dictionary<int, List<int>> interfaceNodes;
                if (project.TaskType == TaskType.Volume |
                    project.TaskType == TaskType.Volume_mixed)
                    interfaceNodes = project.FindInterfacedNodes(3);
                else
                    interfaceNodes = project.FindInterfacedNodes(2);

                console.PrintInfo($"{Resources.ResultsMainMenuEvents_MergeResults_RecalculationOnNodes_Message} " +
                    $"{result.Time}", Color.Black);
                console.PrintInfo("", Color.Black);

                var resNames = result.Data.Tables[(int)ResultType.elements].GetTableSchema();

                for (int i = 1; i < resNames.Length; i++)
                {
                    resultsController.ResultsMerger.Merge(interfaceNodes, resNames[i], result);

                    var resultName = resNames[i];
                    Dispatcher.UIThread.Invoke(new Action(() => console.PrintInfo($"{Resources.ResultsMainMenuEvents_MergeResults_RecalculationOnNodesResNames_Message} {resultName}", Color.Black)));
                }

                console.PrintInfo(Resources.ResultsMainMenuEvents_MergeResults_Recalculated_Message, Color.Green);
            }
            catch (Exception ex)
            {
                Dispatcher.UIThread.Invoke(new Action(() => console.PrintInfo(ex.Message, Color.Red)));
            }
        }

        private void ShowResultValue(ResultType resType, string resName, Result result)
        {
            IEnumerable<IModelObject> objs;

            if (resType == ResultType.nodes)
                objs = project.GetAllModelNodes();
            else
                objs = project.GetAllModelElements();

            foreach (var obj in objs)
            {
                if (project.IsSelected(obj.ObjType, obj.Number))
                {
                    var coord = obj.CalcCentr();
                    var res = result.GetValue((int)resType, obj.Number, resName);
                    DisplayText3D(res.ToString(), Color.Black, coord);
                }
            }
        }
    }
}
