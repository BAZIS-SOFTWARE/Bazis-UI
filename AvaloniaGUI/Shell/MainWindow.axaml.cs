using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AvaloniaGUI.Console;
using AvaloniaGUI.Navigator;
using AvaloniaGUI.Properties;
using AvaloniaGUI.Scene;
using System;
using System.Resources;

namespace AvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private static readonly ResourceManager resources = new("AvaloniaGUI.Shell.MainWindow", typeof(MainWindow).Assembly);

        private NavigatorControl navigator;
        private PropertiesPanelControl propertiesPanel;
        private AvaloniaGUI.Player.PlayerControl checkPlayerControl;
        private Grid cntrНавигатор;
        private GridLength panel1Width;
        private GridLength panel2Height;

        // GridSplitter не сдвигается дальше MinWidth/MinHeight колонки или строки.
        private const double NavigatorPanelMinWidth = 300;
        private const double ConsolePanelMinHeight = 150;

        /// <summary>Аналог splitContainer3.Panel1Collapsed (навигатор и свойства).</summary>
        private bool Panel1Collapsed
        {
            get => !splitContainer3Panel1.IsVisible;
            set
            {
                if (value == Panel1Collapsed) return;
                if (value) panel1Width = splitContainer3.ColumnDefinitions[0].Width;
                splitContainer3Panel1.IsVisible = splitContainer3Splitter.IsVisible = !value;
                splitContainer3.ColumnDefinitions[0].MinWidth = value ? 0 : NavigatorPanelMinWidth;
                splitContainer3.ColumnDefinitions[0].Width = value ? new GridLength(0) : panel1Width;
                splitContainer3.ColumnDefinitions[1].Width = new GridLength(value ? 0 : 8);
                toolStripMenuItem2.IsChecked = !value;
            }
        }

        /// <summary>Аналог splitContainer2.Panel2Collapsed (консоль).</summary>
        private bool Panel2Collapsed
        {
            get => !console.IsVisible;
            set
            {
                if (value == Panel2Collapsed) return;
                if (value) panel2Height = splitContainer2.RowDefinitions[2].Height;
                console.IsVisible = splitContainer2Splitter.IsVisible = !value;
                splitContainer2.RowDefinitions[1].Height = new GridLength(value ? 0 : 8);
                splitContainer2.RowDefinitions[2].MinHeight = value ? 0 : ConsolePanelMinHeight;
                splitContainer2.RowDefinitions[2].Height = value ? new GridLength(0) : panel2Height;
                toolStripMenuItem3.IsChecked = !value;
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            menuStrip = this.FindControl<Menu>("menuStrip");
            файлToolStripMenuItem = this.FindControl<MenuItem>("файлToolStripMenuItem");
            создатьToolStripMenuItem = this.FindControl<MenuItem>("создатьToolStripMenuItem");
            открытьToolStripMenuItem = this.FindControl<MenuItem>("открытьToolStripMenuItem");
            добавитьToolStripMenuItem = this.FindControl<MenuItem>("добавитьToolStripMenuItem");
            сохранитьToolStripMenuItem = this.FindControl<MenuItem>("сохранитьToolStripMenuItem");
            сохранитькакToolStripMenuItem = this.FindControl<MenuItem>("сохранитькакToolStripMenuItem");
            выходToolStripMenuItem = this.FindControl<MenuItem>("выходToolStripMenuItem");
            viewMenuItem = this.FindControl<MenuItem>("viewMenuItem");
            toolStripMenuItem2 = this.FindControl<MenuItem>("toolStripMenuItem2");
            toolStripMenuItem3 = this.FindControl<MenuItem>("toolStripMenuItem3");
            геометрияToolStripMenuItem = this.FindControl<MenuItem>("геометрияToolStripMenuItem");
            создатьТочкуToolStripMenuItem = this.FindControl<MenuItem>("создатьТочкуToolStripMenuItem");
            создатьЛиниюToolStripMenuItem = this.FindControl<MenuItem>("создатьЛиниюToolStripMenuItem");
            создатьПлоскостьToolStripMenuItem = this.FindControl<MenuItem>("создатьПлоскостьToolStripMenuItem");
            создатьОбъемToolStripMenuItem = this.FindControl<MenuItem>("создатьОбъемToolStripMenuItem");
            addChamferToolStripMenuItem = this.FindControl<MenuItem>("addChamferToolStripMenuItem");
            сеткаToolStripMenuItem = this.FindControl<MenuItem>("сеткаToolStripMenuItem");
            загрузитьgeoToolStripMenuItem = this.FindControl<MenuItem>("загрузитьgeoToolStripMenuItem");
            сформироватьgeoToolStripMenuItem = this.FindControl<MenuItem>("сформироватьgeoToolStripMenuItem");
            dToolStripMenuItem = this.FindControl<MenuItem>("dToolStripMenuItem");
            наToolStripMenuItem = this.FindControl<MenuItem>("наToolStripMenuItem");
            dToolStripMenuItem1 = this.FindControl<MenuItem>("dToolStripMenuItem1");
            уплотнитьToolStripMenuItem = this.FindControl<MenuItem>("уплотнитьToolStripMenuItem");
            наПоверхности3DToolStripMenuItem = this.FindControl<MenuItem>("наПоверхности3DToolStripMenuItem");
            наПоверхностиГеометрииToolStripMenuItem = this.FindControl<MenuItem>("наПоверхностиГеометрииToolStripMenuItem");
            квадратизацияСуществующейToolStripMenuItem = this.FindControl<MenuItem>("квадратизацияСуществующейToolStripMenuItem");
            dToolStripMenuItem2 = this.FindControl<MenuItem>("dToolStripMenuItem2");
            dataBasesMenuItem = this.FindControl<MenuItem>("dataBasesMenuItem");
            материалыMenuItem = this.FindControl<MenuItem>("материалыMenuItem");
            функцииMenuItem = this.FindControl<MenuItem>("функцииMenuItem");
            tasksMenuItem = this.FindControl<MenuItem>("tasksMenuItem");
            создатьToolStripMenuItem1 = this.FindControl<MenuItem>("создатьToolStripMenuItem1");
            мастерToolStripMenuItem = this.FindControl<MenuItem>("мастерToolStripMenuItem");
            загрузитьМастерToolStripMenuItem = this.FindControl<MenuItem>("загрузитьМастерToolStripMenuItem");
            показатьНаДиаграммеToolStripMenuItem = this.FindControl<MenuItem>("показатьНаДиаграммеToolStripMenuItem");
            расчетыToolStripMenuItem = this.FindControl<MenuItem>("расчетыToolStripMenuItem");
            открытьИнструкцииToolStripMenuItem = this.FindControl<MenuItem>("открытьИнструкцииToolStripMenuItem");
            сформироватьИнструкцииToolStripMenuItem = this.FindControl<MenuItem>("сформироватьИнструкцииToolStripMenuItem");
            запуститьToolStripMenuItem = this.FindControl<MenuItem>("запуститьToolStripMenuItem");
            остановитьToolStripMenuItem = this.FindControl<MenuItem>("остановитьToolStripMenuItem");
            результатыMenuItem = this.FindControl<MenuItem>("результатыMenuItem");
            открытьToolStripMenuItem1 = this.FindControl<MenuItem>("открытьToolStripMenuItem1");
            объединитьToolStripMenuItem = this.FindControl<MenuItem>("объединитьToolStripMenuItem");
            построитьГрафикToolStripMenuItem = this.FindControl<MenuItem>("построитьГрафикToolStripMenuItem");
            построитьДиаграммуToolStripMenuItem = this.FindControl<MenuItem>("построитьДиаграммуToolStripMenuItem");
            создатьАнимациюToolStripMenuItem = this.FindControl<MenuItem>("создатьАнимациюToolStripMenuItem");
            экспортироватьРезультатыToolStripMenuItem = this.FindControl<MenuItem>("экспортироватьРезультатыToolStripMenuItem");
            toolStripMenuItem4 = this.FindControl<MenuItem>("toolStripMenuItem4");
            инструментыToolStripMenuItem = this.FindControl<MenuItem>("инструментыToolStripMenuItem");
            измеритьToolStripMenuItem = this.FindControl<MenuItem>("измеритьToolStripMenuItem");
            скрытьПлоскостьюToolStripMenuItem = this.FindControl<MenuItem>("скрытьПлоскостьюToolStripMenuItem");
            рассечьПлоскостьюToolStripMenuItem = this.FindControl<MenuItem>("рассечьПлоскостьюToolStripMenuItem");
            настройкиToolStripMenuItem = this.FindControl<MenuItem>("настройкиToolStripMenuItem");
            справкаToolStripMenuItem = this.FindControl<MenuItem>("справкаToolStripMenuItem");
            содержаниеToolStripMenuItem = this.FindControl<MenuItem>("содержаниеToolStripMenuItem");
            опрограммеToolStripMenuItem = this.FindControl<MenuItem>("опрограммеToolStripMenuItem");
            лицензияToolStripMenuItem = this.FindControl<MenuItem>("лицензияToolStripMenuItem");
            сведенияMenuItem = this.FindControl<MenuItem>("сведенияMenuItem");
            scene = this.FindControl<SceneView>("scene");
            console = this.FindControl<ConsoleControl>("console");
            lblStatus = this.FindControl<TextBlock>("lblStatus");
            lblVersion = this.FindControl<TextBlock>("lblVersion");
            splitContainer3 = this.FindControl<Grid>("splitContainer3");
            splitContainer3Panel1 = this.FindControl<Grid>("splitContainer3Panel1");
            splitContainer3Splitter = this.FindControl<GridSplitter>("splitContainer3Splitter");
            splitContainer2 = this.FindControl<Grid>("splitContainer2");
            splitContainer2Splitter = this.FindControl<GridSplitter>("splitContainer2Splitter");
            cntrНавигатор = new NavigatorPage();
            navigator = cntrНавигатор.FindControl<NavigatorControl>("navigator");
            propertiesPanel = cntrНавигатор.FindControl<PropertiesPanelControl>("propertiesPanel");
            checkPlayerControl = cntrНавигатор.FindControl<AvaloniaGUI.Player.PlayerControl>("checkPlayerControl");
            Title = resources.GetString("$this.Text");
            lblStatus.Text = resources.GetString("lblStatus.Text");
            foreach (var item in menuStrip.Items)
                if (item is MenuItem menuItem)
                    LocalizeMenu(menuItem);

            создатьToolStripMenuItem.Click += создатьToolStripMenuItem_Click;
            открытьToolStripMenuItem.Click += открытьToolStripMenuItem_Click;
            добавитьToolStripMenuItem.Click += добавитьСеткуToolStripMenuItem_Click;
            сохранитьToolStripMenuItem.Click += сохранитьToolStripMenuItem_Click;
            сохранитькакToolStripMenuItem.Click += сохранитькакToolStripMenuItem_Click;
            выходToolStripMenuItem.Click += выходToolStripMenuItem_Click;
            toolStripMenuItem2.Click += toolStripMenuItem2_Click;
            toolStripMenuItem3.Click += toolStripMenuItem3_Click;
            addChamferToolStripMenuItem.Click += addChamferToolStripMenuItem_Click;
            загрузитьgeoToolStripMenuItem.Click += загрузитьgeoToolStripMenuItem_Click;
            сформироватьgeoToolStripMenuItem.Click += сформироватьgeoToolStripMenuItem_Click;
            наToolStripMenuItem.Click += наПоверхности2DToolStripMenuItem_Click;
            уплотнитьToolStripMenuItem.Click += уплотнитьToolStripMenuItem_Click;
            наПоверхности3DToolStripMenuItem.Click += наПоверхности3DToolStripMenuItem_Click;
            наПоверхностиГеометрииToolStripMenuItem.Click += наПоверхностиГеометрииToolStripMenuItem_Click;
            квадратизацияСуществующейToolStripMenuItem.Click += квадратизацияСуществующейToolStripMenuItem_Click;
            dToolStripMenuItem2.Click += создать3DСеткуToolStripMenuItem_Click;
            материалыMenuItem.Click += материалыMenuItem_Click;
            функцииMenuItem.Click += функцииMenuItem_Click;
            создатьToolStripMenuItem1.Click += создатьЗадачуToolStripMenuItem_Click;
            показатьНаДиаграммеToolStripMenuItem.Click += показатьНаДиаграммеToolStripMenuItem_Click;
            открытьИнструкцииToolStripMenuItem.Click += открытьИнструкцииToolStripMenuItem_Click;
            сформироватьИнструкцииToolStripMenuItem.Click += сформироватьИнструкцииToolStripMenuItem_Click;
            запуститьToolStripMenuItem.Click += запуститьToolStripMenuItem_Click;
            остановитьToolStripMenuItem.Click += остановитьToolStripMenuItem_Click;
            открытьToolStripMenuItem1.Click += открытьToolStripMenuItem1_Click;
            объединитьToolStripMenuItem.Click += MergeDataBase_Click;
            построитьГрафикToolStripMenuItem.Click += построитьГрафикToolStripMenuItem_Click;
            построитьДиаграммуToolStripMenuItem.Click += построитьДиаграммуToolStripMenuItem_Click;
            создатьАнимациюToolStripMenuItem.Click += создатьАнимациюToolStripMenuItem_Click;
            toolStripMenuItem4.Click += отзеркаливаниеToolStripMenuItem_Click;
            измеритьToolStripMenuItem.Click += измеритьToolStripMenuItem_Click;
            скрытьПлоскостьюToolStripMenuItem.Click += скрытьПлоскостьюToolStripMenuItem_Click;
            рассечьПлоскостьюToolStripMenuItem.Click += рассечьПлоскостьюToolStripMenuItem_Click;
            настройкиToolStripMenuItem.Click += настройкиToolStripMenuItem_Click;
            содержаниеToolStripMenuItem.Click += содержаниеToolStripMenuItem_Click;
            опрограммеToolStripMenuItem.Click += опрограммеToolStripMenuItem_Click;
            сведенияMenuItem.Click += сведенияMenuItem_Click;

            checkPlayerControl.CheckingEvent += CheckPlayerControl_CheckingEvent;
            checkPlayerControl.StopCheckingEvent += CheckPlayerControl_StopCheckingEvent;
            checkPlayerControl.StartCheckingEvent += CheckPlayerControl_StartCheckingEvent;

            console.ConsoleCommandEnteredEvent += ExecuteCommand;
            console.CommandsListRequestedEvent += PrintAllCommands;

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
            navigator.SelectGeneralInfoEvent += navigator_SelectGeneralInfoEvent;
            navigator.DelCondEvent += navigator_DelCondEvent;
            navigator.HideResultsEvent += navigator_HideResultsEvent;
            navigator.RemoveResultsEvent += navigator_RemoveResultsEvent;
            navigator.SelectResultsEvent += navigator_SelectResultsEvent;
            navigator.SelectCompEvent += Navigator_SelectCompEvent;
            navigator.SelectCompsEvent += Navigator_SelectCompsEvent;
            navigator.SelectTimeEvent += navigator_SelectTimeEvent;
            navigator.SelectResultEvent += navigator_SelectResultEvent;
            navigator.GetResultInfoEvent += navigator_GetResultInfoEvent;

            propertiesPanel.PropertyUpdateEvent += PropertiesPanel_OnPropertyUpdate;

            scene.Surface.SelectionApplied += scene_SelectionApplied;
            scene.Surface.ProjectDisplayFailed += (sender, ex) => console.PrintInfo(ex.Message, System.Drawing.Color.Red);
            scene.Surface.MessageReported += (message, color) => console.PrintInfo(message, color);
            scene.Surface.GroupCreated += scene_GroupCreated;
            scene.Surface.ObjectsRemoved += scene_ObjectsRemoved;
            scene.Surface.SelectedObjectTypeChanged += scene_SelectedObjectTypeChanged;
            // Esc на сцене: в BaseForm.GlControl_KeyDown также закрывается окно дополнительного выбора.
            scene.Surface.SelectionReset += CloseAdvancedSelectionForm;
            scene.AdvancedSelectionRequested += scene_AdvancedSelectionRequested;
            scene.ScreenShotRequested += btnMakeScreenShot_Click;

            Opened += BaseForm_Load;
            Closing += OnClosingForm;
            AddHandler(KeyDownEvent, (sender, e) =>
            {
                if (e.Key != Key.Escape)
                    BaseForm_KeyDown(sender, e);
            }, RoutingStrategies.Tunnel);
            AddHandler(KeyDownEvent, (sender, e) =>
            {
                if (e.Key != Key.Escape)
                    return;

                BaseForm_KeyDown(sender, e);
                scene.Surface.ClearSelection();
                e.Handled = true;
            }, RoutingStrategies.Bubble);
        }

        private static void LocalizeMenu(MenuItem item)
        {
            // Тексты меню хранятся в существующих локализованных resx.
            item.Header = (resources.GetString(item.Name + ".Text") ?? item.Name).Replace('&', '_');
            foreach (var child in item.Items)
                if (child is MenuItem menuItem)
                    LocalizeMenu(menuItem);
        }

    }
}



