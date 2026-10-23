using Avalonia;
using Avalonia.Automation;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using AvaloniaGUI.Console;
using AvaloniaGUI.Localization;
using AvaloniaGUI.Navigator;
using AvaloniaGUI.Properties;
using AvaloniaGUI.Scene;
using System;
using System.Resources;

namespace AvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        // Аналог ComponentResourceManager(typeof(BaseForm)): тексты перенесены из BaseForm.resx.
        private static readonly ResourceManager resources = new("BazisAvaloniaGUI.Shell.MainWindow", typeof(MainWindow).Assembly);

        private Menu menuStrip;
        private MenuItem файлToolStripMenuItem;
        private MenuItem создатьToolStripMenuItem;
        private MenuItem открытьToolStripMenuItem;
        private MenuItem добавитьToolStripMenuItem;
        private MenuItem сохранитьToolStripMenuItem;
        private MenuItem сохранитькакToolStripMenuItem;
        private MenuItem выходToolStripMenuItem;
        private MenuItem viewMenuItem;
        private MenuItem toolStripMenuItem2;
        private MenuItem toolStripMenuItem3;
        private MenuItem геометрияToolStripMenuItem;
        private MenuItem создатьТочкуToolStripMenuItem;
        private MenuItem создатьЛиниюToolStripMenuItem;
        private MenuItem создатьПлоскостьToolStripMenuItem;
        private MenuItem создатьОбъемToolStripMenuItem;
        private MenuItem addChamferToolStripMenuItem;
        private MenuItem сеткаToolStripMenuItem;
        private MenuItem загрузитьgeoToolStripMenuItem;
        private MenuItem сформироватьgeoToolStripMenuItem;
        private MenuItem dToolStripMenuItem;
        private MenuItem наToolStripMenuItem;
        private MenuItem dToolStripMenuItem1;
        private MenuItem уплотнитьToolStripMenuItem;
        private MenuItem наПоверхности3DToolStripMenuItem;
        private MenuItem наПоверхностиГеометрииToolStripMenuItem;
        private MenuItem квадратизацияСуществующейToolStripMenuItem;
        private MenuItem dToolStripMenuItem2;
        private MenuItem dataBasesMenuItem;
        private MenuItem материалыMenuItem;
        private MenuItem функцииMenuItem;
        private MenuItem tasksMenuItem;
        private MenuItem создатьToolStripMenuItem1;
        private MenuItem мастерToolStripMenuItem;
        private MenuItem загрузитьМастерToolStripMenuItem;
        private MenuItem показатьНаДиаграммеToolStripMenuItem;
        private MenuItem расчетыToolStripMenuItem;
        private MenuItem открытьИнструкцииToolStripMenuItem;
        private MenuItem сформироватьИнструкцииToolStripMenuItem;
        private MenuItem запуститьToolStripMenuItem;
        private MenuItem остановитьToolStripMenuItem;
        private MenuItem статусToolStripMenuItem;
        private MenuItem результатыMenuItem;
        private MenuItem открытьToolStripMenuItem1;
        private MenuItem объединитьToolStripMenuItem;
        private MenuItem построитьГрафикToolStripMenuItem;
        private MenuItem построитьДиаграммуToolStripMenuItem;
        private MenuItem создатьАнимациюToolStripMenuItem;
        private MenuItem экспортироватьРезультатыToolStripMenuItem;
        private MenuItem toolStripMenuItem4;
        private MenuItem инструментыToolStripMenuItem;
        private MenuItem измеритьToolStripMenuItem;
        private MenuItem скрытьПлоскостьюToolStripMenuItem;
        private MenuItem рассечьПлоскостьюToolStripMenuItem;
        private MenuItem настройкиToolStripMenuItem;
        private MenuItem справкаToolStripMenuItem;
        private MenuItem содержаниеToolStripMenuItem;
        private MenuItem опрограммеToolStripMenuItem;
        private MenuItem лицензияToolStripMenuItem;
        private MenuItem сведенияMenuItem;

        private SceneView scene;
        private NavigatorControl navigator;
        private PropertiesPanelControl propertiesPanel;
        private AvaloniaGUI.Player.PlayerControl checkPlayerControl;
        private ConsoleControl console;
        private TextBlock lblStatus;
        private TextBlock lblVersion;

        private Grid splitContainer3;
        private Grid splitContainer3Panel1;
        private Grid cntrНавигатор;
        private GridSplitter splitContainer3Splitter;
        private Grid splitContainer2;
        private GridSplitter splitContainer2Splitter;
        private GridLength panel1Width;
        private GridLength panel2Height;

        // Минимальные размеры панелей (аналог SplitContainer.Panel1MinSize и Panel2MinSize).
        // GridSplitter не сдвигается дальше MinWidth/MinHeight колонки или строки.
        // В BaseForm они не заданы (WinForms по умолчанию 25 px); боковая панель и консоль ограничены по требованию.
        private const double PanelMinSize = 25;
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
            scene = new SceneView();
            navigator = new NavigatorControl();
            propertiesPanel = new PropertiesPanelControl();
            checkPlayerControl = new AvaloniaGUI.Player.PlayerControl { SpeedValue = 500, StartValue = 0, StopValue = 100, CurrentValue = 50 };
            checkPlayerControl.CheckingEvent += CheckPlayerControl_CheckingEvent;
            checkPlayerControl.StopCheckingEvent += CheckPlayerControl_StopCheckingEvent;
            checkPlayerControl.StartCheckingEvent += CheckPlayerControl_StartCheckingEvent;
            console = new ConsoleControl();

            Width = 942;
            Height = 625;
            // BaseForm: 415 px; увеличено, чтобы поместились боковая панель (NavigatorPanelMinWidth), разделитель,
            // минимальная сцена и поля splitContainer3.
            MinWidth = Math.Max(415, NavigatorPanelMinWidth + 8 + PanelMinSize + 10);
            MinHeight = 320;
            WindowState = WindowState.Maximized;
            Background = Brush.Parse("#F0F0F0");
            RequestedThemeVariant = ThemeVariant.Light;
            FontFamily = new FontFamily("Microsoft Sans Serif");
            FontSize = 11; // BaseForm: 8.25 pt at 96 DPI
            Foreground = Brushes.Black;
            Title = resources.GetString("$this.Text");

            // menuStrip
            создатьToolStripMenuItem = Item("создатьToolStripMenuItem", создатьToolStripMenuItem_Click);
            открытьToolStripMenuItem = Item("открытьToolStripMenuItem", открытьToolStripMenuItem_Click);
            добавитьToolStripMenuItem = Item("добавитьToolStripMenuItem", добавитьСеткуToolStripMenuItem_Click);
            сохранитьToolStripMenuItem = Item("сохранитьToolStripMenuItem", сохранитьToolStripMenuItem_Click);
            сохранитькакToolStripMenuItem = Item("сохранитькакToolStripMenuItem", сохранитькакToolStripMenuItem_Click);
            выходToolStripMenuItem = Item("выходToolStripMenuItem", выходToolStripMenuItem_Click);
            создатьToolStripMenuItem.InputGesture = new KeyGesture(Key.N, KeyModifiers.Control);
            открытьToolStripMenuItem.InputGesture = new KeyGesture(Key.O, KeyModifiers.Control);
            сохранитьToolStripMenuItem.InputGesture = new KeyGesture(Key.S, KeyModifiers.Control);
            файлToolStripMenuItem = Item("файлToolStripMenuItem", null, создатьToolStripMenuItem, открытьToolStripMenuItem,
                добавитьToolStripMenuItem, new Separator(), сохранитьToolStripMenuItem, сохранитькакToolStripMenuItem,
                new Separator(), new Separator(), выходToolStripMenuItem);

            toolStripMenuItem2 = Item("toolStripMenuItem2", toolStripMenuItem2_Click);
            toolStripMenuItem3 = Item("toolStripMenuItem3", toolStripMenuItem3_Click);
            // В BaseForm у пунктов нет флажка; добавлен, чтобы показывать видимость панелей, как у пунктов баз данных.
            toolStripMenuItem2.ToggleType = toolStripMenuItem3.ToggleType = MenuItemToggleType.CheckBox;
            toolStripMenuItem2.IsChecked = toolStripMenuItem3.IsChecked = true;
            viewMenuItem = Item("viewMenuItem", null, toolStripMenuItem2, toolStripMenuItem3);

            // В WinForms у пунктов создания геометрии обработчики не назначены.
            создатьТочкуToolStripMenuItem = Item("создатьТочкуToolStripMenuItem", null);
            создатьЛиниюToolStripMenuItem = Item("создатьЛиниюToolStripMenuItem", null);
            создатьПлоскостьToolStripMenuItem = Item("создатьПлоскостьToolStripMenuItem", null);
            создатьОбъемToolStripMenuItem = Item("создатьОбъемToolStripMenuItem", null);
            // Обработчиков нет и в WinForms — пункты скрыты, пока создание геометрии не реализовано.
            создатьТочкуToolStripMenuItem.IsVisible = false;
            создатьЛиниюToolStripMenuItem.IsVisible = false;
            создатьПлоскостьToolStripMenuItem.IsVisible = false;
            создатьОбъемToolStripMenuItem.IsVisible = false;
            addChamferToolStripMenuItem = Item("addChamferToolStripMenuItem", addChamferToolStripMenuItem_Click);
            addChamferToolStripMenuItem.ToggleType = MenuItemToggleType.CheckBox;
            геометрияToolStripMenuItem = Item("геометрияToolStripMenuItem", null, создатьТочкуToolStripMenuItem,
                создатьЛиниюToolStripMenuItem, создатьПлоскостьToolStripMenuItem, создатьОбъемToolStripMenuItem,
                addChamferToolStripMenuItem);
            геометрияToolStripMenuItem.IsEnabled = false;

            загрузитьgeoToolStripMenuItem = Item("загрузитьgeoToolStripMenuItem", загрузитьgeoToolStripMenuItem_Click);
            сформироватьgeoToolStripMenuItem = Item("сформироватьgeoToolStripMenuItem", сформироватьgeoToolStripMenuItem_Click);
            наToolStripMenuItem = Item("наToolStripMenuItem", наПоверхности2DToolStripMenuItem_Click);
            dToolStripMenuItem = Item("dToolStripMenuItem", null, наToolStripMenuItem);
            уплотнитьToolStripMenuItem = Item("уплотнитьToolStripMenuItem", уплотнитьToolStripMenuItem_Click);
            уплотнитьToolStripMenuItem.IsEnabled = false;
            наПоверхности3DToolStripMenuItem = Item("наПоверхности3DToolStripMenuItem", наПоверхности3DToolStripMenuItem_Click);
            наПоверхностиГеометрииToolStripMenuItem = Item("наПоверхностиГеометрииToolStripMenuItem", наПоверхностиГеометрииToolStripMenuItem_Click);
            квадратизацияСуществующейToolStripMenuItem = Item("квадратизацияСуществующейToolStripMenuItem", квадратизацияСуществующейToolStripMenuItem_Click);
            квадратизацияСуществующейToolStripMenuItem.IsEnabled = false;
            dToolStripMenuItem1 = Item("dToolStripMenuItem1", null, уплотнитьToolStripMenuItem, наПоверхности3DToolStripMenuItem,
                наПоверхностиГеометрииToolStripMenuItem, квадратизацияСуществующейToolStripMenuItem);
            dToolStripMenuItem2 = Item("dToolStripMenuItem2", создать3DСеткуToolStripMenuItem_Click);
            сеткаToolStripMenuItem = Item("сеткаToolStripMenuItem", null, загрузитьgeoToolStripMenuItem, сформироватьgeoToolStripMenuItem,
                dToolStripMenuItem, dToolStripMenuItem1, dToolStripMenuItem2);
            сеткаToolStripMenuItem.IsEnabled = false;

            материалыMenuItem = Item("материалыMenuItem", материалыMenuItem_Click);
            материалыMenuItem.ToggleType = MenuItemToggleType.CheckBox;
            функцииMenuItem = Item("функцииMenuItem", функцииMenuItem_Click);
            функцииMenuItem.ToggleType = MenuItemToggleType.CheckBox;
            dataBasesMenuItem = Item("dataBasesMenuItem", null, материалыMenuItem, функцииMenuItem);
            dataBasesMenuItem.IsEnabled = false;

            создатьToolStripMenuItem1 = Item("создатьToolStripMenuItem1", создатьЗадачуToolStripMenuItem_Click);
            загрузитьМастерToolStripMenuItem = Item("загрузитьМастерToolStripMenuItem", null);
            загрузитьМастерToolStripMenuItem.IsEnabled = false; // мастера в Avalonia-оболочке ещё не перенесены
            мастерToolStripMenuItem = Item("мастерToolStripMenuItem", null, new Separator(), загрузитьМастерToolStripMenuItem, new Separator());
            показатьНаДиаграммеToolStripMenuItem = Item("показатьНаДиаграммеToolStripMenuItem", показатьНаДиаграммеToolStripMenuItem_Click);
            показатьНаДиаграммеToolStripMenuItem.ToggleType = MenuItemToggleType.CheckBox;
            tasksMenuItem = Item("tasksMenuItem", null, создатьToolStripMenuItem1, мастерToolStripMenuItem, показатьНаДиаграммеToolStripMenuItem);
            tasksMenuItem.IsEnabled = false;

            открытьИнструкцииToolStripMenuItem = Item("открытьИнструкцииToolStripMenuItem", открытьИнструкцииToolStripMenuItem_Click);
            сформироватьИнструкцииToolStripMenuItem = Item("сформироватьИнструкцииToolStripMenuItem", сформироватьИнструкцииToolStripMenuItem_Click);
            запуститьToolStripMenuItem = Item("запуститьToolStripMenuItem", запуститьToolStripMenuItem_Click);
            остановитьToolStripMenuItem = Item("остановитьToolStripMenuItem", остановитьToolStripMenuItem_Click);
            статусToolStripMenuItem = Item("статусToolStripMenuItem", статусToolStripMenuItem_Click);
            расчетыToolStripMenuItem = Item("расчетыToolStripMenuItem", null, открытьИнструкцииToolStripMenuItem,
                сформироватьИнструкцииToolStripMenuItem, запуститьToolStripMenuItem, остановитьToolStripMenuItem, статусToolStripMenuItem);
            расчетыToolStripMenuItem.IsEnabled = false;

            открытьToolStripMenuItem1 = Item("открытьToolStripMenuItem1", открытьToolStripMenuItem1_Click);
            объединитьToolStripMenuItem = Item("объединитьToolStripMenuItem", MergeDataBase_Click);
            объединитьToolStripMenuItem.IsEnabled = false;
            построитьГрафикToolStripMenuItem = Item("построитьГрафикToolStripMenuItem", построитьГрафикToolStripMenuItem_Click);
            построитьДиаграммуToolStripMenuItem = Item("построитьДиаграммуToolStripMenuItem", построитьДиаграммуToolStripMenuItem_Click);
            создатьАнимациюToolStripMenuItem = Item("создатьАнимациюToolStripMenuItem", создатьАнимациюToolStripMenuItem_Click);
            создатьАнимациюToolStripMenuItem.ToggleType = MenuItemToggleType.CheckBox;
            // В BaseForm у экспорта результатов нет обработчика.
            экспортироватьРезультатыToolStripMenuItem = Item("экспортироватьРезультатыToolStripMenuItem", null);
            экспортироватьРезультатыToolStripMenuItem.IsEnabled = false;
            toolStripMenuItem4 = Item("toolStripMenuItem4", отзеркаливаниеToolStripMenuItem_Click);
            toolStripMenuItem4.ToggleType = MenuItemToggleType.CheckBox;
            результатыMenuItem = Item("результатыMenuItem", null, открытьToolStripMenuItem1, объединитьToolStripMenuItem,
                построитьГрафикToolStripMenuItem, построитьДиаграммуToolStripMenuItem, создатьАнимациюToolStripMenuItem,
                экспортироватьРезультатыToolStripMenuItem, toolStripMenuItem4);
            результатыMenuItem.IsEnabled = false;

            измеритьToolStripMenuItem = Item("измеритьToolStripMenuItem", измеритьToolStripMenuItem_Click);
            измеритьToolStripMenuItem.ToggleType = MenuItemToggleType.CheckBox;
            скрытьПлоскостьюToolStripMenuItem = Item("скрытьПлоскостьюToolStripMenuItem", скрытьПлоскостьюToolStripMenuItem_Click);
            скрытьПлоскостьюToolStripMenuItem.ToggleType = MenuItemToggleType.CheckBox;
            // В BaseForm у пункта нет обработчика, а окно сечения (btnCrossSection) не подключено к интерфейсу;
            // пункт открывает перенесённое окно сечения, но, как в BaseForm.resx, отключён.
            рассечьПлоскостьюToolStripMenuItem = Item("рассечьПлоскостьюToolStripMenuItem", рассечьПлоскостьюToolStripMenuItem_Click);
            рассечьПлоскостьюToolStripMenuItem.ToggleType = MenuItemToggleType.CheckBox;
            рассечьПлоскостьюToolStripMenuItem.IsEnabled = false;
            инструментыToolStripMenuItem = Item("инструментыToolStripMenuItem", null, измеритьToolStripMenuItem,
                скрытьПлоскостьюToolStripMenuItem, рассечьПлоскостьюToolStripMenuItem);
            инструментыToolStripMenuItem.IsEnabled = false;
            настройкиToolStripMenuItem = Item("настройкиToolStripMenuItem", настройкиToolStripMenuItem_Click);
            настройкиToolStripMenuItem.ToggleType = MenuItemToggleType.CheckBox;

            содержаниеToolStripMenuItem = Item("содержаниеToolStripMenuItem", содержаниеToolStripMenuItem_Click);
            опрограммеToolStripMenuItem = Item("опрограммеToolStripMenuItem", опрограммеToolStripMenuItem_Click);
            справкаToolStripMenuItem = Item("справкаToolStripMenuItem", null, содержаниеToolStripMenuItem, опрограммеToolStripMenuItem);
            сведенияMenuItem = Item("сведенияMenuItem", сведенияMenuItem_Click);
            лицензияToolStripMenuItem = Item("лицензияToolStripMenuItem", null, сведенияMenuItem);

            menuStrip = new Menu
            {
                Background = Brush.Parse("#F0F0F0"),
                Height = 24,
                FontFamily = new FontFamily("Segoe UI"),
                FontSize = 11,
                ItemsSource = new[]
                {
                    файлToolStripMenuItem, viewMenuItem, геометрияToolStripMenuItem, сеткаToolStripMenuItem, dataBasesMenuItem,
                    tasksMenuItem, расчетыToolStripMenuItem, результатыMenuItem, инструментыToolStripMenuItem,
                    настройкиToolStripMenuItem, справкаToolStripMenuItem, лицензияToolStripMenuItem
                }
            };

            // Fluent задаёт радиус панели Popup локальной ссылкой на ресурс, поверх сеттеров стиля.
            menuStrip.Resources["OverlayCornerRadius"] = new CornerRadius(0);

            // Компоновка: splitContainer3 (навигатор | сцена+консоль), splitContainer2 (сцена / консоль).
            var root = new Grid { RowDefinitions = new RowDefinitions("Auto,*,Auto") };
            root.Children.Add(menuStrip);

            splitContainer3 = new Grid { ColumnDefinitions = new ColumnDefinitions("304,8,*"), MinHeight = 0, Margin = new Thickness(5) };
            splitContainer3.ColumnDefinitions[0].MinWidth = NavigatorPanelMinWidth;
            splitContainer3.ColumnDefinitions[2].MinWidth = PanelMinSize;
            Grid.SetRow(splitContainer3, 1);
            root.Children.Add(splitContainer3);

            cntrНавигатор = new Grid { RowDefinitions = new RowDefinitions("298*,8,253*") };
            cntrНавигатор.RowDefinitions[0].MinHeight = cntrНавигатор.RowDefinitions[2].MinHeight = PanelMinSize;
            cntrНавигатор.Children.Add(CreatePinnedPage(Resources.NavigatorControl_headerName_text, navigator));
            var navigatorSplitter = new GridSplitter { Height = 8, ResizeDirection = GridResizeDirection.Rows, Background = Brush.Parse("#F0F0F0") };
            Grid.SetRow(navigatorSplitter, 1);
            cntrНавигатор.Children.Add(navigatorSplitter);
            // tableLayoutPanel1 в BaseForm: панель свойств и под ней плеер проверки условий/результатов.
            var propertiesPage = CreatePinnedPage(Resources.PropertiesPanelControl_headerName_text, propertiesPanel);
            var tableLayoutPanel1 = new Grid { RowDefinitions = new RowDefinitions("*,Auto") };
            tableLayoutPanel1.Children.Add(propertiesPage);
            Grid.SetRow(checkPlayerControl, 1);
            tableLayoutPanel1.Children.Add(checkPlayerControl);
            Grid.SetRow(tableLayoutPanel1, 2);
            cntrНавигатор.Children.Add(tableLayoutPanel1);
            splitContainer3Panel1 = new Grid();
            splitContainer3.Children.Add(splitContainer3Panel1);

            splitContainer3Splitter = new GridSplitter { Width = 8, ResizeDirection = GridResizeDirection.Columns, Background = Brush.Parse("#F0F0F0") };
            Grid.SetColumn(splitContainer3Splitter, 1);
            splitContainer3.Children.Add(splitContainer3Splitter);

            splitContainer2 = new Grid { RowDefinitions = new RowDefinitions("*,8,140"), MinWidth = 0 };
            splitContainer2.RowDefinitions[0].MinHeight = PanelMinSize;
            splitContainer2.RowDefinitions[2].MinHeight = ConsolePanelMinHeight;
            Grid.SetColumn(splitContainer2, 2);
            splitContainer3.Children.Add(splitContainer2);

            // splitContainer2.Panel1: место сцены. Сама сцена и её кнопки (btnSelect, виды, вписывание и т.д.) — SceneView.
            splitContainer2.Children.Add(scene);

            splitContainer2Splitter = new GridSplitter { Height = 8, ResizeDirection = GridResizeDirection.Rows, Background = Brush.Parse("#F0F0F0") };
            Grid.SetRow(splitContainer2Splitter, 1);
            splitContainer2.Children.Add(splitContainer2Splitter);
            Grid.SetRow(console, 2);
            splitContainer2.Children.Add(console);

            lblStatus = new TextBlock { Text = resources.GetString("lblStatus.Text"), VerticalAlignment = VerticalAlignment.Center, TextTrimming = TextTrimming.CharacterEllipsis };
            lblVersion = new TextBlock { VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(12, 0, 0, 0) };
            var statusStrip = new DockPanel { LastChildFill = true };
            DockPanel.SetDock(lblVersion, Dock.Right);
            statusStrip.Children.Add(lblVersion);
            statusStrip.Children.Add(lblStatus);
            var statusBorder = new Border { Background = Brush.Parse("#F0F0F0"), Height = 32, Padding = new Thickness(5, 3), Child = statusStrip };
            Grid.SetRow(statusBorder, 2);
            root.Children.Add(statusBorder);
            Content = root;

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

        private static MenuItem Item(string name, EventHandler<RoutedEventArgs> click, params object[] items)
        {
            // Avalonia использует "_" вместо "&" для клавиши доступа.
            var item = new MenuItem { Name = name, Header = (resources.GetString(name + ".Text") ?? name).Replace('&', '_') };
            if (items.Length > 0)
                item.ItemsSource = items;
            if (click != null)
                item.Click += click;
            return item;
        }

        private static Border CreatePinnedPage(string title, Control content)
        {
            var layout = new Grid { RowDefinitions = new RowDefinitions("20,*") };
            var header = new Border
            {
                Background = Brushes.Gainsboro,
                BorderBrush = Brush.Parse("#A0A0A0"),
                BorderThickness = new Thickness(0, 0, 0, 1),
                Child = new TextBlock
                {
                    Text = title,
                    Foreground = Brushes.Black,
                    Margin = new Thickness(14, 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Center
                }
            };
            layout.Children.Add(header);
            Grid.SetRow(content, 1);
            layout.Children.Add(content);
            return new Border
            {
                BorderBrush = Brush.Parse("#7A7A7A"),
                BorderThickness = new Thickness(1),
                Background = Brush.Parse("#F0F0F0"),
                Child = layout
            };
        }

    }
}
