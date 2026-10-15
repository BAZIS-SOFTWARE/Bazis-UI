using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using BazisAvaloniaGUI.Args;
using BazisAvaloniaGUI.Chamfer.Services;
using BazisAvaloniaGUI.Localization;
using BazisAvaloniaGUI.SettingsControls;
using ClientLogic;
using LicenseInfo;
using MaterialDB.FunctionData;
using MaterialDB.MaterialData;
using Model.Interfaces;
using OperationalController;
using OperationalController.ModelScenePresentator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow : Window
    {
        // Avalonia: перекрывает StyledElement.Resources, чтобы Resources.X ссылался на строковые ресурсы, как в BaseForm.
        private new class Resources : Localization.Resources { }

        public TabButtonControlService TabButtonsService;
        public event Action OnProjectLoaded;
        public event Action<ObjType, int, string> OnGroupCreated;
        public event Action<ObjType, int, string> OnGroupRenamed;
        public event Action<ObjType, int> OnGroupDeleted;
        public event EventHandler<ChangeMaterialsEventArgs> OnChangeMaterials;
        public event EventHandler<ChangeFunctionsEventArgs> OnChangeFunctions;

        string WorkingDir
        {
            get
            {
                return Path.GetDirectoryName(projectFilePath ?? lblStatus.Text);
            }
        }

        private readonly string projFilter = "Project file(*.bpf)|*.bpf|Project file(*.bpf2)|*.bpf2";
        private readonly string meshFilter = "Visual-Mesh ESI Group(*.ASC)|*.ASC|" +
            "GMSH(*.inp)|*.inp|" +
            "GMSH(*.inp_v2)|*.inp_v2|" +
            "STL(*.stl*)|*.stl|" +
            "SOLOMIA(*.dat*)|*.dat";
        private readonly string geomFilter = "(*.brep*)|*.brep|" +
            "(*.geo*)|*.geo|" +
            "*.stp*)|*.stp|" +
            "(*.step*)|*.step|" +
            "(*.iges*)|*.iges|" +
            "(*.igs*)|*.igs";

        // Ядро сцены (SceneController, VBOController, рендереры) принадлежит SceneView;
        // точки подключения сцены — Shell/Methods/Scene/SceneConnection.cs.

        ProjectController project;
        private string projectFilePath;
        private readonly ChamferWindowService chamferWindowService = new();
        private bool isLoadingProject;
        IODataController dataController;
        PreProc.PreProc preProc = new();
        IPresentersCreator presentersCreator = new PresentersCreator();
        PostProc.PostProcController resultsController = new();

        SettingsConfig settingsConfig = new()
        {
            BackGroundColor = Color.White,
            SelectObjectColor = Color.GreenYellow,
            NodeColor = Color.FromArgb(153, 192, 86),
            Transparency = false,
            Lighting = true,
            BackRibbers = false,
            SolverPath = "?",
        };

        private void BaseForm_Load(object sender, EventArgs e)
        {
            var ver = Assembly.GetExecutingAssembly().GetName().Version;
            var verStr = $"{Resources.versionWordPrefix} {ver.Major}.{ver.Minor}.{ver.Build}";
            lblVersion.Text = verStr;

            SetGeneralSettings();
            RequestRedraw();
        }

        public MainWindow() : this(Array.Empty<string>())
        {
        }

        public MainWindow(string[] args)
        {
            var datacontroller = new IODataController();
            var config = datacontroller.LoadConfig();

            if (config != null)
                settingsConfig = config;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(settingsConfig.Language);
            // Сообщения команд консоли выводятся из фоновых задач — язык ресурсов должен совпадать.
            CultureInfo.DefaultThreadCurrentUICulture = Thread.CurrentThread.CurrentUICulture;

            InitializeComponent();
            dataController = new IODataController(this);

            TabButtonsService = new TabButtonControlService(splitContainer3Panel1);
            TabButtonsService.AddControl(Resources.BaseForm_BaseForm_Load_Navigator, cntrНавигатор);
            OnProjectLoaded += RefreshDataBasePages;
            Closed += (_, _) => checkPlayerControl.Dispose();

            Opened += (arg1, arg2) => HandleArgs(args);
        }

        public async void HandleArgs(string[] args)
        {
            if (args.Length != 0)
            {
                if (args.Contains("-proj"))
                {
                    var projInd = Array.IndexOf(args, "-proj");

                    if (args.Length - 1 - projInd < 1)
                        throw new Exception(Resources.HandleArgsProjectAbsenceException);

                    await OpenProject(Path.GetFullPath(args[projInd + 1]));
                }
                if (args.Contains("-res"))
                {
                    var resInd = Array.IndexOf(args, "-res");

                    if (args.Length - 1 - resInd < 1)
                        throw new Exception(Resources.HandleArgsResultsAbsenceException);

                    var fullPath = Path.GetFullPath(args[resInd + 1]);

                    if (project == null)
                        throw new Exception(Resources.HandleArgsResultsLoadingWithoutProjectException);

                    ResultDbPath = fullPath;
                    FillingResultsData();
                }
                if (args.Contains("-cad"))
                {
                    var resInd = Array.IndexOf(args, "-cad");

                    if (args.Length - 1 - resInd < 1)
                        throw new Exception(Resources.HandleArgsCADAbsenceException);

                    await OpenProject(Path.GetFullPath(args[resInd + 1]));
                }
            }
        }

        private void SetGeneralSettings()
        {
            try
            {
                var intervals = settingsConfig.Scale_Intervals == 0 ? 2 : settingsConfig.Scale_Intervals;
                var min = settingsConfig.Scale_MinValue;
                var max = settingsConfig.Scale_MaxValue;
                var pre = settingsConfig.Scale_Precision;
                resultsController.FillRange(min, max, intervals, pre);
                // Цвет фона, прозрачность, освещение и проекция (в BaseForm — sceneController,
                // averageColorRenderer, UpdateProjection).
                ApplySceneSettings();
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }

        }

        private void BaseForm_KeyDown(object sender, Avalonia.Input.KeyEventArgs e)
        {
            PressedKey = e.Key;
        }

        private void содержаниеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var helpFile = Directory.GetFiles(AppContext.BaseDirectory, "ПО Bazis 5.2. Руководство пользователя.pdf", SearchOption.AllDirectories);

            if (helpFile.Count() != 0)
                Process.Start(new ProcessStartInfo(helpFile[0]) { UseShellExecute = true });
            else MessageBox.Show(this, Localization.Localization.GetFileMissingCaption());
        }

        private void опрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new AboutProgrammWindow() { Title = Resources.About };
            form.ShowDialog(this);
        }

        private async void сведенияMenuItem_Click(object sender, EventArgs e)
        {
            var form = new AboutLicenseWindow() { Title = Resources.LicenseInfo };

            try
            {
                if (TryServerConnection())
                {
                    serverConnection.RequestServer("CheckLicenseInfo");
                    var licInfo = JsonConvert.DeserializeObject<License>(serverConnection.Answer);

                    if (licInfo != null)
                    {
                        form.KeysInfo = string.Empty;

                        foreach (var key in licInfo.Keys)
                            form.KeysInfo += $"{key}\n";

                        form.OwnerInfo = licInfo.Company;
                    }
                    form.AdressInfo = $"{serverConnection.IPAddress} : {serverConnection.Port}";
                }
                else
                {
                    var res = await MessageBox.Show(this,
                        Resources.BazisServerPathMissingMessage,
                        Localization.Localization.GetAttentionCaption(), MessageBoxButtons.YesNo);

                    if (res == DialogResult.Yes)
                        // StartLisenceForm: форма ClientGUI.ClientControl реализована на WinForms и в Avalonia не перенесена.
                        console.PrintInfo($"{Resources.Licensing}: форма подключения к серверу лицензий в Avalonia-версии ещё не перенесена.", Color.Orange);
                    else
                        serverConnection = new ClientController(IPAddress.Loopback, 8001);
                }
            }
            catch (Exception ex)
            {
                if (ex is Newtonsoft.Json.JsonReaderException)
                    await MessageBox.Show(this, Resources.GetLicenseInfoException);
                else
                    await MessageBox.Show(this, ex.Message);
            }

            await form.ShowDialog(this);
        }

        private async void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var dialog = await StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions { AllowMultiple = false });
                if (dialog.Count == 0)
                    return;

                var folderName = dialog[0].TryGetLocalPath();

                project = new ProjectController();
                project.CreateProject("newProject.bpf2");

                projectFilePath = Path.GetFullPath(Path.Combine(folderName, project.Name));
                lblStatus.Text = projectFilePath;

                var appDirName = AppContext.BaseDirectory;

                string[] matfiles = Directory.GetFiles(appDirName, "*Materials*.jsf", SearchOption.AllDirectories);
                string[] funfiles = Directory.GetFiles(appDirName, "*functions*.jsf", SearchOption.AllDirectories);

                project.CreateTask();

                if (matfiles.Length != 0)
                {
                    var matDirName = Path.GetDirectoryName(matfiles[0]);
                    var matName = Path.GetFileName(matfiles[0]);
                    if (IOFileController.CopyFile(matName, matDirName, folderName))
                    {
                        var matDB = new MaterialDBData(matName, folderName);
                        project.MaterialsDB = matDB;
                    }

                }

                if (funfiles.Length != 0)
                {
                    var funDirName = Path.GetDirectoryName(funfiles[0]);
                    var funName = Path.GetFileName(funfiles[0]);
                    if (IOFileController.CopyFile(funName, funDirName, folderName))
                    {
                        var funDB = new FunctionDBData(funName, folderName);
                        project.FunctionsDB = funDB;
                    }
                }

                ClearResultsState();
                CloseGanttChart();
                CancelInteractiveSelection();
                chamferWindowService.Close();
                ClearAllDataOnScene();
                PresentProject();
                PresentCompDataOnTree(new List<string>());
                UnblockInterface();
                OnProjectLoaded?.Invoke();

                RequestRedraw();
            }
            catch (Exception ex)
            {
                await MessageBox.Show(this, Localization.Localization.GetErrorWithStackMessage(ex), Localization.Localization.GetErrorCaption());
            }
        }

        private async Task OpenProject(string filePath)
        {
            if (isLoadingProject)
                return;

            isLoadingProject = true;
            var wasEnabled = IsEnabled;
            IsEnabled = false;
            try
            {
                var previousProject = project;
                filePath = Path.GetFullPath(filePath);
                var ext = Path.GetExtension(filePath).ToLowerInvariant();

                if (IsProjectFile(filePath))
                {
                    project = await dataController.OpenProject(filePath);
                    projectFilePath = filePath;
                }

                else if (geomFilter.Contains(ext))
                {
                    if (project == null)
                        project = new ProjectController();

                    if (!project.IsGeometryInitialized)
                    {
                        var gmshLibraryPath = await dataController.GetGmshLibraryPath();
                        if (string.IsNullOrWhiteSpace(gmshLibraryPath))
                            return;

                        project.InitializeGeometry(gmshLibraryPath);
                    }

                    project.ImportCAD(filePath);
                }

                else
                {
                    project = await dataController.ImportMesh(filePath);
                    projectFilePath = null;
                }

                lblStatus.Text = projectFilePath ?? filePath;

                if (!ReferenceEquals(previousProject, project))
                {
                    ClearResultsState();
                    CloseGanttChart();
                    CancelInteractiveSelection();
                    chamferWindowService.Close();
                }

                ClearAllDataOnScene();
                PresentProject();
                OnProjectLoaded?.Invoke();

                UnblockInterface();

                FitObjectsToScreen();
                RequestRedraw();
            }
            catch (Exception ex)
            {
                OwnedWindows.FirstOrDefault(window => window.Name == "Загрузка")?.Close();
                IsEnabled = wasEnabled;
                await MessageBox.Show(this, Localization.Localization.GetErrorWithStackMessage(ex), Localization.Localization.GetErrorCaption());
            }
            finally
            {
                IsEnabled = wasEnabled;
                isLoadingProject = false;
            }
        }

        private async void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isLoadingProject)
                return;
            var dialog = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                AllowMultiple = false,
                FileTypeFilter = FileTypes(string.Join("|", "All files(*.*)|*.*", projFilter, geomFilter, meshFilter))
            });
            if (dialog.Count == 0)
                return;

            await OpenProject(dialog[0].TryGetLocalPath());
        }

        private void UnblockInterface()
        {
            геометрияToolStripMenuItem.IsEnabled = true;
            сеткаToolStripMenuItem.IsEnabled = true;
            dataBasesMenuItem.IsEnabled = true;
            tasksMenuItem.IsEnabled = true;
            расчетыToolStripMenuItem.IsEnabled = true;
            результатыMenuItem.IsEnabled = true;
            инструментыToolStripMenuItem.IsEnabled = true;
            // Кнопки панели сцены (btnAdvSelection, btnDisplayStates, btnDisplayViews, btnFitToScreen,
            // btnMakeScreenShot, btnShowInsideObjects) — часть SceneView.

            console.IsEnabled = true;
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationExit();
        }

        /// <summary>Аналог Application.Exit() из WinForms.</summary>
        private void ApplicationExit()
        {
            if (Avalonia.Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                desktop.Shutdown();
            else
                Close();
        }

        private async void сохранитькакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (await SaveProjectAsync(saveAs: true))
                    console.PrintInfo(Resources.ProjectSavedCaption, Color.Black);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }

        private async void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (await SaveProjectAsync())
                    console.PrintInfo(Resources.ProjectSavedCaption, Color.Black);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }

        }

        private static bool IsProjectFile(string filePath)
        {
            var extension = Path.GetExtension(filePath);
            return string.Equals(extension, ".bpf", StringComparison.OrdinalIgnoreCase)
                || string.Equals(extension, ".bpf2", StringComparison.OrdinalIgnoreCase);
        }

        private async Task<bool> SaveProjectAsync(bool saveAs = false)
        {
            if (project == null)
                return false;

            var fileName = projectFilePath;
            if (saveAs || string.IsNullOrEmpty(fileName))
            {
                var saveDialog = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
                {
                    DefaultExtension = "bpf2",
                    FileTypeChoices = FileTypes("(*.bpf2)|*.bpf2|(*.bpf)|*.bpf")
                });
                if (saveDialog == null)
                    return false;

                fileName = saveDialog.TryGetLocalPath();
                if (string.IsNullOrWhiteSpace(fileName))
                    return false;
                if (!IsProjectFile(fileName))
                    fileName += ".bpf2";
            }

            SaveProjectToFile(fileName);
            return true;
        }

        private void SaveProjectToFile(string fileName)
        {
            if (!IsProjectFile(fileName))
                throw new ArgumentException("Project file extension must be .bpf or .bpf2.", nameof(fileName));

            fileName = Path.GetFullPath(fileName);
            var newFolder = Path.GetDirectoryName(fileName);
            var oldFolder = WorkingDir;
            var comparison = OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            var oldName = project.Name;
            try
            {
                project.ChangeProjectName(Path.GetFileName(fileName));
                if (!string.IsNullOrEmpty(oldFolder) && !string.Equals(oldFolder, newFolder, comparison))
                {
                    if (project.MaterialsDB != null)
                        IOFileController.CopyFile(project.MaterialsDB.Name, oldFolder, newFolder);
                    if (project.FunctionsDB != null)
                        IOFileController.CopyFile(project.FunctionsDB.Name, oldFolder, newFolder);
                }
                project.Save(fileName);
            }
            catch
            {
                project.ChangeProjectName(oldName);
                throw;
            }

            projectFilePath = fileName;
            lblStatus.Text = fileName;
        }

        private void PresentProject()
        {
            SubscribeToModelView();
            CreateVBObjects("Объекты");

            PresentGeoData();
            PresentMeshData();
            PresentGroupDataOnTree();
            PresentCondDataOnTree();
            PresentModelObjectsForSelection();
        }

        private void OnClosingForm(object sender, WindowClosingEventArgs e)
        {
            // Страница настроек закрывается вместе с окном: изменения сохраняются без сообщения.
            isClosing = true;
            CancelInteractiveSelection();
            chamferWindowService.Close();
            if (savedConfigSnapshot != null)
                SaveConfigIfChanged(showMessage: false);
            project?.UnloadGeometry();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Panel1Collapsed = !Panel1Collapsed;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Panel2Collapsed = !Panel2Collapsed;
        }

        private async void добавитьСеткуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (project != null)
                {
                    var dialog = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                    {
                        AllowMultiple = false,
                        FileTypeFilter = FileTypes(meshFilter + "|" + projFilter)
                    });
                    if (dialog.Count == 0)
                        return;
                    var fileName = dialog[0].TryGetLocalPath();

                    await AppendMeshAsync(fileName);

                    ClearAllDataOnScene();
                    PresentProject();

                    UnblockInterface();

                    FitObjectsToScreen();
                    RequestRedraw();
                }
            }

            catch (Exception ex)
            {
                OwnedWindows.FirstOrDefault(window => window.Name == "Загрузка")?.Close();
                await MessageBox.Show(this, Localization.Localization.GetErrorWithStackMessage(ex), Localization.Localization.GetErrorCaption());
            }
        }

        private async Task AppendMeshAsync(string fileName)
        {
            var currentProject = project;
            var wasEnabled = IsEnabled;
            var message = new TextBlock { TextWrapping = Avalonia.Media.TextWrapping.Wrap };
            var loadingWindow = dataController.CreateMessageBoxExForm(message);
            var progress = new Progress<int>(value => message.Text = $"{value}%");
            try
            {
                loadingWindow.Show(this);
                IsEnabled = false;
                await Task.Run(() => currentProject.Append(fileName, progress));
            }
            finally
            {
                loadingWindow.Close();
                IsEnabled = wasEnabled;
            }
        }

        private void addChamferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // CheckOnClick: MenuItem с ToggleType = CheckBox переключает IsChecked до события Click.
            if (addChamferToolStripMenuItem.IsChecked)
            {
                SelectedObjects = SelectionType.Curves;
                var synchronizationContext = SynchronizationContext.Current;
                var operationService = new SynchronizationContextChamferOperationService(synchronizationContext, RequestChamferByAngle, RequestChamferByLengths, RequestChamferPreview, RequestClearChamferPreview);
                chamferWindowService.Show(this, operationService, OnChamferWindowClosed);
            }
            else
                chamferWindowService.Close();
        }

        /// <summary>
        /// Приводит состояние пункта меню построения фаски в соответствие с фактическим состоянием окна.
        /// </summary>
        /// <remarks>
        /// Вызывается после закрытия окна любым способом: как по команде <see cref="ChamferWindowService.Close"/>,
        /// так и при закрытии окна самим пользователем. Программное снятие флажка не вызывает событие
        /// <see cref="MenuItem.Click"/>, поэтому повторного закрытия окна не происходит.
        /// </remarks>
        private void OnChamferWindowClosed()
        {
            if (!IsVisible)
                return;

            addChamferToolStripMenuItem.IsChecked = false;
        }

        /// <summary>
        /// Действие, выполняемое после изменения выбора на сцене.
        /// <c>null</c>, если дополнительное обновление сцены не требуется.
        /// </summary>
        private Action sceneSelectionChangedAction;

        private void RequestChamferByAngle(double length, double angle, bool reflected) => CreateChamfer(length, angle, true, reflected);
        private void RequestChamferByLengths(double length1, double length2, bool reflected) => CreateChamfer(length1, length2, false, reflected);
        private void RequestChamferPreview(double length, double valueSecond, bool isAngle, bool isReflected)
        {
            sceneSelectionChangedAction = () => PreviewChamfer(length, valueSecond, isAngle, isReflected);
            sceneSelectionChangedAction();
        }
        private void RequestClearChamferPreview()
        {
            sceneSelectionChangedAction = null;
            ClearChamferPreview(true);
        }

        /// <summary>
        /// Avalonia: преобразует фильтр WinForms "Имя|*.ext|..." в типы файлов StorageProvider.
        /// </summary>
        private static List<FilePickerFileType> FileTypes(string filter)
        {
            var parts = filter.Split('|');
            var result = new List<FilePickerFileType>();
            for (var i = 0; i + 1 < parts.Length; i += 2)
                result.Add(new FilePickerFileType(parts[i]) { Patterns = parts[i + 1].Split(';') });
            return result;
        }
    }
}
