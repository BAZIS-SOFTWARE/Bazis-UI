using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using BazisAvaloniaGUI.Localization;
using BazisAvaloniaGUI.SettingsControls;
using Newtonsoft.Json;
using OperationalController;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BazisAvaloniaGUI
{
    public class IODataController
    {
        // Avalonia: диалоги открываются через StorageProvider окна-владельца.
        private readonly Window owner;

        public IODataController(Window owner = null)
        {
            this.owner = owner;
        }

        public SettingsConfig LoadConfig()
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var fullPath = Path.Combine(folder, "settingsConfig.json");

            if (File.Exists(fullPath))
            {
                var settings = File.ReadAllText(fullPath);
                var config = (SettingsConfig)JsonConvert.DeserializeObject(settings, typeof(SettingsConfig));
                config?.NormalizeTransparencyValue();
                return config;
            }
            else return null;
        }

        public async Task<string> GetGmshLibraryPath()
        {
            var path = OperatingSystem.IsWindows()
                ? Environment.GetEnvironmentVariable("BazisMeshPath", EnvironmentVariableTarget.Machine)
                : Environment.GetEnvironmentVariable("BazisMeshPath");

            if (string.IsNullOrWhiteSpace(path) && OperatingSystem.IsLinux())
            {
                var bundledLibrary = Path.Combine(AppContext.BaseDirectory, "libgmsh.so");
                if (File.Exists(bundledLibrary))
                    return bundledLibrary;
            }

            if (path == null || path == "")
            {
                var files = await owner.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    AllowMultiple = false,
                    FileTypeFilter =
                    [
                        OperatingSystem.IsLinux()
                            ? new FilePickerFileType("Gmsh (*.so)") { Patterns = ["*.so", "*.so.*"] }
                            : new FilePickerFileType("Gmsh (*.dll)") { Patterns = ["*.dll"] },
                        FilePickerFileTypes.All
                    ]
                });
                if (files.Count == 0)
                    return null;
                path = files[0].TryGetLocalPath();
            }
            else
                path = $@"{path}";

            return path;
        }

        public async Task<ProjectController> ImportMesh(string fullPath)
        {
            var controller = new ProjectController();

            var mb = new TextBlock { TextWrapping = TextWrapping.Wrap };

            var mbf = CreateMessageBoxExForm(mb);
            mbf.Show(owner);
            mb.Text = Resources.ImportMeshCaption;
            var progress = new Progress<int>(value => mb.Text = $"{value}%");
            try
            {
                await Task.Run(() => controller.Open(fullPath, progress));
            }
            finally
            {
                mbf.Close();
            }

            controller.ChangeProjectName("новый_проект.bpf2");
            return controller;
        }

        public async Task<string> ExportMesh(ProjectController project)
        {
            var file = await owner.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                FileTypeChoices =
                [
                    FilePickerFileTypes.All,
                    new FilePickerFileType("STL(*.stl*)") { Patterns = ["*.stl"] }
                ]
            });
            if (file == null)
                return null;

            var fileName = file.TryGetLocalPath();
            project.ExportMesh(fileName);

            return fileName;
        }

        /// <summary>
        /// Аналог формы "Загрузка" с MessageBoxEx: окно без рамки поверх главного окна.
        /// </summary>
        public Window CreateMessageBoxExForm(TextBlock mb)
        {
            var mbf = new Window()
            {
                Name = "Загрузка",
                Title = Resources.LoadingForm_Text,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Topmost = true,
                CanResize = false,
                ShowInTaskbar = false,
                Width = 320,
                SizeToContent = SizeToContent.Height,
                Content = new Border
                {
                    Padding = new Thickness(12),
                    BorderBrush = Brushes.DarkGray,
                    BorderThickness = new Thickness(1),
                    Child = mb
                }
            };
            mb.VerticalAlignment = VerticalAlignment.Center;
            return mbf;
        }

        public async Task<ProjectController> OpenProject(string fullPath)
        {
            var controller = new ProjectController();
            var mb = new TextBlock { TextWrapping = TextWrapping.Wrap };

            var mbf = CreateMessageBoxExForm(mb);
            mbf.Show(owner);
            var progress = new Progress<int>(value => mb.Text = $"{value}%");
            try
            {
                await Task.Run(() => controller.Open(fullPath, progress));
            }
            finally
            {
                mbf.Close();
            }
            return controller;
        }
    }
}
