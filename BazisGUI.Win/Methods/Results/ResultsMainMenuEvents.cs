using BazisGUI.Win.Navigator;
using BazisGUI.Win.Properties;
using Model.Interfaces;
using Model.Interfaces.MeshObjects;
using Project.Interfaces.Tasks;
using ResultDB;
using ResultDB.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        IEnumerable<float> resultTimes;

        //public void MergeResults(Result result)
        //{
        //    try
        //    {
        //        Dictionary<int, List<int>> interfaceNodes;
        //        if (project.ProjectType == TaskType.Volume |
        //            project.ProjectType == TaskType.Volume_mixed)
        //            interfaceNodes = project.FindInterfacedNodes(3);
        //        else
        //            interfaceNodes = project.FindInterfacedNodes(2);

        //        console.PrintInfo($"Выполняется пересчет на узлы, время {result.Time}", Color.Black);
        //        console.PrintInfo("", Color.Black);

        //        var resNames = result.Data.Tables[(int)ResultType.elements].GetTableSchema();

        //        for (int i = 1; i < resNames.Length; i++)
        //        {
        //            resultsController.ResultsMerger.Merge(interfaceNodes, resNames[i], result);

        //            Invoke(new Action(() =>
        //            {
        //                console.PrintInfo($"Выполнен пересчет на узлы для {resNames[i]}", Color.Black);
        //            }));
        //        }

        //        console.PrintInfo("Пересчет завершен", Color.Green);

        //    }
        //    catch (Exception ex)
        //    {
        //        Invoke(new Action(() =>
        //        {
        //            console.PrintInfo($"В ходе пересчета возникла ошибка: {ex.Message}", Color.Red);
        //        }));
        //    }
        //}

        /// <summary>
        /// Обработчик нажатия пункта меню "Объединить БД результатов".
        /// Позволяет выбрать несколько файлов .db и последовательно объединяет их в один.
        /// </summary>
        private void MergeDataBase_Click(object sender, EventArgs e)
        {
            using var openDialog = new OpenFileDialog()
            {
                Title = "Выберите файлы БД результатов",
                Filter = "Results files (*.db)|*.db",
                Multiselect = true,
            };

            if (openDialog.ShowDialog() != DialogResult.OK)
                return;

            var paths = openDialog.FileNames;
            if (paths == null || paths.Length < 2)
            {
                console.PrintInfo("Необходимо выбрать минимум два файла", Color.Orange);
                return;
            }

            var loader = new LoadResultsFileDB();
            var currentResultPath = string.Empty;

            try
            {
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
            }

            /// <summary>
            /// Пытается удалить файл по указанному пути. Выполняет сборку мусора перед удалением,
            /// чтобы освободить возможные блокировки файла.
            /// </summary>
            void TryDeleteFile(string path)
            {
                try
                {
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

        private void открытьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var openDialog = new OpenFileDialog();

            openDialog.InitialDirectory = Path.GetFullPath(System.Windows.Forms.Application.ExecutablePath);
            openDialog.AddExtension = true;

            openDialog.Filter = "Results files (*.db)|*.db";

            if (openDialog.ShowDialog() == DialogResult.Cancel)
                return;


            ResultDbPath = openDialog.FileName;

            FillingResultsData();
        }

        private void FillingResultsData()
        {
            var loader = new LoadResultsFileDB();
            var scheme = loader.GetTablesSchemes(ResultDbPath).
                FirstOrDefault(x => x.Key == ResultType.nodes.ToString());

            List<TreeNode> results;

            if (!navigator.TrySearchNodes(NodeName.Results, out results))
            {
                var rn = navigator.CreateRealNode(NodeName.Results);

                //.SetContextMenu(rn);
                navigator.TrySearchNodes(NodeName.Project, out List<TreeNode> prNodes);
                prNodes[0].Nodes.Add(rn);
                results.Add(rn);
            }
            else
                results[0].Nodes.Clear();

            resultTimes = loader.GetValues(ResultDbPath, scheme.Key, "Time");


            foreach (var desc in scheme.Value)
            {
                var rn = navigator.CreateRealNode(NodeName.Result, $"{desc}");

                //var node = new TreeNode($"{desc}", 16, 16)
                //{ Tag = "6.1", Name = desc };

                var vn = navigator.CreateVirtualNode(NodeName.Result);
                rn.Nodes.Add(vn);
                results[0].Nodes.Add(rn);
            }
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
                Dictionary<int,List<int>> interfaceNodes;
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

                    Invoke(new Action(() => console.PrintInfo($"{Resources.ResultsMainMenuEvents_MergeResults_RecalculationOnNodesResNames_Message} {resNames[i]}", Color.Black)));
                }

                console.PrintInfo(Resources.ResultsMainMenuEvents_MergeResults_Recalculated_Message, Color.Green);

            }
            catch (Exception ex)
            {
                Invoke(new Action(() => console.PrintInfo(ex.Message, Color.Red)));
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
