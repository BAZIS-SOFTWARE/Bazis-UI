using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using BazisAvaloniaGUI.Properties;
using BazisGUI.Scene.Interfaces;
using System;
using System.Drawing;
using System.Linq;
using System.Resources;
using Color = System.Drawing.Color;

namespace BazisAvaloniaGUI.SettingsControls
{
    /// <summary>
    /// Avalonia-аналог SettingsControl. TabControlEx заменён TabControl, ColorSlider — Slider,
    /// панели цвета — Border, ColorDialog и OpenFileDialog — ColorSelectionDialog и StorageProvider.
    /// </summary>
    internal sealed class SettingsControl : UserControl
    {
        enum Culture { en, ru }
        public event Action<Color> SetSelectionObjectColorEvent;
        public event Action<Color> SetSelectionGroupColorEvent;
        public event Action<Color> SetBackGroundColorEvent;

        public event Action<Color> Set3DElemColorEvent;
        public event Action<Color> Set2DElemColorEvent;
        public event Action<Color> SetNodeColorEvent;

        public event Action<string> SetSolverPathEvent;
        public event Action<bool> SetLightingEvent;
        /// <summary>«Рёбра внутренних элементов»: показ задних рёбер поверхностей при прозрачной отрисовке.</summary>
        public event Action<bool> SetBackRibbersEvent;
        public Action<int> SetLightingIntensityEvent;
        public Action<System.Drawing.Point> SetLighterPositionEvent;
        public Action<bool> SetTransparencyEvent;
        public Action<int> SetTransparencyValueEvent;

        public Action<bool> SetOrtoProjectionEvent;

        public Action<string> SetLanguageEvent;

        private readonly ResourceManager resources = new(typeof(SettingsControl));

        private Border panelBackGroundColor;
        private Border pnlSelectionObjsColor;
        private Border pnlSelectionGroupColor;
        private Border pnl3DElemColor;
        private Border pnl2DElemColor;
        private Border pnlNodeColor;
        private TextBlock lblSolverPath;
        private CheckBox chbLighting;
        private CheckBox chbBackRibbers;
        private CheckBox chbTransparency;
        private CheckBox chbOrtoProjection;
        private Slider clslLigthingIntensity;
        private Slider clslTransparency;
        private LightingControl lightingControl;
        private ComboBox cmbLanguage;

        public SettingsControl()
        {
            InitializeComponent();

            lightingControl.SetBallPositionEvent += (ar) =>
            {
                SetLighterPositionEvent?.Invoke(ar);
            };

            cmbLanguage.ItemsSource = Enum.GetValues<Culture>().Select(GetLanguageByCulture).ToArray();
        }

        /// <summary>
        /// Идёт заполнение страницы из конфигурации. В Avalonia ValueChanged/SelectionChanged срабатывают и при
        /// программной установке значения (в WinForms Scroll — только от пользователя), поэтому события
        /// изменения настроек в это время не поднимаются.
        /// </summary>
        private bool isLoading;

        public void SetSettings(SettingsConfig settingsConfig)
        {
            isLoading = true;
            try
            {
                LoadSettings(settingsConfig);
            }
            finally
            {
                isLoading = false;
            }
        }

        private void LoadSettings(SettingsConfig settingsConfig)
        {
            SetPanelColor(panelBackGroundColor, settingsConfig.BackGroundColor);
            SetPanelColor(pnlSelectionObjsColor, settingsConfig.SelectObjectColor);
            SetPanelColor(pnlSelectionGroupColor, settingsConfig.SelectGroupColor);
            lblSolverPath.Text = settingsConfig.SolverPath;
            chbLighting.IsChecked = settingsConfig.Lighting;
            chbBackRibbers.IsChecked = settingsConfig.BackRibbers;
            lightingControl.BallPosition = settingsConfig.LighterPosition;
            clslLigthingIntensity.Value = settingsConfig.LightingIntensity;
            chbTransparency.IsChecked = settingsConfig.Transparency;
            SetPanelColor(pnlNodeColor, settingsConfig.NodeColor);
            chbOrtoProjection.IsChecked = settingsConfig.Projection == ViewProjection.Parallel ?
                true : false;
            clslTransparency.Value = settingsConfig.TransparencyValue;

            cmbLanguage.SelectedItem = GetLanguageByCulture(ParseCulture(settingsConfig.Language));
        }

        private Culture GetCultureByLanguage(string language)
        {
            switch (language)
            {
                case "Русский":
                    return Culture.ru;
                default:
                    return Culture.en;
            }
        }

        private string GetLanguageByCulture(Culture culture)
        {
            switch (culture)
            {
                case Culture.ru:
                    return "Русский";
                default:
                    return "English";
            }
        }

        private Culture ParseCulture(string culture)
        {
            switch (culture)
            {
                case "ru":
                    return Culture.ru;
                default:
                    return Culture.en;
            }
        }

        private async void btnSelectObjectColor_Click(object sender, RoutedEventArgs e)
        {
            var dialog = await ShowColorDialog(pnlSelectionObjsColor);

            if (dialog == null)
                return;

            SetPanelColor(pnlSelectionObjsColor, dialog.Value);

            SetSelectionObjectColorEvent?.Invoke(dialog.Value);
        }

        private async void btnSelectGroupColor_Click(object sender, RoutedEventArgs e)
        {
            var dialog = await ShowColorDialog(pnlSelectionGroupColor);

            if (dialog == null)
                return;

            SetPanelColor(pnlSelectionGroupColor, dialog.Value);

            SetSelectionGroupColorEvent?.Invoke(dialog.Value);
        }

        private async void btnBackGroundColor_Click(object sender, RoutedEventArgs e)
        {
            var dialog = await ShowColorDialog(panelBackGroundColor);

            if (dialog == null)
                return;

            SetPanelColor(panelBackGroundColor, dialog.Value);

            SetBackGroundColorEvent?.Invoke(dialog.Value);
        }

        private async void btnSetSolverPath_Click(object sender, RoutedEventArgs e)
        {
            if (TopLevel.GetTopLevel(this) is not { } topLevel)
                return;
            var dialog = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                AllowMultiple = false,
                FileTypeFilter = new[] { new FilePickerFileType("(*.exe)") { Patterns = new[] { "*.exe" } } }
            });

            // Файл не из локальной ФС (нет пути) — путь к решателю не меняется, а не сбрасывается в null.
            var path = dialog.Count == 0 ? null : dialog[0].TryGetLocalPath();
            if (string.IsNullOrEmpty(path))
                return;

            lblSolverPath.Text = path;

            SetSolverPathEvent?.Invoke(lblSolverPath.Text);
        }

        private void chbLighting_Click(object sender, RoutedEventArgs e)
        {
            SetLightingEvent?.Invoke(chbLighting.IsChecked == true);
        }

        private void chbTransparency_Click(object sender, RoutedEventArgs e)
        {
            SetTransparencyEvent?.Invoke(chbTransparency.IsChecked == true);
        }

        private void chbBackRibbers_Click(object sender, RoutedEventArgs e)
        {
            // В WinForms обработчик пустой; флажок управляет показом задних рёбер в рендере прозрачности.
            SetBackRibbersEvent?.Invoke(chbBackRibbers.IsChecked == true);
        }

        private void clslLigthingIntensity_Scroll(object sender, Avalonia.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            if (isLoading)
                return;
            SetLightingIntensityEvent?.Invoke((int)e.NewValue);
        }

        private void clslTransparency_Scroll(object sender, Avalonia.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            if (isLoading)
                return;
            SetTransparencyValueEvent?.Invoke((int)e.NewValue);
        }

        private async void btnSelect3DElemColor_Click(object sender, RoutedEventArgs e)
        {
            var dialog = await ShowColorDialog(pnl3DElemColor);

            if (dialog == null)
                return;

            SetPanelColor(pnl3DElemColor, dialog.Value);

            Set3DElemColorEvent?.Invoke(dialog.Value);
        }

        private async void btnSelect2DElemColor_Click(object sender, RoutedEventArgs e)
        {
            var dialog = await ShowColorDialog(pnl2DElemColor);

            if (dialog == null)
                return;

            SetPanelColor(pnl2DElemColor, dialog.Value);

            Set2DElemColorEvent?.Invoke(dialog.Value);
        }

        private async void btnSelectNodeColor_Click(object sender, RoutedEventArgs e)
        {
            var dialog = await ShowColorDialog(pnlNodeColor);

            if (dialog == null)
                return;

            SetPanelColor(pnlNodeColor, dialog.Value);

            SetNodeColorEvent?.Invoke(dialog.Value);
        }

        private void chbOrtoProjection_Click(object sender, RoutedEventArgs e)
        {
            if (chbOrtoProjection.IsChecked == true)
                SetOrtoProjectionEvent?.Invoke(true);
            else
                SetOrtoProjectionEvent?.Invoke(false);
        }

        private void cmbLanguage_TextChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isLoading)
                return;
            SetLanguageEvent?.Invoke(GetCultureByLanguage(cmbLanguage.SelectedItem as string).ToString());
        }

        /// <summary>Avalonia: аналог ColorDialog.ShowDialog, начальный цвет берётся из панели цвета.</summary>
        private System.Threading.Tasks.Task<Color?> ShowColorDialog(Border colorPanel)
        {
            var current = colorPanel.Tag is Color color ? color : Color.White;
            // Страница не в окне (вкладку уже закрыли): ShowDialog(null) бросил бы исключение в async void.
            if (TopLevel.GetTopLevel(this) is not Window owner)
                return System.Threading.Tasks.Task.FromResult<Color?>(null);
            return new ColorSelectionDialog(current).ShowDialog<Color?>(owner);
        }

        /// <summary>Avalonia: аналог присвоения Panel.BackColor.</summary>
        private static void SetPanelColor(Border colorPanel, Color color)
        {
            colorPanel.Tag = color;
            colorPanel.Background = new SolidColorBrush(Avalonia.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
        }

        private void InitializeComponent()
        {
            Styles.Add(new StyleInclude(new Uri("avares://BazisAvaloniaGUI/")) { Source = new Uri("avares://BazisAvaloniaGUI/Databases/DatabaseStyles.axaml") });
            Background = Avalonia.Media.Brushes.White;

            panelBackGroundColor = ColorPanel(Color.White);
            pnlSelectionObjsColor = ColorPanel(Color.LawnGreen);
            pnlSelectionGroupColor = ColorPanel(Color.Yellow);
            pnl3DElemColor = ColorPanel(Color.Yellow);
            pnl2DElemColor = ColorPanel(Color.Yellow);
            pnlNodeColor = ColorPanel(Color.Yellow);

            chbLighting = Toggle("chbLighting", chbLighting_Click);
            chbBackRibbers = Toggle("chbBackRibbers", chbBackRibbers_Click);
            chbTransparency = Toggle("chbTransparency", chbTransparency_Click);
            chbOrtoProjection = Toggle("chbOrtoProjection", chbOrtoProjection_Click);

            clslLigthingIntensity = new Slider { Name = "clslLigthingIntensity", Minimum = 0, Maximum = 100, SmallChange = 1, LargeChange = 5, IsSnapToTickEnabled = true, TickFrequency = 1 };
            clslLigthingIntensity.ValueChanged += clslLigthingIntensity_Scroll;
            clslTransparency = new Slider { Name = "clslTransparency", Minimum = 0, Maximum = 100, SmallChange = 1, LargeChange = 5, IsSnapToTickEnabled = true, TickFrequency = 1 };
            clslTransparency.ValueChanged += clslTransparency_Scroll;

            lightingControl = new LightingControl { Name = "lightingControl" };

            // tbScene
            var tableLayoutPanel1 = Table(8);
            tableLayoutPanel1.RowDefinitions[3].Height = new GridLength(1, GridUnitType.Star);
            AddCell(tableLayoutPanel1, Button("btnBackGroundColor", btnBackGroundColor_Click), 0, 0);
            AddCell(tableLayoutPanel1, panelBackGroundColor, 0, 1);
            AddCell(tableLayoutPanel1, Label("label1"), 1, 0);
            AddCell(tableLayoutPanel1, chbBackRibbers, 1, 1);
            AddCell(tableLayoutPanel1, Label("label2"), 2, 0);
            AddCell(tableLayoutPanel1, chbLighting, 2, 1);
            var lightingBorder = new Border { Background = Avalonia.Media.Brushes.White, BorderBrush = Avalonia.Media.Brushes.Black, BorderThickness = new Thickness(1), Child = lightingControl, MinHeight = 150 };
            Grid.SetColumnSpan(lightingBorder, 2);
            AddCell(tableLayoutPanel1, lightingBorder, 3, 0);
            // Ползунки растягиваются на всю ширину страницы, как рамка источника света.
            var lightingIntensity = SliderWithValue(clslLigthingIntensity);
            Grid.SetColumnSpan(lightingIntensity, 2);
            AddCell(tableLayoutPanel1, lightingIntensity, 4, 0);
            AddCell(tableLayoutPanel1, Label("label4"), 5, 0);
            AddCell(tableLayoutPanel1, chbTransparency, 5, 1);
            var transparencyValue = SliderWithValue(clslTransparency);
            Grid.SetColumnSpan(transparencyValue, 2);
            AddCell(tableLayoutPanel1, transparencyValue, 6, 0);
            AddCell(tableLayoutPanel1, Label("label5"), 7, 0);
            AddCell(tableLayoutPanel1, chbOrtoProjection, 7, 1);

            // tbObjects
            var tableLayoutPanel4 = Table(5);
            AddCell(tableLayoutPanel4, Button("btnSelectColor", btnSelectObjectColor_Click), 0, 0);
            AddCell(tableLayoutPanel4, pnlSelectionObjsColor, 0, 1);
            AddCell(tableLayoutPanel4, Button("btnSelectGroupColor", btnSelectGroupColor_Click), 1, 0);
            AddCell(tableLayoutPanel4, pnlSelectionGroupColor, 1, 1);
            AddCell(tableLayoutPanel4, Button("btnSelect3DElemColor", btnSelect3DElemColor_Click), 2, 0);
            AddCell(tableLayoutPanel4, pnl3DElemColor, 2, 1);
            AddCell(tableLayoutPanel4, Button("btnSelect2DElemColor", btnSelect2DElemColor_Click), 3, 0);
            AddCell(tableLayoutPanel4, pnl2DElemColor, 3, 1);
            AddCell(tableLayoutPanel4, Button("btnSelectNodeColor", btnSelectNodeColor_Click), 4, 0);
            AddCell(tableLayoutPanel4, pnlNodeColor, 4, 1);

            // tbSolver: путь к решателю выбирается щелчком по подписи пути.
            lblSolverPath = new TextBlock { Name = "lblSolverPath", Text = resources.GetString("lblSolverPath.Text"), Cursor = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Hand), TextWrapping = TextWrapping.Wrap, VerticalAlignment = VerticalAlignment.Center, Background = Avalonia.Media.Brushes.Transparent };
            lblSolverPath.PointerReleased += (sender, e) => btnSetSolverPath_Click(sender, e);
            var tableLayoutPanel5 = Table(1);
            AddCell(tableLayoutPanel5, Label("label3"), 0, 0);
            AddCell(tableLayoutPanel5, lblSolverPath, 0, 1);

            // tabPage1: язык
            cmbLanguage = new ComboBox { Name = "cmbLanguage", Height = 20, MinHeight = 20, MinWidth = 200, HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Center };
            cmbLanguage.SelectionChanged += cmbLanguage_TextChanged;
            var tableLayoutPanel2 = Table(1);
            AddCell(tableLayoutPanel2, Label("lblLanguage"), 0, 0);
            AddCell(tableLayoutPanel2, cmbLanguage, 0, 1);

            var tabControlEx1 = new TabControl
            {
                Name = "tabControlEx1",
                ItemsSource = new[]
                {
                    Tab("tbScene", tableLayoutPanel1),
                    Tab("tbObjects", tableLayoutPanel4),
                    Tab("tbSolver", tableLayoutPanel5),
                    Tab("tabPage1", tableLayoutPanel2)
                }
            };
            Content = tabControlEx1;
        }

        private TabItem Tab(string name, Control content) => new()
        {
            Name = name,
            Header = resources.GetString(name + ".Text"),
            Content = new ScrollViewer { Content = content }
        };

        private static Grid Table(int rows)
        {
            var table = new Grid { ColumnDefinitions = new ColumnDefinitions("2*,*"), Margin = new Thickness(8), RowSpacing = 16, ColumnSpacing = 16 };
            for (var i = 0; i < rows; i++)
                table.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            return table;
        }

        private static void AddCell(Grid table, Control control, int row, int column)
        {
            Grid.SetRow(control, row);
            Grid.SetColumn(control, column);
            table.Children.Add(control);
        }

        private TextBlock Label(string name) => new()
        {
            Name = name,
            Text = resources.GetString(name + ".Text"),
            TextWrapping = TextWrapping.Wrap,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };

        private Avalonia.Controls.Button Button(string name, EventHandler<RoutedEventArgs> click)
        {
            var button = new Avalonia.Controls.Button
            {
                Name = name,
                Content = new TextBlock { Text = resources.GetString(name + ".Text"), TextWrapping = TextWrapping.Wrap, TextAlignment = TextAlignment.Center },
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Center
            };
            button.Click += click;
            return button;
        }

        private CheckBox Toggle(string name, EventHandler<RoutedEventArgs> click)
        {
            var checkBox = new CheckBox { Name = name, Content = resources.GetString(name + ".Text"), VerticalAlignment = VerticalAlignment.Center };
            checkBox.Classes.Add("database-toggle");
            checkBox.Click += click;
            return checkBox;
        }

        /// <summary>Аналог ColorSlider.ShowTextValue: рядом со шкалой выводится текущее значение.</summary>
        private static Control SliderWithValue(Slider slider)
        {
            var value = new TextBlock { Text = ((int)slider.Value).ToString(), MinWidth = 24, VerticalAlignment = VerticalAlignment.Center };
            slider.ValueChanged += (_, e) => value.Text = ((int)e.NewValue).ToString();
            var panel = new DockPanel();
            DockPanel.SetDock(value, Dock.Right);
            panel.Children.Add(value);
            panel.Children.Add(slider);
            return panel;
        }

        private static Border ColorPanel(Color color)
        {
            var panel = new Border { BorderBrush = Avalonia.Media.Brushes.Black, BorderThickness = new Thickness(1), MinHeight = 31, VerticalAlignment = VerticalAlignment.Stretch };
            SetPanelColor(panel, color);
            return panel;
        }
    }
}
