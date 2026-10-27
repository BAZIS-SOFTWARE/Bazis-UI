using BazisGUI.Win.Properties;
using BazisGUI.MasterInterface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        private readonly List<Type> importedMasters = new();

        /// <summary>
        /// Добавление пользовательских мастеров постановки задач из определенной dll
        /// </summary>
        /// <param name="dllPath">путь к dll, в которой определен(-ы) пользовательские мастера</param>
        public void ImportMasterDLL(string dllPath)
        {
            var assembly = Assembly.LoadFrom(dllPath);
            var tempImportedMasters = new List<Type>();
            foreach (var ctrl in assembly.GetTypes())
            {
                // проверка на соответствие типа ctrl BaseMaster типу,
                // проверка типа ctrl на абстрактность,
                // проверка ctrl в сохраненных (импортированных) типах мастеров
                // при удовлетворении этих проверок, мастер будет добавлен на графике и в коллекцию мастеров
                if (typeof(BaseMaster).IsAssignableFrom(ctrl)
                    && typeof(BaseMaster) != ctrl
                    && !ctrl.IsAbstract
                    && !importedMasters.Contains(ctrl))
                    tempImportedMasters.Add(ctrl);
            }
            CreateImportedMasters(tempImportedMasters);
        }

        private void CreateImportedMasters(List<Type> masterTypes)
        {
            try
            {
                if (masterTypes.Count == 0)
                {
                    console.PrintInfo(Resources.ImportMaster_CreateImportedMasters_MasterNotFoundOrAlreadyLoaded_Message, Color.DarkOrange);
                    return;
                }

                foreach (var item in masterTypes)
                {
                    var master = (BaseMaster)Activator.CreateInstance(item);
                    OpenMaster(master);
                    importedMasters.Add(item);
                    console.PrintInfo($"{Resources.ImportMaster_CreateImportedMasters_MasterOpened_Message} {master.MasterName}", Color.Black);
                }
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }

        private void загрузитьМастерToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog() { Filter = "dll files (*.dll) |*.dll" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                    ImportMasterDLL(dialog.FileName);
            }
        }
    }
}
