
using WinGUI.Navigator;
using WinGUI.Scene.EventsArgs;
using WinGUI.SettingsControls;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WinGUI
{
    partial class BaseForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseForm));
            toolStripContainer = new ToolStripContainer();
            statusStrip = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();
            lblVersion = new ToolStripStatusLabel();
            webPageLabel = new ToolStripStatusLabel();
            splitContainer3 = new UserControlsEx.SplitContainerEx();
            splitContainer2 = new UserControlsEx.SplitContainerEx();
            btnSelect = new Button();
            btnDisplayStates = new Button();
            btnRotVert90 = new Button();
            sceneImageList = new ImageList(components);
            btnRotHor90 = new Button();
            btnRotZ = new Button();
            btnRotY = new Button();
            btnRotX = new Button();
            btnZY = new Button();
            btnZX = new Button();
            btnXY = new Button();
            btnDisplayViews = new Button();
            btnShowInsideObjects = new Button();
            btnFitToScreen = new Button();
            btnShowSidesRibs = new Button();
            btnShowRibs = new Button();
            btnShowSides = new Button();
            btnBazis = new Button();
            btnBorder = new Button();
            btnMakeScreenShot = new Button();
            btnAdvSelection = new Button();
            scene = new OpenTK.GLControl.GLControl();
            console = new WinGUI.Console.ConsoleControl();
            menuStrip = new MenuStrip();
            файлToolStripMenuItem = new ToolStripMenuItem();
            создатьToolStripMenuItem = new ToolStripMenuItem();
            открытьToolStripMenuItem = new ToolStripMenuItem();
            добавитьToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator = new ToolStripSeparator();
            сохранитьToolStripMenuItem = new ToolStripMenuItem();
            сохранитькакToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripSeparator2 = new ToolStripSeparator();
            выходToolStripMenuItem = new ToolStripMenuItem();
            viewMenuItem = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripMenuItem();
            геометрияToolStripMenuItem = new ToolStripMenuItem();
            создатьТочкуToolStripMenuItem = new ToolStripMenuItem();
            создатьЛиниюToolStripMenuItem = new ToolStripMenuItem();
            создатьПлоскостьToolStripMenuItem = new ToolStripMenuItem();
            создатьОбъемToolStripMenuItem = new ToolStripMenuItem();
            addChamferToolStripMenuItem = new ToolStripMenuItem();
            сеткаToolStripMenuItem = new ToolStripMenuItem();
            загрузитьgeoToolStripMenuItem = new ToolStripMenuItem();
            сформироватьgeoToolStripMenuItem = new ToolStripMenuItem();
            dToolStripMenuItem = new ToolStripMenuItem();
            наToolStripMenuItem = new ToolStripMenuItem();
            dToolStripMenuItem1 = new ToolStripMenuItem();
            уплотнитьToolStripMenuItem = new ToolStripMenuItem();
            наПоверхности3DToolStripMenuItem = new ToolStripMenuItem();
            наПоверхностиГеометрииToolStripMenuItem = new ToolStripMenuItem();
            квадратизацияСуществующейToolStripMenuItem = new ToolStripMenuItem();
            dToolStripMenuItem2 = new ToolStripMenuItem();
            dataBasesMenuItem = new ToolStripMenuItem();
            материалыMenuItem = new ToolStripMenuItem();
            функцииMenuItem = new ToolStripMenuItem();
            tasksMenuItem = new ToolStripMenuItem();
            создатьToolStripMenuItem1 = new ToolStripMenuItem();
            мастерToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            загрузитьМастерToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            показатьНаДиаграммеToolStripMenuItem = new ToolStripMenuItem();
            расчетыToolStripMenuItem = new ToolStripMenuItem();
            открытьИнструкцииToolStripMenuItem = new ToolStripMenuItem();
            сформироватьИнструкцииToolStripMenuItem = new ToolStripMenuItem();
            запуститьToolStripMenuItem = new ToolStripMenuItem();
            остановитьToolStripMenuItem = new ToolStripMenuItem();
            результатыMenuItem = new ToolStripMenuItem();
            открытьToolStripMenuItem1 = new ToolStripMenuItem();
            объединитьToolStripMenuItem = new ToolStripMenuItem();
            построитьГрафикToolStripMenuItem = new ToolStripMenuItem();
            построитьДиаграммуToolStripMenuItem = new ToolStripMenuItem();
            создатьАнимациюToolStripMenuItem = new ToolStripMenuItem();
            экспортироватьРезультатыToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem4 = new ToolStripMenuItem();
            инструментыToolStripMenuItem = new ToolStripMenuItem();
            измеритьToolStripMenuItem = new ToolStripMenuItem();
            скрытьПлоскостьюToolStripMenuItem = new ToolStripMenuItem();
            рассечьПлоскостьюToolStripMenuItem = new ToolStripMenuItem();
            настройкиToolStripMenuItem = new ToolStripMenuItem();
            справкаToolStripMenuItem = new ToolStripMenuItem();
            содержаниеToolStripMenuItem = new ToolStripMenuItem();
            опрограммеToolStripMenuItem = new ToolStripMenuItem();
            лицензияToolStripMenuItem = new ToolStripMenuItem();
            сведенияMenuItem = new ToolStripMenuItem();
            cntrНавигатор = new UserControlsEx.SplitContainerEx();
            navigator = new NavigatorControl();
            tableLayoutPanel1 = new TableLayoutPanel();
            checkPlayerControl = new WinGUI.Player.PlayerControl();
            propertiesPanel = new WinGUI.PropertiesPanel.PropertiesPanelControl();
            btnTabНастройки = new Button();
            btnTabГант = new Button();
            btnTabФункции = new Button();
            btnTabМатериалы = new Button();
            btnTabНавигатор = new Button();
            contextMenu = new ContextMenuStrip(components);
            создатьГруппуItem = new ToolStripMenuItem();
            скрытьВыбранноеItem = new ToolStripMenuItem();
            показатьСкрытыеItem = new ToolStripMenuItem();
            menuItem_InfoSelectedObjects = new ToolStripMenuItem();
            menuItem_SetRotPoint = new ToolStripMenuItem();
            показатьСопряженныеItem = new ToolStripMenuItem();
            menuItem_DeleteSelectedObjects = new ToolStripMenuItem();
            toolStripContainer.BottomToolStripPanel.SuspendLayout();
            toolStripContainer.ContentPanel.SuspendLayout();
            toolStripContainer.TopToolStripPanel.SuspendLayout();
            toolStripContainer.SuspendLayout();
            statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer3).BeginInit();
            splitContainer3.Panel2.SuspendLayout();
            splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cntrНавигатор).BeginInit();
            cntrНавигатор.Panel1.SuspendLayout();
            cntrНавигатор.Panel2.SuspendLayout();
            cntrНавигатор.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            contextMenu.SuspendLayout();
            SuspendLayout();
            // 
            // toolStripContainer
            // 
            // 
            // toolStripContainer.BottomToolStripPanel
            // 
            toolStripContainer.BottomToolStripPanel.Controls.Add(statusStrip);
            // 
            // toolStripContainer.ContentPanel
            // 
            toolStripContainer.ContentPanel.BackColor = System.Drawing.SystemColors.Control;
            toolStripContainer.ContentPanel.Controls.Add(splitContainer3);
            resources.ApplyResources(toolStripContainer.ContentPanel, "toolStripContainer.ContentPanel");
            resources.ApplyResources(toolStripContainer, "toolStripContainer");
            toolStripContainer.Name = "toolStripContainer";
            // 
            // toolStripContainer.TopToolStripPanel
            // 
            toolStripContainer.TopToolStripPanel.BackColor = System.Drawing.SystemColors.Control;
            toolStripContainer.TopToolStripPanel.Controls.Add(menuStrip);
            resources.ApplyResources(toolStripContainer.TopToolStripPanel, "toolStripContainer.TopToolStripPanel");
            // 
            // statusStrip
            // 
            statusStrip.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(statusStrip, "statusStrip");
            statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            statusStrip.Items.AddRange(new ToolStripItem[] { lblStatus, lblVersion, webPageLabel });
            statusStrip.Name = "statusStrip";
            // 
            // lblStatus
            // 
            resources.ApplyResources(lblStatus, "lblStatus");
            lblStatus.BackColor = System.Drawing.SystemColors.Control;
            lblStatus.Margin = new Padding(5, 3, 0, 2);
            lblStatus.Name = "lblStatus";
            lblStatus.Spring = true;
            // 
            // lblVersion
            // 
            lblVersion.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Right;
            lblVersion.DisplayStyle = ToolStripItemDisplayStyle.Text;
            lblVersion.Name = "lblVersion";
            resources.ApplyResources(lblVersion, "lblVersion");
            // 
            // webPageLabel
            // 
            webPageLabel.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(webPageLabel, "webPageLabel");
            webPageLabel.IsLink = true;
            webPageLabel.LinkColor = System.Drawing.Color.OrangeRed;
            webPageLabel.Name = "webPageLabel";
            webPageLabel.Click += webPageLabel_Click;
            // 
            // splitContainer3
            // 
            resources.ApplyResources(splitContainer3, "splitContainer3");
            splitContainer3.IncrementButtonSize = new System.Drawing.Size(50, 5);
            splitContainer3.IncrementShifting = 50;
            splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel2
            // 
            splitContainer3.Panel2.Controls.Add(splitContainer2);
            splitContainer3.SwitchShifting = false;
            // 
            // splitContainer2
            // 
            resources.ApplyResources(splitContainer2, "splitContainer2");
            splitContainer2.IncrementButtonSize = new System.Drawing.Size(50, 5);
            splitContainer2.IncrementShifting = 50;
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(btnSelect);
            splitContainer2.Panel1.Controls.Add(btnDisplayStates);
            splitContainer2.Panel1.Controls.Add(btnRotVert90);
            splitContainer2.Panel1.Controls.Add(btnRotHor90);
            splitContainer2.Panel1.Controls.Add(btnRotZ);
            splitContainer2.Panel1.Controls.Add(btnRotY);
            splitContainer2.Panel1.Controls.Add(btnRotX);
            splitContainer2.Panel1.Controls.Add(btnZY);
            splitContainer2.Panel1.Controls.Add(btnZX);
            splitContainer2.Panel1.Controls.Add(btnXY);
            splitContainer2.Panel1.Controls.Add(btnDisplayViews);
            splitContainer2.Panel1.Controls.Add(btnShowInsideObjects);
            splitContainer2.Panel1.Controls.Add(btnFitToScreen);
            splitContainer2.Panel1.Controls.Add(btnShowSidesRibs);
            splitContainer2.Panel1.Controls.Add(btnShowRibs);
            splitContainer2.Panel1.Controls.Add(btnShowSides);
            splitContainer2.Panel1.Controls.Add(btnBazis);
            splitContainer2.Panel1.Controls.Add(btnBorder);
            splitContainer2.Panel1.Controls.Add(btnMakeScreenShot);
            splitContainer2.Panel1.Controls.Add(btnAdvSelection);
            splitContainer2.Panel1.Controls.Add(scene);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(console);
            splitContainer2.SwitchShifting = false;
            // 
            // btnSelect
            // 
            resources.ApplyResources(btnSelect, "btnSelect");
            btnSelect.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            btnSelect.Image = Properties.Resources.arrow_r;
            btnSelect.Name = "btnSelect";
            btnSelect.Tag = "False";
            btnSelect.UseVisualStyleBackColor = true;
            btnSelect.Click += btnSelect_Click;
            btnSelect.Leave += btnSelect_Leave;
            // 
            // btnDisplayStates
            // 
            resources.ApplyResources(btnDisplayStates, "btnDisplayStates");
            btnDisplayStates.Image = Properties.Resources.arrow_d;
            btnDisplayStates.Name = "btnDisplayStates";
            btnDisplayStates.Tag = "False";
            btnDisplayStates.UseVisualStyleBackColor = true;
            btnDisplayStates.Click += btnDisplayStates_Click;
            // 
            // btnRotVert90
            // 
            resources.ApplyResources(btnRotVert90, "btnRotVert90");
            btnRotVert90.ImageList = sceneImageList;
            btnRotVert90.Name = "btnRotVert90";
            btnRotVert90.UseVisualStyleBackColor = true;
            btnRotVert90.Click += btnRotVert90_Click;
            // 
            // sceneImageList
            // 
            sceneImageList.ColorDepth = ColorDepth.Depth8Bit;
            sceneImageList.ImageStream = (ImageListStreamer)resources.GetObject("sceneImageList.ImageStream");
            sceneImageList.TransparentColor = System.Drawing.Color.Transparent;
            sceneImageList.Images.SetKeyName(0, "");
            sceneImageList.Images.SetKeyName(1, "");
            sceneImageList.Images.SetKeyName(2, "");
            sceneImageList.Images.SetKeyName(3, "");
            sceneImageList.Images.SetKeyName(4, "");
            sceneImageList.Images.SetKeyName(5, "");
            sceneImageList.Images.SetKeyName(6, "");
            sceneImageList.Images.SetKeyName(7, "");
            sceneImageList.Images.SetKeyName(8, "");
            sceneImageList.Images.SetKeyName(9, "");
            sceneImageList.Images.SetKeyName(10, "");
            sceneImageList.Images.SetKeyName(11, "");
            sceneImageList.Images.SetKeyName(12, "");
            sceneImageList.Images.SetKeyName(13, "");
            sceneImageList.Images.SetKeyName(14, "");
            sceneImageList.Images.SetKeyName(15, "");
            sceneImageList.Images.SetKeyName(16, "");
            sceneImageList.Images.SetKeyName(17, "");
            // 
            // btnRotHor90
            // 
            resources.ApplyResources(btnRotHor90, "btnRotHor90");
            btnRotHor90.ImageList = sceneImageList;
            btnRotHor90.Name = "btnRotHor90";
            btnRotHor90.UseVisualStyleBackColor = true;
            btnRotHor90.Click += btnRotHor90_Click;
            // 
            // btnRotZ
            // 
            resources.ApplyResources(btnRotZ, "btnRotZ");
            btnRotZ.ImageList = sceneImageList;
            btnRotZ.Name = "btnRotZ";
            btnRotZ.Tag = "False";
            btnRotZ.UseVisualStyleBackColor = true;
            btnRotZ.Click += btnRotZ_Click;
            btnRotZ.Paint += btnRot_Paint;
            // 
            // btnRotY
            // 
            resources.ApplyResources(btnRotY, "btnRotY");
            btnRotY.ImageList = sceneImageList;
            btnRotY.Name = "btnRotY";
            btnRotY.Tag = "False";
            btnRotY.UseVisualStyleBackColor = true;
            btnRotY.Click += btnRotY_Click;
            btnRotY.Paint += btnRot_Paint;
            // 
            // btnRotX
            // 
            resources.ApplyResources(btnRotX, "btnRotX");
            btnRotX.ImageList = sceneImageList;
            btnRotX.Name = "btnRotX";
            btnRotX.Tag = "False";
            btnRotX.UseVisualStyleBackColor = true;
            btnRotX.Click += btnRotX_Click;
            btnRotX.Paint += btnRot_Paint;
            // 
            // btnZY
            // 
            resources.ApplyResources(btnZY, "btnZY");
            btnZY.ImageList = sceneImageList;
            btnZY.Name = "btnZY";
            btnZY.UseVisualStyleBackColor = true;
            btnZY.Click += btnZY_Click;
            // 
            // btnZX
            // 
            resources.ApplyResources(btnZX, "btnZX");
            btnZX.ImageList = sceneImageList;
            btnZX.Name = "btnZX";
            btnZX.UseVisualStyleBackColor = true;
            btnZX.Click += btnZX_Click;
            // 
            // btnXY
            // 
            resources.ApplyResources(btnXY, "btnXY");
            btnXY.ImageList = sceneImageList;
            btnXY.Name = "btnXY";
            btnXY.UseVisualStyleBackColor = true;
            btnXY.Click += btnXY_Click;
            // 
            // btnDisplayViews
            // 
            resources.ApplyResources(btnDisplayViews, "btnDisplayViews");
            btnDisplayViews.Image = Properties.Resources.arrow_r;
            btnDisplayViews.Name = "btnDisplayViews";
            btnDisplayViews.Tag = "False";
            btnDisplayViews.UseVisualStyleBackColor = true;
            btnDisplayViews.Click += btnDisplayViews_Click;
            // 
            // btnShowInsideObjects
            // 
            resources.ApplyResources(btnShowInsideObjects, "btnShowInsideObjects");
            btnShowInsideObjects.ImageList = sceneImageList;
            btnShowInsideObjects.Name = "btnShowInsideObjects";
            btnShowInsideObjects.Tag = "False";
            btnShowInsideObjects.UseVisualStyleBackColor = true;
            btnShowInsideObjects.Click += btnShowInsideObjects_Click;
            // 
            // btnFitToScreen
            // 
            resources.ApplyResources(btnFitToScreen, "btnFitToScreen");
            btnFitToScreen.ImageList = sceneImageList;
            btnFitToScreen.Name = "btnFitToScreen";
            btnFitToScreen.UseVisualStyleBackColor = true;
            btnFitToScreen.Click += btnFitToScreen_Click;
            // 
            // btnShowSidesRibs
            // 
            resources.ApplyResources(btnShowSidesRibs, "btnShowSidesRibs");
            btnShowSidesRibs.BackColor = System.Drawing.SystemColors.Control;
            btnShowSidesRibs.ImageList = sceneImageList;
            btnShowSidesRibs.Name = "btnShowSidesRibs";
            btnShowSidesRibs.UseVisualStyleBackColor = false;
            btnShowSidesRibs.Click += btnShowSidesRibs_Click;
            // 
            // btnShowRibs
            // 
            resources.ApplyResources(btnShowRibs, "btnShowRibs");
            btnShowRibs.BackColor = System.Drawing.SystemColors.Control;
            btnShowRibs.ImageList = sceneImageList;
            btnShowRibs.Name = "btnShowRibs";
            btnShowRibs.UseVisualStyleBackColor = false;
            btnShowRibs.Click += btnShowRibs_Click;
            // 
            // btnShowSides
            // 
            resources.ApplyResources(btnShowSides, "btnShowSides");
            btnShowSides.ImageList = sceneImageList;
            btnShowSides.Name = "btnShowSides";
            btnShowSides.UseVisualStyleBackColor = true;
            btnShowSides.Click += btnShowSides_Click;
            // 
            // btnBazis
            // 
            resources.ApplyResources(btnBazis, "btnBazis");
            btnBazis.ImageList = sceneImageList;
            btnBazis.Name = "btnBazis";
            btnBazis.Tag = "False";
            btnBazis.UseVisualStyleBackColor = true;
            btnBazis.Click += btnBazis_Click;
            btnBazis.Paint += btnBazis_Paint;
            // 
            // btnBorder
            // 
            resources.ApplyResources(btnBorder, "btnBorder");
            btnBorder.ImageList = sceneImageList;
            btnBorder.Name = "btnBorder";
            btnBorder.Tag = "False";
            btnBorder.UseVisualStyleBackColor = true;
            btnBorder.Click += btnBorder_Click;
            // 
            // btnMakeScreenShot
            // 
            resources.ApplyResources(btnMakeScreenShot, "btnMakeScreenShot");
            btnMakeScreenShot.ImageList = sceneImageList;
            btnMakeScreenShot.Name = "btnMakeScreenShot";
            btnMakeScreenShot.UseVisualStyleBackColor = true;
            btnMakeScreenShot.Click += btnMakeScreenShot_Click;
            // 
            // btnAdvSelection
            // 
            resources.ApplyResources(btnAdvSelection, "btnAdvSelection");
            btnAdvSelection.ImageList = sceneImageList;
            btnAdvSelection.Name = "btnAdvSelection";
            btnAdvSelection.Tag = "False";
            btnAdvSelection.UseVisualStyleBackColor = true;
            btnAdvSelection.Click += btnAdvSelection_Click;
            btnAdvSelection.Paint += btnSelection_Paint;
            // 
            // scene
            // 
            scene.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            scene.APIVersion = new Version(3, 3, 0, 0);
            scene.BackColor = System.Drawing.Color.Silver;
            resources.ApplyResources(scene, "scene");
            scene.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            scene.IsEventDriven = true;
            scene.Name = "scene";
            scene.Profile = OpenTK.Windowing.Common.ContextProfile.Compatability;
            scene.SharedContext = null;
            scene.KeyDown += GlControl_KeyDown;
            scene.MouseWheel += GlControl_MouseWheel;
            // 
            // console
            // 
            resources.ApplyResources(console, "console");
            console.BorderStyle = BorderStyle.FixedSingle;
            console.CheckPrintElemsInfo = false;
            console.CheckPrintNodesInfo = false;
            console.DownColor = System.Drawing.Color.Gainsboro;
            console.HeaderColor = System.Drawing.Color.Black;
            console.HeaderName = "Console";
            console.IsPinndable = false;
            console.Name = "console";
            console.SessionNumber = 7742;
            console.UpColor = System.Drawing.Color.Gainsboro;
            console.ConsoleCommandEnteredEvent += ExecuteCommand;
            console.CommandsListRequestedEvent += PrintAllCommands;
            // 
            // menuStrip
            // 
            menuStrip.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(menuStrip, "menuStrip");
            menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            menuStrip.Items.AddRange(new ToolStripItem[] { файлToolStripMenuItem, viewMenuItem, геометрияToolStripMenuItem, сеткаToolStripMenuItem, dataBasesMenuItem, tasksMenuItem, расчетыToolStripMenuItem, результатыMenuItem, инструментыToolStripMenuItem, настройкиToolStripMenuItem, справкаToolStripMenuItem, лицензияToolStripMenuItem });
            menuStrip.Name = "menuStrip";
            menuStrip.RenderMode = ToolStripRenderMode.Professional;
            // 
            // файлToolStripMenuItem
            // 
            файлToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { создатьToolStripMenuItem, открытьToolStripMenuItem, добавитьToolStripMenuItem, toolStripSeparator, сохранитьToolStripMenuItem, сохранитькакToolStripMenuItem, toolStripSeparator1, toolStripSeparator2, выходToolStripMenuItem });
            файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            resources.ApplyResources(файлToolStripMenuItem, "файлToolStripMenuItem");
            // 
            // создатьToolStripMenuItem
            // 
            resources.ApplyResources(создатьToolStripMenuItem, "создатьToolStripMenuItem");
            создатьToolStripMenuItem.Name = "создатьToolStripMenuItem";
            создатьToolStripMenuItem.Click += создатьToolStripMenuItem_Click;
            // 
            // открытьToolStripMenuItem
            // 
            resources.ApplyResources(открытьToolStripMenuItem, "открытьToolStripMenuItem");
            открытьToolStripMenuItem.Name = "открытьToolStripMenuItem";
            открытьToolStripMenuItem.Click += открытьToolStripMenuItem_Click;
            // 
            // добавитьToolStripMenuItem
            // 
            добавитьToolStripMenuItem.Name = "добавитьToolStripMenuItem";
            resources.ApplyResources(добавитьToolStripMenuItem, "добавитьToolStripMenuItem");
            добавитьToolStripMenuItem.Click += добавитьСеткуToolStripMenuItem_Click;
            // 
            // toolStripSeparator
            // 
            toolStripSeparator.Name = "toolStripSeparator";
            resources.ApplyResources(toolStripSeparator, "toolStripSeparator");
            // 
            // сохранитьToolStripMenuItem
            // 
            resources.ApplyResources(сохранитьToolStripMenuItem, "сохранитьToolStripMenuItem");
            сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            сохранитьToolStripMenuItem.Click += сохранитьToolStripMenuItem_Click;
            // 
            // сохранитькакToolStripMenuItem
            // 
            сохранитькакToolStripMenuItem.Name = "сохранитькакToolStripMenuItem";
            resources.ApplyResources(сохранитькакToolStripMenuItem, "сохранитькакToolStripMenuItem");
            сохранитькакToolStripMenuItem.Click += сохранитькакToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(toolStripSeparator1, "toolStripSeparator1");
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(toolStripSeparator2, "toolStripSeparator2");
            // 
            // выходToolStripMenuItem
            // 
            выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            resources.ApplyResources(выходToolStripMenuItem, "выходToolStripMenuItem");
            выходToolStripMenuItem.Click += выходToolStripMenuItem_Click;
            // 
            // viewMenuItem
            // 
            viewMenuItem.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem2, toolStripMenuItem3 });
            viewMenuItem.Name = "viewMenuItem";
            resources.ApplyResources(viewMenuItem, "viewMenuItem");
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            resources.ApplyResources(toolStripMenuItem2, "toolStripMenuItem2");
            toolStripMenuItem2.Click += toolStripMenuItem2_Click;
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            resources.ApplyResources(toolStripMenuItem3, "toolStripMenuItem3");
            toolStripMenuItem3.Click += toolStripMenuItem3_Click;
            // 
            // геометрияToolStripMenuItem
            // 
            геометрияToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { создатьТочкуToolStripMenuItem, создатьЛиниюToolStripMenuItem, создатьПлоскостьToolStripMenuItem, создатьОбъемToolStripMenuItem, addChamferToolStripMenuItem });
            resources.ApplyResources(геометрияToolStripMenuItem, "геометрияToolStripMenuItem");
            геометрияToolStripMenuItem.Name = "геометрияToolStripMenuItem";
            // 
            // создатьТочкуToolStripMenuItem
            // 
            создатьТочкуToolStripMenuItem.Name = "создатьТочкуToolStripMenuItem";
            resources.ApplyResources(создатьТочкуToolStripMenuItem, "создатьТочкуToolStripMenuItem");
            // 
            // создатьЛиниюToolStripMenuItem
            // 
            создатьЛиниюToolStripMenuItem.Name = "создатьЛиниюToolStripMenuItem";
            resources.ApplyResources(создатьЛиниюToolStripMenuItem, "создатьЛиниюToolStripMenuItem");
            // 
            // создатьПлоскостьToolStripMenuItem
            // 
            создатьПлоскостьToolStripMenuItem.Name = "создатьПлоскостьToolStripMenuItem";
            resources.ApplyResources(создатьПлоскостьToolStripMenuItem, "создатьПлоскостьToolStripMenuItem");
            // 
            // создатьОбъемToolStripMenuItem
            // 
            создатьОбъемToolStripMenuItem.Name = "создатьОбъемToolStripMenuItem";
            resources.ApplyResources(создатьОбъемToolStripMenuItem, "создатьОбъемToolStripMenuItem");
            // 
            // addChamferToolStripMenuItem
            // 
            addChamferToolStripMenuItem.CheckOnClick = true;
            addChamferToolStripMenuItem.Name = "addChamferToolStripMenuItem";
            resources.ApplyResources(addChamferToolStripMenuItem, "addChamferToolStripMenuItem");
            addChamferToolStripMenuItem.Click += addChamferToolStripMenuItem_Click;
            // 
            // сеткаToolStripMenuItem
            // 
            сеткаToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { загрузитьgeoToolStripMenuItem, сформироватьgeoToolStripMenuItem, dToolStripMenuItem, dToolStripMenuItem1, dToolStripMenuItem2 });
            resources.ApplyResources(сеткаToolStripMenuItem, "сеткаToolStripMenuItem");
            сеткаToolStripMenuItem.Name = "сеткаToolStripMenuItem";
            // 
            // загрузитьgeoToolStripMenuItem
            // 
            загрузитьgeoToolStripMenuItem.Name = "загрузитьgeoToolStripMenuItem";
            resources.ApplyResources(загрузитьgeoToolStripMenuItem, "загрузитьgeoToolStripMenuItem");
            загрузитьgeoToolStripMenuItem.Click += загрузитьgeoToolStripMenuItem_Click;
            // 
            // сформироватьgeoToolStripMenuItem
            // 
            сформироватьgeoToolStripMenuItem.Name = "сформироватьgeoToolStripMenuItem";
            resources.ApplyResources(сформироватьgeoToolStripMenuItem, "сформироватьgeoToolStripMenuItem");
            сформироватьgeoToolStripMenuItem.Click += сформироватьgeoToolStripMenuItem_Click;
            // 
            // dToolStripMenuItem
            // 
            dToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { наToolStripMenuItem });
            dToolStripMenuItem.Name = "dToolStripMenuItem";
            resources.ApplyResources(dToolStripMenuItem, "dToolStripMenuItem");
            // 
            // наToolStripMenuItem
            // 
            наToolStripMenuItem.Name = "наToolStripMenuItem";
            resources.ApplyResources(наToolStripMenuItem, "наToolStripMenuItem");
            наToolStripMenuItem.Click += наПоверхности2DToolStripMenuItem_Click;
            // 
            // dToolStripMenuItem1
            // 
            dToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { уплотнитьToolStripMenuItem, наПоверхности3DToolStripMenuItem, наПоверхностиГеометрииToolStripMenuItem, квадратизацияСуществующейToolStripMenuItem });
            dToolStripMenuItem1.Name = "dToolStripMenuItem1";
            resources.ApplyResources(dToolStripMenuItem1, "dToolStripMenuItem1");
            // 
            // уплотнитьToolStripMenuItem
            // 
            resources.ApplyResources(уплотнитьToolStripMenuItem, "уплотнитьToolStripMenuItem");
            уплотнитьToolStripMenuItem.Name = "уплотнитьToolStripMenuItem";
            уплотнитьToolStripMenuItem.Click += уплотнитьToolStripMenuItem_Click;
            // 
            // наПоверхности3DToolStripMenuItem
            // 
            наПоверхности3DToolStripMenuItem.Name = "наПоверхности3DToolStripMenuItem";
            resources.ApplyResources(наПоверхности3DToolStripMenuItem, "наПоверхности3DToolStripMenuItem");
            наПоверхности3DToolStripMenuItem.Click += наПоверхности3DToolStripMenuItem_Click;
            // 
            // наПоверхностиГеометрииToolStripMenuItem
            // 
            наПоверхностиГеометрииToolStripMenuItem.Name = "наПоверхностиГеометрииToolStripMenuItem";
            resources.ApplyResources(наПоверхностиГеометрииToolStripMenuItem, "наПоверхностиГеометрииToolStripMenuItem");
            наПоверхностиГеометрииToolStripMenuItem.Click += наПоверхностиГеометрииToolStripMenuItem_Click;
            // 
            // квадратизацияСуществующейToolStripMenuItem
            // 
            resources.ApplyResources(квадратизацияСуществующейToolStripMenuItem, "квадратизацияСуществующейToolStripMenuItem");
            квадратизацияСуществующейToolStripMenuItem.Name = "квадратизацияСуществующейToolStripMenuItem";
            квадратизацияСуществующейToolStripMenuItem.Click += квадратизацияСуществующейToolStripMenuItem_Click;
            // 
            // dToolStripMenuItem2
            // 
            dToolStripMenuItem2.Name = "dToolStripMenuItem2";
            resources.ApplyResources(dToolStripMenuItem2, "dToolStripMenuItem2");
            dToolStripMenuItem2.Click += создать3DСеткуToolStripMenuItem_Click;
            // 
            // dataBasesMenuItem
            // 
            dataBasesMenuItem.DropDownItems.AddRange(new ToolStripItem[] { материалыMenuItem, функцииMenuItem });
            resources.ApplyResources(dataBasesMenuItem, "dataBasesMenuItem");
            dataBasesMenuItem.Name = "dataBasesMenuItem";
            // 
            // материалыMenuItem
            // 
            материалыMenuItem.CheckOnClick = true;
            материалыMenuItem.Name = "материалыMenuItem";
            resources.ApplyResources(материалыMenuItem, "материалыMenuItem");
            материалыMenuItem.Click += материалыMenuItem_Click;
            // 
            // функцииMenuItem
            // 
            функцииMenuItem.CheckOnClick = true;
            функцииMenuItem.Name = "функцииMenuItem";
            resources.ApplyResources(функцииMenuItem, "функцииMenuItem");
            функцииMenuItem.Click += функцииMenuItem_Click;
            // 
            // tasksMenuItem
            // 
            tasksMenuItem.DropDownItems.AddRange(new ToolStripItem[] { создатьToolStripMenuItem1, мастерToolStripMenuItem, показатьНаДиаграммеToolStripMenuItem });
            resources.ApplyResources(tasksMenuItem, "tasksMenuItem");
            tasksMenuItem.Name = "tasksMenuItem";
            // 
            // создатьToolStripMenuItem1
            // 
            создатьToolStripMenuItem1.Name = "создатьToolStripMenuItem1";
            resources.ApplyResources(создатьToolStripMenuItem1, "создатьToolStripMenuItem1");
            создатьToolStripMenuItem1.Click += создатьЗадачуToolStripMenuItem_Click;
            // 
            // мастерToolStripMenuItem
            // 
            мастерToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { toolStripSeparator3, загрузитьМастерToolStripMenuItem, toolStripSeparator4 });
            мастерToolStripMenuItem.Name = "мастерToolStripMenuItem";
            resources.ApplyResources(мастерToolStripMenuItem, "мастерToolStripMenuItem");
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(toolStripSeparator3, "toolStripSeparator3");
            // 
            // загрузитьМастерToolStripMenuItem
            // 
            загрузитьМастерToolStripMenuItem.Name = "загрузитьМастерToolStripMenuItem";
            resources.ApplyResources(загрузитьМастерToolStripMenuItem, "загрузитьМастерToolStripMenuItem");
            загрузитьМастерToolStripMenuItem.Click += загрузитьМастерToolStripMenuItem_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(toolStripSeparator4, "toolStripSeparator4");
            // 
            // показатьНаДиаграммеToolStripMenuItem
            // 
            показатьНаДиаграммеToolStripMenuItem.CheckOnClick = true;
            показатьНаДиаграммеToolStripMenuItem.Name = "показатьНаДиаграммеToolStripMenuItem";
            resources.ApplyResources(показатьНаДиаграммеToolStripMenuItem, "показатьНаДиаграммеToolStripMenuItem");
            показатьНаДиаграммеToolStripMenuItem.Click += показатьНаДиаграммеToolStripMenuItem_Click;
            // 
            // расчетыToolStripMenuItem
            // 
            расчетыToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { открытьИнструкцииToolStripMenuItem, сформироватьИнструкцииToolStripMenuItem, запуститьToolStripMenuItem, остановитьToolStripMenuItem });
            resources.ApplyResources(расчетыToolStripMenuItem, "расчетыToolStripMenuItem");
            расчетыToolStripMenuItem.Name = "расчетыToolStripMenuItem";
            // 
            // открытьИнструкцииToolStripMenuItem
            // 
            открытьИнструкцииToolStripMenuItem.Name = "открытьИнструкцииToolStripMenuItem";
            resources.ApplyResources(открытьИнструкцииToolStripMenuItem, "открытьИнструкцииToolStripMenuItem");
            открытьИнструкцииToolStripMenuItem.Click += открытьИнструкцииToolStripMenuItem_Click;
            // 
            // сформироватьИнструкцииToolStripMenuItem
            // 
            сформироватьИнструкцииToolStripMenuItem.Name = "сформироватьИнструкцииToolStripMenuItem";
            resources.ApplyResources(сформироватьИнструкцииToolStripMenuItem, "сформироватьИнструкцииToolStripMenuItem");
            сформироватьИнструкцииToolStripMenuItem.Click += сформироватьИнструкцииToolStripMenuItem_Click;
            // 
            // запуститьToolStripMenuItem
            // 
            запуститьToolStripMenuItem.Name = "запуститьToolStripMenuItem";
            resources.ApplyResources(запуститьToolStripMenuItem, "запуститьToolStripMenuItem");
            запуститьToolStripMenuItem.Click += запуститьToolStripMenuItem_Click;
            // 
            // остановитьToolStripMenuItem
            // 
            остановитьToolStripMenuItem.Name = "остановитьToolStripMenuItem";
            resources.ApplyResources(остановитьToolStripMenuItem, "остановитьToolStripMenuItem");
            остановитьToolStripMenuItem.Click += остановитьToolStripMenuItem_Click;
            // 
            // результатыMenuItem
            // 
            результатыMenuItem.DropDownItems.AddRange(new ToolStripItem[] { открытьToolStripMenuItem1, объединитьToolStripMenuItem, построитьГрафикToolStripMenuItem, построитьДиаграммуToolStripMenuItem, создатьАнимациюToolStripMenuItem, экспортироватьРезультатыToolStripMenuItem, toolStripMenuItem4 });
            resources.ApplyResources(результатыMenuItem, "результатыMenuItem");
            результатыMenuItem.Name = "результатыMenuItem";
            // 
            // открытьToolStripMenuItem1
            // 
            открытьToolStripMenuItem1.Name = "открытьToolStripMenuItem1";
            resources.ApplyResources(открытьToolStripMenuItem1, "открытьToolStripMenuItem1");
            открытьToolStripMenuItem1.Click += открытьToolStripMenuItem1_Click;
            // 
            // объединитьToolStripMenuItem
            // 
            resources.ApplyResources(объединитьToolStripMenuItem, "объединитьToolStripMenuItem");
            объединитьToolStripMenuItem.Name = "объединитьToolStripMenuItem";
            объединитьToolStripMenuItem.Click += MergeDataBase_Click;
            // 
            // построитьГрафикToolStripMenuItem
            // 
            построитьГрафикToolStripMenuItem.Name = "построитьГрафикToolStripMenuItem";
            resources.ApplyResources(построитьГрафикToolStripMenuItem, "построитьГрафикToolStripMenuItem");
            построитьГрафикToolStripMenuItem.Click += построитьГрафикToolStripMenuItem_Click;
            // 
            // построитьДиаграммуToolStripMenuItem
            // 
            построитьДиаграммуToolStripMenuItem.Name = "построитьДиаграммуToolStripMenuItem";
            resources.ApplyResources(построитьДиаграммуToolStripMenuItem, "построитьДиаграммуToolStripMenuItem");
            построитьДиаграммуToolStripMenuItem.Click += построитьДиаграммуToolStripMenuItem_Click;
            // 
            // создатьАнимациюToolStripMenuItem
            // 
            создатьАнимациюToolStripMenuItem.CheckOnClick = true;
            создатьАнимациюToolStripMenuItem.Name = "создатьАнимациюToolStripMenuItem";
            resources.ApplyResources(создатьАнимациюToolStripMenuItem, "создатьАнимациюToolStripMenuItem");
            создатьАнимациюToolStripMenuItem.Click += создатьАнимациюToolStripMenuItem_Click;
            // 
            // экспортироватьРезультатыToolStripMenuItem
            // 
            resources.ApplyResources(экспортироватьРезультатыToolStripMenuItem, "экспортироватьРезультатыToolStripMenuItem");
            экспортироватьРезультатыToolStripMenuItem.Name = "экспортироватьРезультатыToolStripMenuItem";
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.CheckOnClick = true;
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            resources.ApplyResources(toolStripMenuItem4, "toolStripMenuItem4");
            toolStripMenuItem4.Click += отзеркаливаниеToolStripMenuItem_Click;
            // 
            // инструментыToolStripMenuItem
            // 
            инструментыToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { измеритьToolStripMenuItem, скрытьПлоскостьюToolStripMenuItem, рассечьПлоскостьюToolStripMenuItem });
            resources.ApplyResources(инструментыToolStripMenuItem, "инструментыToolStripMenuItem");
            инструментыToolStripMenuItem.Name = "инструментыToolStripMenuItem";
            // 
            // измеритьToolStripMenuItem
            // 
            измеритьToolStripMenuItem.CheckOnClick = true;
            измеритьToolStripMenuItem.Name = "измеритьToolStripMenuItem";
            resources.ApplyResources(измеритьToolStripMenuItem, "измеритьToolStripMenuItem");
            измеритьToolStripMenuItem.Click += измеритьToolStripMenuItem_Click;
            // 
            // скрытьПлоскостьюToolStripMenuItem
            // 
            скрытьПлоскостьюToolStripMenuItem.CheckOnClick = true;
            скрытьПлоскостьюToolStripMenuItem.Name = "скрытьПлоскостьюToolStripMenuItem";
            resources.ApplyResources(скрытьПлоскостьюToolStripMenuItem, "скрытьПлоскостьюToolStripMenuItem");
            скрытьПлоскостьюToolStripMenuItem.Click += скрытьПлоскостьюToolStripMenuItem_Click;
            // 
            // рассечьПлоскостьюToolStripMenuItem
            // 
            resources.ApplyResources(рассечьПлоскостьюToolStripMenuItem, "рассечьПлоскостьюToolStripMenuItem");
            рассечьПлоскостьюToolStripMenuItem.Name = "рассечьПлоскостьюToolStripMenuItem";
            // 
            // настройкиToolStripMenuItem
            // 
            настройкиToolStripMenuItem.CheckOnClick = true;
            настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            resources.ApplyResources(настройкиToolStripMenuItem, "настройкиToolStripMenuItem");
            настройкиToolStripMenuItem.Click += настройкиToolStripMenuItem_Click;
            // 
            // справкаToolStripMenuItem
            // 
            справкаToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { содержаниеToolStripMenuItem, опрограммеToolStripMenuItem });
            справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            resources.ApplyResources(справкаToolStripMenuItem, "справкаToolStripMenuItem");
            // 
            // содержаниеToolStripMenuItem
            // 
            содержаниеToolStripMenuItem.Name = "содержаниеToolStripMenuItem";
            resources.ApplyResources(содержаниеToolStripMenuItem, "содержаниеToolStripMenuItem");
            содержаниеToolStripMenuItem.Click += содержаниеToolStripMenuItem_Click;
            // 
            // опрограммеToolStripMenuItem
            // 
            опрограммеToolStripMenuItem.Name = "опрограммеToolStripMenuItem";
            resources.ApplyResources(опрограммеToolStripMenuItem, "опрограммеToolStripMenuItem");
            опрограммеToolStripMenuItem.Click += опрограммеToolStripMenuItem_Click;
            // 
            // лицензияToolStripMenuItem
            // 
            лицензияToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { сведенияMenuItem });
            лицензияToolStripMenuItem.Name = "лицензияToolStripMenuItem";
            resources.ApplyResources(лицензияToolStripMenuItem, "лицензияToolStripMenuItem");
            // 
            // сведенияMenuItem
            // 
            сведенияMenuItem.Name = "сведенияMenuItem";
            resources.ApplyResources(сведенияMenuItem, "сведенияMenuItem");
            сведенияMenuItem.Click += сведенияMenuItem_Click;
            // 
            // cntrНавигатор
            // 
            resources.ApplyResources(cntrНавигатор, "cntrНавигатор");
            cntrНавигатор.IncrementButtonSize = new System.Drawing.Size(50, 5);
            cntrНавигатор.IncrementShifting = 50;
            cntrНавигатор.Name = "cntrНавигатор";
            // 
            // cntrНавигатор.Panel1
            // 
            cntrНавигатор.Panel1.Controls.Add(navigator);
            // 
            // cntrНавигатор.Panel2
            // 
            cntrНавигатор.Panel2.Controls.Add(tableLayoutPanel1);
            cntrНавигатор.SwitchShifting = false;
            // 
            // navigator
            // 
            navigator.BackColor = System.Drawing.Color.Gainsboro;
            navigator.BorderStyle = BorderStyle.FixedSingle;
            navigator.CollapseIndex = 7;
            resources.ApplyResources(navigator, "navigator");
            navigator.DownColor = System.Drawing.Color.Gainsboro;
            navigator.DrawNodeFrozen = false;
            navigator.ExpandIndex = 8;
            navigator.HeaderColor = System.Drawing.Color.Black;
            navigator.HeaderName = "Навигатор";
            navigator.IsPinndable = false;
            navigator.Name = "navigator";
            navigator.ProjectInfoIndex = 0;
            navigator.UpColor = System.Drawing.Color.Gainsboro;
            navigator.HideResultsEvent += navigator_HideResultsEvent;
            navigator.RemoveResultsEvent += navigator_RemoveResultsEvent;
            navigator.RemoveAllConditionsEvent += navigator_RemoveAllConditionsEvent;
            navigator.DelAllGroupsEvent += navigator_DelAllGroupsEvent;
            navigator.ShowAllGroupsEvent += navigator_ShowAllGroupsEvent;
            navigator.HideAllGroupsEvent += navigator_HideAllGroupsEvent;
            navigator.ChangeAllGeoViewStateEvent += navigator_ChangeAllObjectsViewStateEvent;
            navigator.DelAllGeoEvent += navigator_DelAllObjectsEvent;
            navigator.DelAllMeshEvent += navigator_DelAllObjectsEvent;
            navigator.ChangeAllMeshViewStateEvent += navigator_ChangeAllObjectsViewStateEvent;
            navigator.ShowSetEvent += navigator_ShowSetEvent;
            navigator.HideSetEvent += navigator_HideSetEvent;
            navigator.DelSetEvent += navigator_DelSetEvent;
            navigator.SelectSetEvent += navigator_SelectSetEvent;
            navigator.GetSetsInfoEvent += navigator_GetSetsInfoEvent;
            navigator.SelectGroupEvent += navigator_SelectGroupEvent;
            navigator.DelGroupEvent += navigator_DelGroupEvent;
            navigator.HideGroupEvent += navigator_HideGroupEvent;
            navigator.ShowGroupEvent += navigator_ShowGroupEvent;
            navigator.EditGroupEvent += EditGroup;
            navigator.InfoGroupEvent += navigator_InfoGroupEvent;
            navigator.GetObjectsInfoEvent += navigator_GetObjectsInfoEvent;
            navigator.SelectObjectEvent += navigator_SelectObjectEvent;
            navigator.DelObjectEvent += navigator_DelObjectEvent;
            navigator.ShowObjectEvent += navigator_ShowObjectEvent;
            navigator.HideObjectEvent += navigator_HideObjectEvent;
            navigator.SelectCondEvent += Navigator_SelectCondEvent;
            navigator.SelectTaskEvent += navigator_SelectTaskEvent;
            navigator.SelectGeoEvent += navigator_SelectGeoEvent;
            navigator.SelectMeshEvent += navigator_SelectMeshEvent;
            navigator.SelectResultsEvent += navigator_SelectResultsEvent;
            navigator.SelectCompEvent += Navigator_SelectCompEvent;
            navigator.SelectCompsEvent += Navigator_SelectCompsEvent;
            navigator.SelectGeneralInfoEvent += navigator_SelectGeneralInfoEvent;
            navigator.SelectTimeEvent += navigator_SelectTimeEvent;
            navigator.SelectResultEvent += navigator_SelectResultEvent;
            navigator.GetResultInfoEvent += navigator_GetResultInfoEvent;
            navigator.DelCondEvent += navigator_DelCondEvent;
            navigator.ControlCollapseEvent += navigator_ControlCollapseEvent;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(tableLayoutPanel1, "tableLayoutPanel1");
            tableLayoutPanel1.Controls.Add(checkPlayerControl, 0, 1);
            tableLayoutPanel1.Controls.Add(propertiesPanel, 0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // checkPlayerControl
            // 
            resources.ApplyResources(checkPlayerControl, "checkPlayerControl");
            checkPlayerControl.BorderStyle = BorderStyle.FixedSingle;
            checkPlayerControl.Cancelation = false;
            checkPlayerControl.CheckState = Player.CheckState.start;
            checkPlayerControl.CurrentValue = 50;
            checkPlayerControl.Name = "checkPlayerControl";
            checkPlayerControl.ShowTextValue = true;
            checkPlayerControl.SliderBarInnerColor = System.Drawing.Color.Silver;
            checkPlayerControl.SliderBarOuterColor = System.Drawing.Color.Silver;
            checkPlayerControl.SliderElapsedInnerColor = System.Drawing.Color.Silver;
            checkPlayerControl.SliderElapsedOuterColor = System.Drawing.Color.Silver;
            checkPlayerControl.SpeedValue = 500;
            checkPlayerControl.StartValue = 0;
            checkPlayerControl.StopValue = 100;
            checkPlayerControl.TextValueColor = System.Drawing.Color.Black;
            checkPlayerControl.CheckingEvent += CheckPlayerControl_CheckingEvent;
            checkPlayerControl.StopCheckingEvent += CheckPlayerControl_StopCheckingEvent;
            checkPlayerControl.StartCheckingEvent += CheckPlayerControl_StartCheckingEvent;
            // 
            // propertiesPanel
            // 
            resources.ApplyResources(propertiesPanel, "propertiesPanel");
            propertiesPanel.BackColor = System.Drawing.SystemColors.ControlLight;
            propertiesPanel.BorderStyle = BorderStyle.FixedSingle;
            propertiesPanel.DownColor = System.Drawing.Color.Gainsboro;
            propertiesPanel.HeaderColor = System.Drawing.Color.Black;
            propertiesPanel.HeaderName = "Properties";
            propertiesPanel.IsPinndable = false;
            propertiesPanel.Name = "propertiesPanel";
            propertiesPanel.UpColor = System.Drawing.Color.Gainsboro;
            propertiesPanel.PropertyUpdateEvent += PropertiesPanel_OnPropertyUpdate;
            // 
            // btnTabНастройки
            // 
            resources.ApplyResources(btnTabНастройки, "btnTabНастройки");
            btnTabНастройки.Name = "btnTabНастройки";
            btnTabНастройки.Tag = "True";
            btnTabНастройки.UseVisualStyleBackColor = true;
            // 
            // btnTabГант
            // 
            resources.ApplyResources(btnTabГант, "btnTabГант");
            btnTabГант.Name = "btnTabГант";
            btnTabГант.Tag = "True";
            btnTabГант.UseVisualStyleBackColor = true;
            // 
            // btnTabФункции
            // 
            resources.ApplyResources(btnTabФункции, "btnTabФункции");
            btnTabФункции.Name = "btnTabФункции";
            btnTabФункции.Tag = "True";
            btnTabФункции.UseVisualStyleBackColor = true;
            // 
            // btnTabМатериалы
            // 
            resources.ApplyResources(btnTabМатериалы, "btnTabМатериалы");
            btnTabМатериалы.Name = "btnTabМатериалы";
            btnTabМатериалы.Tag = "True";
            btnTabМатериалы.UseVisualStyleBackColor = true;
            // 
            // btnTabНавигатор
            // 
            resources.ApplyResources(btnTabНавигатор, "btnTabНавигатор");
            btnTabНавигатор.Name = "btnTabНавигатор";
            btnTabНавигатор.Tag = "True";
            btnTabНавигатор.UseVisualStyleBackColor = true;
            // 
            // contextMenu
            // 
            contextMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            contextMenu.Items.AddRange(new ToolStripItem[] { создатьГруппуItem, скрытьВыбранноеItem, показатьСкрытыеItem, menuItem_InfoSelectedObjects, menuItem_SetRotPoint, показатьСопряженныеItem, menuItem_DeleteSelectedObjects });
            contextMenu.Name = "sceneContextMenu";
            resources.ApplyResources(contextMenu, "contextMenu");
            // 
            // создатьГруппуItem
            // 
            resources.ApplyResources(создатьГруппуItem, "создатьГруппуItem");
            создатьГруппуItem.Name = "создатьГруппуItem";
            создатьГруппуItem.Click += создатьГруппуItem_Click;
            // 
            // скрытьВыбранноеItem
            // 
            скрытьВыбранноеItem.Image = Properties.Resources.Скрыть_выбранное;
            resources.ApplyResources(скрытьВыбранноеItem, "скрытьВыбранноеItem");
            скрытьВыбранноеItem.Name = "скрытьВыбранноеItem";
            скрытьВыбранноеItem.Click += скрытьВыбранноеItem_Click;
            // 
            // показатьСкрытыеItem
            // 
            показатьСкрытыеItem.Image = Properties.Resources.Показать_все_скрытые;
            resources.ApplyResources(показатьСкрытыеItem, "показатьСкрытыеItem");
            показатьСкрытыеItem.Name = "показатьСкрытыеItem";
            показатьСкрытыеItem.Click += показатьСкрытыеItem_Click;
            // 
            // menuItem_InfoSelectedObjects
            // 
            menuItem_InfoSelectedObjects.Image = Properties.Resources.Инфо_со_сцены;
            resources.ApplyResources(menuItem_InfoSelectedObjects, "menuItem_InfoSelectedObjects");
            menuItem_InfoSelectedObjects.Name = "menuItem_InfoSelectedObjects";
            menuItem_InfoSelectedObjects.Click += menuItem_InfoSelectedObjects_Click;
            // 
            // menuItem_SetRotPoint
            // 
            menuItem_SetRotPoint.Image = Properties.Resources.Задать_точку_вращения;
            resources.ApplyResources(menuItem_SetRotPoint, "menuItem_SetRotPoint");
            menuItem_SetRotPoint.Name = "menuItem_SetRotPoint";
            menuItem_SetRotPoint.Click += menuItem_SetRotPoint_Click;
            // 
            // показатьСопряженныеItem
            // 
            показатьСопряженныеItem.Image = Properties.Resources.Показать_сопряженные;
            resources.ApplyResources(показатьСопряженныеItem, "показатьСопряженныеItem");
            показатьСопряженныеItem.Name = "показатьСопряженныеItem";
            показатьСопряженныеItem.Click += показатьСопряженныеItem_Click;
            // 
            // menuItem_DeleteSelectedObjects
            // 
            resources.ApplyResources(menuItem_DeleteSelectedObjects, "menuItem_DeleteSelectedObjects");
            menuItem_DeleteSelectedObjects.Name = "menuItem_DeleteSelectedObjects";
            menuItem_DeleteSelectedObjects.Click += menuItem_DeleteSelectedObjects_Click;
            // 
            // BaseForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Dpi;
            Controls.Add(toolStripContainer);
            IsMdiContainer = true;
            KeyPreview = true;
            MainMenuStrip = menuStrip;
            Name = "BaseForm";
            WindowState = FormWindowState.Maximized;
            FormClosing += OnClosingForm;
            FormClosed += BaseForm_FormClosed;
            Load += BaseForm_Load;
            KeyDown += BaseForm_KeyDown;
            toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
            toolStripContainer.BottomToolStripPanel.PerformLayout();
            toolStripContainer.ContentPanel.ResumeLayout(false);
            toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            toolStripContainer.TopToolStripPanel.PerformLayout();
            toolStripContainer.ResumeLayout(false);
            toolStripContainer.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer3).EndInit();
            splitContainer3.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            cntrНавигатор.Panel1.ResumeLayout(false);
            cntrНавигатор.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)cntrНавигатор).EndInit();
            cntrНавигатор.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            contextMenu.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem содержаниеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem опрограммеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem лицензияToolStripMenuItem;
        

        private System.Windows.Forms.ToolStripMenuItem сведенияMenuItem;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem создатьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem открытьToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem сохранитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитькакToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblVersion;
        private System.Windows.Forms.ToolStripStatusLabel webPageLabel;
        private System.Windows.Forms.ToolStripMenuItem viewMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem tasksMenuItem;
        private System.Windows.Forms.ToolStripMenuItem результатыMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataBasesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem материалыMenuItem;
        private System.Windows.Forms.ToolStripMenuItem функцииMenuItem;
        private UserControlsEx.SplitContainerEx splitContainer3;
        private UserControlsEx.SplitContainerEx cntrНавигатор;
        private NavigatorControl navigator;
        private WinGUI.PropertiesPanel.PropertiesPanelControl propertiesPanel;
        private UserControlsEx.SplitContainerEx splitContainer2;
        private WinGUI.Console.ConsoleControl console;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem создатьГруппуItem;
        private System.Windows.Forms.ToolStripMenuItem скрытьВыбранноеItem;
        private System.Windows.Forms.ToolStripMenuItem показатьСкрытыеItem;
        private System.Windows.Forms.ToolStripMenuItem menuItem_InfoSelectedObjects;
        private System.Windows.Forms.ToolStripMenuItem menuItem_SetRotPoint;
        private System.Windows.Forms.ToolStripMenuItem menuItem_DeleteSelectedObjects;
        private WinGUI.Player.PlayerControl checkPlayerControl;
        private TableLayoutPanel tableLayoutPanel1;
        private ToolStripMenuItem открытьToolStripMenuItem1;
        private ToolStripMenuItem мастерToolStripMenuItem;
        private ToolStripMenuItem расчетыToolStripMenuItem;
        private ToolStripMenuItem сформироватьИнструкцииToolStripMenuItem;
        private ToolStripMenuItem сеткаToolStripMenuItem;
        private ToolStripMenuItem dToolStripMenuItem;
        private ToolStripMenuItem наToolStripMenuItem;
        private ToolStripMenuItem dToolStripMenuItem1;
        private ToolStripMenuItem dToolStripMenuItem2;
        private ToolStripMenuItem уплотнитьToolStripMenuItem;
        private ToolStripMenuItem наПоверхности3DToolStripMenuItem;
        private ToolStripMenuItem добавитьToolStripMenuItem;
        private ToolStripMenuItem открытьИнструкцииToolStripMenuItem;
        private ToolStripMenuItem объединитьToolStripMenuItem;
        private ToolStripMenuItem создатьToolStripMenuItem1;
        private ToolStripMenuItem построитьГрафикToolStripMenuItem;
        private ToolStripMenuItem построитьДиаграммуToolStripMenuItem;
        private ToolStripMenuItem загрузитьgeoToolStripMenuItem;
        private ToolStripMenuItem сформироватьgeoToolStripMenuItem;
        private ToolStripMenuItem наПоверхностиГеометрииToolStripMenuItem;
        private ToolStripMenuItem показатьНаДиаграммеToolStripMenuItem;
        private ToolStripMenuItem запуститьToolStripMenuItem;
        private ToolStripMenuItem остановитьToolStripMenuItem;
        private ToolStripMenuItem создатьАнимациюToolStripMenuItem;
        private ToolStripMenuItem экспортироватьРезультатыToolStripMenuItem;
        private ToolStripMenuItem инструментыToolStripMenuItem;
        private ToolStripMenuItem квадратизацияСуществующейToolStripMenuItem;
        private ToolStripMenuItem измеритьToolStripMenuItem;
        private ToolStripMenuItem скрытьПлоскостьюToolStripMenuItem;
        private Button btnAdvSelection;
        private ImageList sceneImageList;
        private Button btnDisplayStates;
        private Button btnRotVert90;
        private Button btnRotHor90;
        private Button btnRotZ;
        private Button btnRotY;
        private Button btnRotX;
        private Button btnZY;
        private Button btnZX;
        private Button btnXY;
        private Button btnDisplayViews;
        private Button btnShowInsideObjects;
        private Button btnFitToScreen;
        private Button btnShowSidesRibs;
        private Button btnShowRibs;
        private Button btnShowSides;
        private Button btnBazis;
        private Button btnBorder;
        private Button btnMakeScreenShot;
        private Button btnSelect;
        private Button btnTabНавигатор;
        private Button btnTabГант;
        private Button btnTabФункции;
        private Button btnTabМатериалы;
        private OpenTK.GLControl.GLControl scene;
        private Button btnTabНастройки;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripMenuItem загрузитьМастерToolStripMenuItem;
        private ToolStripMenuItem рассечьПлоскостьюToolStripMenuItem;
        private ToolStripMenuItem показатьСопряженныеItem;
        private ToolStripMenuItem геометрияToolStripMenuItem;
        private ToolStripMenuItem создатьТочкуToolStripMenuItem;
        private ToolStripMenuItem создатьЛиниюToolStripMenuItem;
        private ToolStripMenuItem создатьПлоскостьToolStripMenuItem;
        private ToolStripMenuItem создатьОбъемToolStripMenuItem;
        private ToolStripMenuItem addChamferToolStripMenuItem;
    }
}

