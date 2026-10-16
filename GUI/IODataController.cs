using BazisGUI.Utilities;
using BazisGUI.SettingsControls;
using GmshApi;
using Model;
using Model.Interfaces;
using Model.IO;
using Model.IO.STL;
using Newtonsoft.Json;
using OperationalController;
using OperationalController.GmshController;
using System;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using BazisGUI.Properties;

namespace BazisGUI
{
    public class IODataController
    {

        public SettingsConfig LoadConfig()
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var fullPath = $@"{folder}\settingsConfig.json";

            if (File.Exists(fullPath))
            {
                var settings = File.ReadAllText(fullPath);
                var config = (SettingsConfig)JsonConvert.DeserializeObject(settings, typeof(SettingsConfig));
                config?.NormalizeTransparencyValue();
                return config;
            }
            else return null;
        }
        public string GetGmshLibraryPath()
        {
            var path = Environment.GetEnvironmentVariable("BazisMeshPath", EnvironmentVariableTarget.Machine);

            if (path == null || path == "")
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "dinamic library(*.dll)|*.dll|All files(*.*)|*.*"
                    ;
                if (dialog.ShowDialog() == DialogResult.Cancel)
                    return null;
                path = dialog.FileName;
            }
            else
                path = $@"{path}";


            return path;
        }

        public async Task<string> ExportMesh(IProjectController project)
        {

            var filter =
"All files(*.*)|*.*|" +
"STL(*.stl*)|*.stl";

            var dialog = new SaveFileDialog();
            dialog.Filter = filter;
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return null;

            project.ExportMesh(dialog.FileName);

            return dialog.FileName;
        }

        public Form CreateMessageBoxExForm(MessageBoxEx.MessageBoxEx mb)
        {
            var mbf = new Form()
            {
                ShowIcon = false,
                Name = "Загрузка",
                Text = Resources.LoadingForm_Text,
                StartPosition = FormStartPosition.Manual,
                Location = new System.Drawing.Point(Application.OpenForms[0].Width / 2, Application.OpenForms[0].Height / 2),
                TopMost = true,
                FormBorderStyle = FormBorderStyle.None,
                ClientSize = mb.Size,
                Owner = Application.OpenForms[0]
            };

            mbf.Controls.Add(mb);
            return mbf;
        }
    }
}
