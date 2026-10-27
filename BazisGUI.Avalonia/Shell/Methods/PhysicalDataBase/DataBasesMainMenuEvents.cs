using BazisGUI.Avalonia.Args;
using BazisGUI.Avalonia.Databases;
using BazisGUI.Avalonia.Localization;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace BazisGUI.Avalonia.Shell
{
    internal partial class MainWindow
    {
        private T LoadDB<T>(string fileName)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
            };

            return JsonConvert.DeserializeObject<T>(
                File.ReadAllText(fileName),
                settings);
        }

        private void материалыMenuItem_Click(object sender, EventArgs e)
        {
            var btn = материалыMenuItem;
            var name = Resources.BaseForm_материалыMenuItem_Click_Materials;
            // CheckOnClick: MenuItem с ToggleType = CheckBox переключает IsChecked до события Click.
            if (btn.IsChecked)
                OpenMaterialsDB(name);
            else
                TabButtonsService.RemoveControl(name);
        }

        public void OpenMaterialsDB(string name)
        {
            try
            {
                var matBasePage = new MaterialsDataBasePage() { HeadColor = Color.Gainsboro };

                matBasePage.LoadEvent += () =>
                {
                    project.MaterialsDB = matBasePage.Materials;
                    OnChangeMaterials?.Invoke(this, new ChangeMaterialsEventArgs(project?.MaterialsDB?.Keys?.ToArray() ?? Array.Empty<string>()));
                    console.PrintInfo($"{Resources.DataBaseMainMenuEvents_OpenDB_Message} {matBasePage.Materials.Name} {Resources.DataBaseMainMenuEvents_OpenDB_SuccessfullyAdded_Message}", Color.Green);
                };

                matBasePage.OnMutationEvent += () =>
                {
                    project.MaterialsDB = matBasePage.Materials;
                    OnChangeMaterials?.Invoke(this, new ChangeMaterialsEventArgs(project.MaterialsDB.Keys.ToArray()));
                };

                // Проект может существовать без базы материалов. Например если старт с геометрии.
                if (project.MaterialsDB == null)
                    console.PrintInfo(Resources.DataBaseMainMenuEvents_OpenMatDB_DBNotLoaded_Message, Color.Red);
                else
                {
                    matBasePage.Materials = project.MaterialsDB;
                    matBasePage.PresentMaterials();
                }

                TabButtonsService.AddControl(name, matBasePage);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }

        private void функцииMenuItem_Click(object sender, EventArgs e)
        {
            var btn = функцииMenuItem;
            var name = Resources.BaseForm_функцииMenuItem_Click_Functions;

            // CheckOnClick: MenuItem с ToggleType = CheckBox переключает IsChecked до события Click.
            if (btn.IsChecked)
                OpenFunctionsDB(name);
            else
                TabButtonsService.RemoveControl(name);
        }

        public void OpenFunctionsDB(string name)
        {
            try
            {
                // Подстраховка, если контроллер null
                if (project == null)
                    return;

                var funBasePage = new FunctionDataBasePage() { HeadColor = Color.Gainsboro };

                funBasePage.LoadEvent += () =>
                {
                    project.FunctionsDB = funBasePage.Functions;
                    OnChangeFunctions?.Invoke(this, new ChangeFunctionsEventArgs(project.FunctionsDB.Keys.ToArray()));
                    console.PrintInfo($"{Resources.DataBaseMainMenuEvents_OpenDB_Message} {funBasePage.Functions.Name} {Resources.DataBaseMainMenuEvents_OpenDB_SuccessfullyAdded_Message}", Color.Green);
                };

                funBasePage.OnMutationEvent += () =>
                {
                    project.FunctionsDB = funBasePage.Functions;
                    OnChangeFunctions?.Invoke(this, new ChangeFunctionsEventArgs(project.FunctionsDB.Keys.ToArray()));
                };

                // Проект может существовать без базы материалов. Например если старт с геометрии.
                if (project.FunctionsDB == null)
                    console.PrintInfo(Resources.DataBaseMainMenuEvents_OpenMatDB_DBNotLoaded_Message, Color.Red);
                else
                {
                    funBasePage.Functions = project.FunctionsDB;
                    funBasePage.PresentFunctions();
                }

                TabButtonsService.AddControl(name, funBasePage);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }

        private void RefreshDataBasePages()
        {
            foreach (var name in TabButtonsService.GetNames().ToArray())
            {
                if (TabButtonsService.GetControl(name) is MaterialsDataBasePage materialsPage)
                {
                    materialsPage.Materials = project.MaterialsDB ?? new MaterialDB.MaterialData.MaterialDBData() { Name = "newMatDataBase.jsf" };
                    materialsPage.PresentMaterials();
                    OnChangeMaterials?.Invoke(this, new ChangeMaterialsEventArgs(project.MaterialsDB?.Keys?.ToArray() ?? Array.Empty<string>()));
                }
                else if (TabButtonsService.GetControl(name) is FunctionDataBasePage functionsPage)
                {
                    functionsPage.Functions = project.FunctionsDB ?? new MaterialDB.FunctionData.FunctionDBData() { Name = "newFuncDataBase.jsf" };
                    functionsPage.PresentFunctions();
                    OnChangeFunctions?.Invoke(this, new ChangeFunctionsEventArgs(functionsPage.Functions.Keys.ToArray()));
                }
            }
        }

        private string FindFileByPath(string path, string fileName)
        {
            var projFiles = Directory.GetFiles(path, fileName, SearchOption.AllDirectories);
            if (projFiles.Count() > 0)
            {
                return Path.GetDirectoryName(projFiles[0]);
            }

            return null;
        }
    }
}
