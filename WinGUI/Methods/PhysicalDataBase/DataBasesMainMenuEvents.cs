using WinGUI.Args;
using WinGUI.DataBases;
using WinGUI.Properties;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WinGUI
{
    public partial class BaseForm
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
            var btn = sender as ToolStripMenuItem;
            var name = Resources.BaseForm_материалыMenuItem_Click_Materials;
            if (btn.Checked)
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

                matBasePage.OnMutationEvent += () => OnChangeMaterials?.Invoke(this, new ChangeMaterialsEventArgs(project?.MaterialsDB?.Keys?.ToArray() ?? Array.Empty<string>()));

                OnProjectLoaded += () =>
                {
                    matBasePage.Materials = project.MaterialsDB;
                    matBasePage.PresentMaterials();
                    OnChangeMaterials?.Invoke(this, new ChangeMaterialsEventArgs(project?.MaterialsDB?.Keys?.ToArray() ?? Array.Empty<string>()));
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
            var btn = sender as ToolStripMenuItem;
            var name = Resources.BaseForm_функцииMenuItem_Click_Functions;

            if (btn.Checked)
                OpenFunctionsDB(name);
            else
                TabButtonsService.RemoveControl(name);
        }

        public void OpenFunctionsDB(string name)
        {
            try
            {
                // Подстраховка, если контроллер null
                if (!project.HasProject)
                    return;

                var funBasePage = new FunctionDataBasePage() {  HeadColor = Color.Gainsboro };

                funBasePage.LoadEvent += () =>
                {
                    project.FunctionsDB = funBasePage.Functions;
                    OnChangeFunctions?.Invoke(this, new ChangeFunctionsEventArgs(project.FunctionsDB.Keys.ToArray()));
                    console.PrintInfo($"{Resources.DataBaseMainMenuEvents_OpenDB_Message} {funBasePage.Functions.Name} {Resources.DataBaseMainMenuEvents_OpenDB_SuccessfullyAdded_Message}", Color.Green);
                };

                funBasePage.OnMutationEvent += () => OnChangeFunctions(this, new ChangeFunctionsEventArgs(project.FunctionsDB.Keys.ToArray()));

                OnProjectLoaded += () =>
                {
                    funBasePage.Functions = project.FunctionsDB;
                    funBasePage.PresentFunctions();
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
