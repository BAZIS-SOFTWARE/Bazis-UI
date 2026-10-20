using BazisGUI.Args;
using BazisGUI.AvaloniaUI.Chamfer.Services;
using BazisGUI.Navigator;
using BazisGUI.Properties;
using BazisGUI.Scene;
using BazisGUI.Scene.Core;
using BazisGUI.Scene.VBO;
using BazisGUI.SettingsControls;
using ClientGUI;
using ClientLogic;
using LicenseInfo;
using MaterialDB.FunctionData;
using MaterialDB.MaterialData;
using Model.Interfaces;
using Newtonsoft.Json;
using OperationalController;
using OperationalController.GmshController;
using OperationalController.ModelScenePresentator;
using PostProc;
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
using System.Windows.Forms;

namespace BazisGUI
{

    public partial class BaseForm : Form
    {
        public TabButtonControlService TabButtonsService;
        public event Action OnProjectLoaded;
        public event Action<ObjType, int, string> OnGroupCreated;
        public event Action<ObjType, int, string> OnGroupRenamed;
        public event Action<ObjType, int> OnGroupDeleted;
        public event EventHandler<ChangeMaterialsEventArgs> OnChangeMaterials;
        public event EventHandler<ChangeFunctionsEventArgs> OnChangeFunctions;


        Point ScreenMousePosition { get; set; } = new Point(0, 0);
        bool MouseMoveFlag { get; set; }

        string WorkingDir
        {
            get
            {
                return Path.GetDirectoryName(project?.FilePath);
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

        //private System.Windows.Forms.Timer connectTimer = new System.Windows.Forms.Timer();
        //ProjectData project;

        // Ядро сцены (BazisGUI.Scene.Core.SceneController) — см. GUI/Documents/scene.avalonia.md,
        // шаг 4 плана перехода. Камера, VBOController, рендер базиса/компаса/точки вращения/модельных
        // объектов и матрицы вида/проекции теперь считаются там; ниже — прежние имена полей,
        // оставленные как проброс к sceneController, чтобы не трогать остальные ~500 файлов проекта,
        // которые их читают (VBOController, ScaleFactor, Position, ViewMatrix и т.д.).
        SceneController sceneController;

        Advanced3DClipper advanced3DClipper => sceneController.Advanced3DClipper;
        AverageColorRenderer averageColorRenderer => sceneController.AverageColorRenderer;
        VBOController VBOController => sceneController.VboController;

        //BasePage module;
        IProjectController project;
        // Проект получен импортом CAD/сетки и ещё не сохранялся под своим именем: «Сохранить» вызывает «Сохранить как»
        bool requiresSaveAs;
        IODataController dataController = new();
        PreProc.PreProc preProc = new();
        PostProcController resultsController = new();
        IPresentersCreator presentersCreator = new PresentersCreator();

        ClientController serverConnection;

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

        private Thread serverConnectionPing;

        private void BaseForm_Load(object sender, EventArgs e)
        {
            var ver = Assembly.GetExecutingAssembly().GetName().Version;
            var verStr = $"{Resources.versionWordPrefix} {ver.Major}.{ver.Minor}.{ver.Build}";
            lblVersion.Text = verStr;


            SetGeneralSettings();
            RequestRedraw();
        }


        public BaseForm(string[] args)
        {
            var datacontroller = new IODataController();
            var config = datacontroller.LoadConfig();

            if (config != null)
                settingsConfig = config;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(settingsConfig.Language);

            InitializeComponent();
            project = new ProjectController();
            ApplyModelViewSettings();
            project.Message += Project_Message;

            propertiesPanel.HeaderName = Resources.PropertiesPanelControl_headerName_text;
            navigator.HeaderName = Resources.NavigatorControl_headerName_text;

            // TODO где-то тут нужно добавить все контроллы в TabButtonControlService
            TabButtonsService = new TabButtonControlService(splitContainer3.Panel1);
            TabButtonsService.AddControl(Resources.BaseForm_BaseForm_Load_Navigator, cntrНавигатор);

            console.HeaderName = Resources.ConsoleControl_headerName_text;

            cntrНавигатор.SplitterWidth = 8;
            splitContainer2.SplitterWidth = 8;
            splitContainer3.SplitterWidth = 8;

            SetPadding();

            //scene.InitializeContexts();
            //Gle.Load();//Это скорее всего больше не понадобится
            scene.Load += SceneInitialization;//Это конвертировалось в событие scene.Load!
            Shown += (arg1, arg2) => HandleArgs(args);
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

                    if (!project.HasProject)
                        throw new Exception(Resources.HandleArgsResultsLoadingWithoutProjectException);

                    ResultDbPath = fullPath;
                    FillingResultsData();
                    //navigator.TrySearchNodes("результаты", out List<TreeNode> nodes);

                    //nodes.First().Nodes[0].Text = fullPath;
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

        [Obsolete("Пока не использовать")]
        private void DropDown_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
            {
                e.Cancel = true;
            }
        }



        private void UnBlockGeneralMenuInterface(string moduleName, bool flag)
        {
            if (moduleName == "Weld" | moduleName == "HeatTreatment")
            {
                if (flag)
                {
                    tasksMenuItem.Enabled = true;
                    dataBasesMenuItem.Enabled = true;
                    //meshMenuItem.Visible = true;
                    результатыMenuItem.Visible = true;
                }
                else
                {
                    tasksMenuItem.Enabled = false;
                    dataBasesMenuItem.Enabled = false;
                    //meshMenuItem.Visible = false;
                    результатыMenuItem.Visible = false;
                }
            }
        }

        private void CloseActivePageChildControls(string moduleName)
        {
            toolStripContainer.ContentPanel.Controls.RemoveByKey(moduleName);

            var openForms = Application.OpenForms.Cast<Form>().ToArray();

            foreach (var form in openForms)
            {
                if (!form.Name.Equals(this.Name))
                    form.Close();
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
                //BackGroundColor = settingsConfig.BackGroudColor;
                sceneController.BackGroundColor = settingsConfig.BackGroundColor;
                averageColorRenderer.IsEnable = settingsConfig.Transparency;
                averageColorRenderer.IsLighting = settingsConfig.Lighting;//Синхронизация с рендером прозрачности
                // Старый код читал освещение, проекцию и положение света из settingsConfig при каждой
                // отрисовке; SceneController хранит их сам, поэтому сохранённые значения передаём при запуске
                sceneController.IsLighting = settingsConfig.Lighting;
                sceneController.Projection = settingsConfig.Projection;
                sceneController.LightTranslateX = settingsConfig.LighterPosition.X;
                sceneController.LightTranslateY = settingsConfig.LighterPosition.Y;
                //module.ScenePage.NodeColor = settingsConfig.NodeColor;
                //module.ScenePage.E2DColor = settingsConfig.Elem2DColor;
                //module.ScenePage.E3DColor = settingsConfig.Elem3DColor;
                UpdateProjection();
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }

        }

        private void BaseForm_KeyDown(object sender, KeyEventArgs e)
        {
            PressedKey = e.KeyCode;
        }

        private void содержаниеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new Form() { Name = "helpForm", Text = BazisGUI.Properties.Resources.Reference, ShowIcon = false };
            form.TopMost = true;
            var helpFile = Directory.GetFiles(Application.StartupPath, "ПО Bazis 5.2. Руководство пользователя.pdf", SearchOption.AllDirectories);

            if (helpFile.Count() != 0)
                Help.ShowHelp(form, helpFile[0]);
            else MessageBox.Show(Localization.Localization.GetFileMissingCaption());
        }

        private void опрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new Form() { Name = "aboutProgrammForm", Text = BazisGUI.Properties.Resources.About, ShowIcon = false };
            var control = new AboutProgrammControl { Dock = DockStyle.Fill };

            form.ClientSize = control.Size;
            form.Controls.Add(control);
            form.ShowDialog();
        }

        private void сведенияMenuItem_Click(object sender, EventArgs e)
        {
            var form = new Form() { Name = "aboutLicenseForm", Text = Resources.LicenseInfo, ShowIcon = false };
            form.TopMost = true;
            var control = new AboutLicenseControl { Dock = DockStyle.Fill };
            form.ClientSize = control.Size;

            try
            {
                if (TryServerConnection())
                {
                    serverConnection.RequestServer("CheckLicenseInfo");
                    var licInfo = JsonConvert.DeserializeObject<License>(serverConnection.Answer);

                    if (licInfo != null)
                    {
                        control.KeysInfo = string.Empty;

                        foreach (var key in licInfo.Keys)
                            control.KeysInfo += $"{key}\n";

                        control.OwnerInfo = licInfo.Company;
                    }
                    control.AdressInfo = $"{serverConnection.IPAddress} : {serverConnection.Port}";
                }
                else
                {
                    var res = MessageBox.Show(
                        Resources.BazisServerPathMissingMessage,
                        Localization.Localization.GetAttentionCaption(), MessageBoxButtons.YesNo);

                    if (res == DialogResult.Yes)
                        StartLisenceForm("");
                    else
                        serverConnection = new ClientController(IPAddress.Loopback, 8001);
                }
            }
            catch (Exception ex)
            {
                if (ex is Newtonsoft.Json.JsonReaderException)
                    MessageBox.Show(Resources.GetLicenseInfoException);
                else
                    MessageBox.Show(ex.Message);
            }

            form.Controls.Add(control);
            form.ShowDialog();
        }

        private void StartLisenceForm(string request)
        {
            var form = new Form() { Name = "checkForm", Text = Resources.Licensing, ShowIcon = false };
            var control = new ClientControl() { Dock = DockStyle.Fill };

            control.LicenseActionEvent += (ar1, ar2) =>
            {
                serverConnection = new ClientController(ar1, ar2);
                if (request != null)
                {
                    serverConnection.RequestServer(request);

                    if (serverConnection.Answer == "можно")
                    {
                        control.LabelAnswer = Resources.LicenseAllowedAnswer;
                        UnBlockGeneralMenuInterface(request.Split(' ')[0], true);
                        StartLicensing(request.Split(' ')[0]);
                    }
                    else if (serverConnection.Answer == "Пустой запрос")
                        control.LabelAnswer = Resources.ConnectionEstablishedAnswer;
                    else
                        control.LabelAnswer = serverConnection.Answer;
                }
            };
            form.ClientSize = control.Size;
            form.Controls.Add(control);

            form.ShowDialog();
        }



        public void KillAlreadyLaunchdExamples()
        {
            var runProc = Process.GetProcessesByName("BazisGUI");

            if (runProc.Length > 1)
            {
                var process = new Process();
                var startInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "cmd.exe",
                    Arguments = $"/C taskkill /pid {runProc[0].Id} /f",
                    Verb = "runas"
                };
                process.StartInfo = startInfo;
                process.Start();
            }
        }

        private void BaseForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (serverConnectionPing != null)
            {
                serverConnectionPing.Abort();

                while (true)
                    if (!serverConnectionPing.IsAlive)
                        break;
            }
        }

        private static void ShowReleaseNotes()
        {
            var form = new Form() { Name = "newsForm", Text = Resources.VersionNews, ShowIcon = false, Size = new Size(500, 300) };
            form.TopMost = true;
            var helpFile = Directory.GetFiles(Application.StartupPath, "ReleaseNotes.pdf", SearchOption.AllDirectories);

            if (helpFile.Count() != 0)
                Help.ShowHelp(form, helpFile[0]);
            else MessageBox.Show(Localization.Localization.GetFileMissingCaption());
        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var dialog = new FolderBrowserDialog();
                if (dialog.ShowDialog() == DialogResult.Cancel)
                    return;

                var folderName = dialog.SelectedPath;

                project.Create(Path.Combine(folderName, "newProject.bpf2"));
                preProc.Clear();
                PresentCompDataOnTree();
                requiresSaveAs = false;
                // Геометрия прежнего проекта не должна переходить в новый (см. OpenProject)
                if (project.IsGeometryInitialized)
                    project.UnloadGeometry();

                var appDirName = Path.GetDirectoryName(Application.ExecutablePath);

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

            }
            catch (Exception ex)
            {
                MessageBox.Show(Localization.Localization.GetErrorWithStackMessage(ex), Localization.Localization.GetErrorCaption());
            }
        }

        private async Task OpenProject(string filePath)
        {
            try
            {
                var ext = Path.GetExtension(filePath).ToLower();

                // Как в исходном BaseForm: CAD импортируется в текущий контроллер с уже открытой сессией Gmsh
                if (geomFilter.Contains(ext))
                {
                    if (!project.IsGeometryInitialized)
                    {
                        var gmshLibraryPath = dataController.GetGmshLibraryPath();
                        if (string.IsNullOrWhiteSpace(gmshLibraryPath))
                            return;

                        project.InitializeGeometry(gmshLibraryPath);

                        //GmshController.Gmsh.Option.SetNumber("General.AbortOnError", 0);
                    }

                    project.ImportGeometry(filePath);
                    requiresSaveAs = true;
                }
                else
                {
                    await LoadModelWithProgress(filePath, false);
                    preProc.Clear();
                    PresentCompDataOnTree();
                    requiresSaveAs = ext != ".bpf" && ext != ".bpf2";

                    // До перехода на один ProjectController открытие проекта/сетки создавало новый контроллер
                    // без геометрии. Open не сбрасывает Gmsh, поэтому после успешной загрузки выгружаем CAD-модель
                    // прежнего проекта, иначе генерация сетки работает по ней. При ошибке загрузки сессия остаётся
                    if (project.IsGeometryInitialized)
                        project.UnloadGeometry();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Localization.Localization.GetErrorWithStackMessage(ex), Localization.Localization.GetErrorCaption());
                Application.OpenForms["Загрузка"]?.Close();
            }
        }

        /// <summary>
        /// Читает проект или сетку в рабочем потоке и показывает процент чтения файла.
        /// </summary>
        private async Task LoadModelWithProgress(string filePath, bool append)
        {
            var loadingView = new MessageBoxEx.MessageBoxEx { Dock = DockStyle.Fill };
            loadingView.Message = append ? Resources.ImportMeshCaption : Resources.LoadingForm_Text;
            using var loadingForm = dataController.CreateMessageBoxExForm(loadingView);
            var progressBar = new ProgressBar { Dock = DockStyle.Bottom, Minimum = 0, Maximum = 100, Height = 18, Style = ProgressBarStyle.Marquee };
            loadingForm.ClientSize = new Size(loadingView.Width, loadingView.Height + progressBar.Height);
            loadingForm.Controls.Add(progressBar);
            progressBar.BringToFront();
            var progress = new Progress<int>(value =>
            {
                if (progressBar.Style == ProgressBarStyle.Marquee)
                    progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Value = Math.Clamp(value, 0, 100);
            });

            loadingForm.Show();
            Enabled = false;
            try
            {
                if (append)
                    await Task.Run(() => project.Append(filePath, progress));
                else
                    await Task.Run(() => project.Open(filePath, progress));

                if (append)
                {
                    FitObjectsToScreen();
                    RequestRedraw();
                }
            }
            finally
            {
                Enabled = true;
                loadingForm.Close();
            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = string.Join("|", "All files(*.*)|*.*", projFilter, geomFilter, meshFilter);
            dialog.DefaultExt = "*.bpf2";
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            OpenProject(dialog.FileName);
        }

        private void UnblockInterface()
        {
            геометрияToolStripMenuItem.Enabled = true;
            сеткаToolStripMenuItem.Enabled = true;
            dataBasesMenuItem.Enabled = true;
            tasksMenuItem.Enabled = true;
            расчетыToolStripMenuItem.Enabled = true;
            результатыMenuItem.Enabled = true;
            инструментыToolStripMenuItem.Enabled = true;
            //scene.Enabled = true;

            btnAdvSelection.Enabled = true;
            btnDisplayStates.Enabled = true;
            btnDisplayViews.Enabled = true;

            btnFitToScreen.Enabled = true;
            btnMakeScreenShot.Enabled = true;
            btnShowInsideObjects.Enabled = true;

            console.Enabled = true;
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void webPageLabel_Click(object sender, EventArgs e)
        {
            var url = $"https://{webPageLabel.Text}";
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
                Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
        }

        private void сохранитькакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveProjectAs();
        }

        /// <summary>
        /// «Сохранить как». Возвращает true, если проект сохранён.
        /// </summary>
        private bool SaveProjectAs()
        {
            try
            {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.DefaultExt = "bpf2";

                var filter = "(*.bpf2)|*.bpf2|(*.bpf)|*.bpf";

                saveDialog.Filter = filter;

                if (saveDialog.ShowDialog() == DialogResult.Cancel)
                    return false;

                if (!project.HasProject)
                {
                    MessageBox.Show(Resources.SaveWithoutProjectMessage);
                    return false;
                }
                else
                {
                    var newFolder = Path.GetDirectoryName(saveDialog.FileName);
                    var oldFolder = Path.GetDirectoryName(project.FilePath);

                    project.ChangeProjectName(Path.GetFileName(saveDialog.FileName));

                    // Пробуем не использовать это свойство
                    //project.Path = newFolder;

                    if (oldFolder != newFolder)
                    {
                        if (project.MaterialsDB != null)
                            IOFileController.CopyFile(project.MaterialsDB.Name, oldFolder, newFolder);
                        if (project.FunctionsDB != null)
                            IOFileController.CopyFile(project.FunctionsDB.Name, oldFolder, newFolder);
                    }

                    project.Save(saveDialog.FileName);
                    requiresSaveAs = false;

                    console.PrintInfo(Resources.ProjectSavedCaption, Color.Black);
                    return true;
                }
            }
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
                return false;
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // IsModified не учитывает правки условий из панели свойств (они меняют объекты напрямую),
                // поэтому сохраняем всегда, как и до перехода на события ProjectController
                if (!project.HasProject)
                    return;
                // После импорта CAD/сетки путь проекта — «новый_проект.bpf2» рядом с исходным файлом:
                // молча писать туда нельзя (затрёт ранее сохранённый проект из той же папки)
                if (requiresSaveAs)
                {
                    SaveProjectAs();
                    return;
                }
                project.Save(project.FilePath);
                console.PrintInfo(Resources.ProjectSavedCaption, Color.Black);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }

        }

        private void PresentProject()
        {
            CreateVBObjects("Объекты");

            PresentGeoData();
            PresentMeshData();
            PresentGroupDataOnTree();
            PresentCondDataOnTree();
            PresentModelObjectsForSelection();
        }

        private void OnClosingForm(object sender, FormClosingEventArgs e)
        {
            project.Message -= Project_Message;
            project?.UnloadGeometry();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //var splitContainer = (SplitContainer)navigator.Parent.Parent;
            splitContainer3.Panel1Collapsed = !splitContainer3.Panel1Collapsed;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            var splitContainer = (SplitContainer)console.Parent.Parent;
            splitContainer.Panel2Collapsed = !splitContainer.Panel2Collapsed;
        }



        private async void экспортСеткиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var res = await dataController.ExportMesh(project);

                if (res == null)
                    return;

                console.PrintInfo(res, Color.Green);
            }

            catch (Exception ex)
            {
                MessageBox.Show(Localization.Localization.GetErrorWithStackMessage(ex), Localization.Localization.GetErrorCaption());
            }
        }

        private async void добавитьСеткуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (project.HasProject)
                {
                    OpenFileDialog dialog = new OpenFileDialog();
                    dialog.Filter = meshFilter + "|" + projFilter;
                    if (dialog.ShowDialog() == DialogResult.Cancel)
                        return;

                    await LoadModelWithProgress(dialog.FileName, true);
                    // сбрасывать gmsh  не обязательно
                    //gmshController?.Gmsh?.Clear();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(Localization.Localization.GetErrorWithStackMessage(ex), Localization.Localization.GetErrorCaption());
                Application.OpenForms["Загрузка"]?.Close();
            }
        }

        private void addChamferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(addChamferToolStripMenuItem.Checked)
            {
                SelectedObjects = SelectionType.Curves;
                var synchronizationContext = SynchronizationContext.Current;
                var operationService = new SynchronizationContextChamferOperationService(synchronizationContext, RequestChamferByAngle, RequestChamferByLengths, RequestChamferPreview, RequestClearChamferPreview);
                ChamferWindowService.Show(operationService, () => synchronizationContext.Post(_ => OnChamferWindowClosed(), null));
            }
            else
                ChamferWindowService.Close();
        }

        /// <summary>
        /// Приводит состояние пункта меню построения фаски в соответствие с фактическим состоянием окна.
        /// </summary>
        /// <remarks>
        /// Вызывается после закрытия окна любым способом: как по команде <see cref="ChamferWindowService.Close"/>,
        /// так и при закрытии окна самим пользователем. Программное снятие флажка не вызывает событие
        /// <see cref="ToolStripItem.Click"/>, поэтому повторного закрытия окна не происходит.
        /// Выполняется в UI-потоке WinForms.
        /// </remarks>
        private void OnChamferWindowClosed()
        {
            if (IsDisposed || Disposing)
                return;

            addChamferToolStripMenuItem.Checked = false;
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
    }
}
