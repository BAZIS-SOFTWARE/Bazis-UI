using Avalonia;
using Avalonia.Automation;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Color = System.Drawing.Color;

namespace BazisAvaloniaGUI.Databases;

internal class GraphPoint(float x, float y)
{
    public float X { get; } = x;
    public float Y { get; } = y;
}
internal sealed class GraphData(string name, Color color, string xUnit, string yUnit, GraphPoint[] points)
{
    public string Name { get; } = name;
    public Color Color { get; } = color;
    public string XUnit { get; } = xUnit;
    public string YUnit { get; } = yUnit;
    public GraphPoint[] Points { get; } = points;
    public bool ValueFlag { get; set; }
    public bool IsShown { get; set; } = true;
    public bool IsTitleShown { get; set; } = true;
    public float Thickness { get; set; } = 1;
}
internal enum StepFormat { normal, logarithmic }
internal sealed class AxisFormat
{
    public StepFormat StepFormat { get; set; }
    public int NumberOfSings { get; set; } = 2;
}

// Avalonia adaptation of UserControlEx.Graph.GraphContainer and its GraphControl.
internal sealed class GraphContainer : UserControl
{
    private readonly GraphControl graphControl;
    private readonly Button dashButton, lineButton, btnValue, btnTitle, showDataSplitButton;
    private readonly TextBox txb_X_Max, txb_X_Min, txb_Y_Max, txb_Y_Min;
    private Point iniPos;
    private AxisFormat xAxis = new(), yAxis = new();
    public IReadOnlyList<GraphData> CurrentData { get; private set; } = [];
    public string Header { get; private set; }
    internal float X_max { get; private set; } = 10;
    internal float X_min { get; private set; }
    internal float Y_max { get; private set; } = 10;
    internal float Y_min { get; private set; }
    internal bool DashPaintFlag { get; private set; }
    internal bool LinePaintFlag { get; private set; } = true;
    internal bool ValueFlag { get; private set; }

    public GraphContainer()
    {
        Styles.Add(new StyleInclude(new Uri("avares://BazisAvaloniaGUI/")) { Source = new Uri("avares://BazisAvaloniaGUI/Databases/DatabaseStyles.axaml") });
        var toolbar = new DatabaseToolbar();
        graphControl = new GraphControl(this) { Name = "graphControl" };
        Button Tool(string name, string image, string tip, EventHandler<RoutedEventArgs> click, bool split = false)
        {
            var button = new Button { Name = name, Content = DatabaseIcons.Create(image) };
            button.Classes.Add("database-tool");
            if (split)
            {
                var content = new Grid { ColumnDefinitions = new ColumnDefinitions("16,12") };
                var icon = (Image)button.Content; button.Content = null; content.Children.Add(icon);
                var arrow = new Avalonia.Controls.Shapes.Path { Name = "GraphDropdownArrow", Data = Avalonia.Media.Geometry.Parse("M0,0 L6,0 L3,3 Z"), Fill = Brushes.Black, Width = 6, Height = 3 };
                Grid.SetColumn(arrow, 1); content.Children.Add(arrow); button.Content = content; button.Width = 45;
                button.AddHandler(PointerReleasedEvent, (_, e) =>
                {
                    if (e.GetPosition(button).X >= button.Width - 12) button.ContextMenu?.Open(button);
                }, RoutingStrategies.Tunnel);
                button.KeyDown += (_, e) => { if (e.Key == Key.Down || e.Key == Key.F4) { button.ContextMenu?.Open(button); e.Handled = true; } };
            }
            AutomationProperties.SetName(button, tip); ToolTip.SetTip(button, tip);
            button.Click += click; toolbar.Add(button, split ? 48 : 27); return button;
        }
        dashButton = Tool("dashButton", "graph-dash.png", "Трассировка", DashPaintButton_CheckedChanged);
        lineButton = Tool("lineButton", "graph-line.png", "Разметка", LinePaintButton_CheckedChanged);
        lineButton.Classes.Add("graph-checked");
        btnValue = Tool("btnValue", "graph-show-values.png", "Показать значения", ValueButton_CheckedChanged);
        btnTitle = Tool("btnTitle", "graph-title.png", "Показать названия", btnTitle_Click);
        btnTitle.Classes.Add("graph-checked");
        var thickness = Tool("btnPathThick", "graph-path-thickness.png", "Толщина линии", (_, _) => { }, true);
        thickness.ContextMenu = new ContextMenu();
        for (var i = 0; i < 3; i++)
        {
            var value = 1f + i * 2;
            var item = new MenuItem { Header = DatabaseIcons.Create($"graph-path-thickness-{i}.png"), Name = $"PathThickness{value}" };
            AutomationProperties.SetName(item, value.ToString(CultureInfo.InvariantCulture));
            item.Click += (_, _) => { foreach (var data in CurrentData) data.Thickness = value; graphControl.InvalidateVisual(); };
            thickness.ContextMenu.Items.Add(item);
        }
        Tool("btnValueToTable", "graph-export-values.png", "Свести данные в таблицу", btnValueToTable_Click);
        TextBox AxisInput(string label, string name, Action<string> apply)
        {
            var field = new TextBox { Name = name, Width = 42, Height = 20, MinHeight = 20, Padding = new Thickness(2, 0), FontSize = 11 };
            void Commit(string text)
            {
                try { apply(text); }
                catch (Exception ex) { MessageBox.Show(this, ex.Message); }
                Set_X_Y_Value(); graphControl.InvalidateVisual();
            }
            field.LostFocus += (_, _) => Commit(field.Text);
            field.KeyDown += (_, e) => { if (e.Key == Key.Enter) { Commit(field.Text); e.Handled = true; } };
            var panel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(3, 3, 0, 3), VerticalAlignment = VerticalAlignment.Center };
            panel.Children.Add(new TextBlock { Text = label, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 3, 0) });
            panel.Children.Add(field);
            toolbar.Add(panel, 79, () =>
            {
                var menuField = new TextBox { Text = field.Text, Width = 70, Height = 20, MinHeight = 20, Padding = new Thickness(2, 0) };
                menuField.KeyDown += (_, e) => { if (e.Key == Key.Enter) { Commit(menuField.Text); e.Handled = true; } };
                menuField.LostFocus += (_, _) => Commit(menuField.Text);
                return new MenuItem { Header = label, Items = { new MenuItem { Header = menuField } } };
            });
            return field;
        }
        float Parse(string text, bool logarithmic = false)
        {
            if (!float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var result) || !float.IsFinite(result)
                || logarithmic && result <= 0) throw new FormatException("Неправильный формат ввода!");
            return logarithmic ? (float)Math.Log10(result) : result;
        }
        txb_X_Max = AxisInput("Xmax", "txb_X_Max", text => { var x = Parse(text, xAxis.StepFormat == StepFormat.logarithmic); if (x != X_min) X_max = x; });
        txb_X_Min = AxisInput("Xmin", "txb_X_Min", text => { var x = Parse(text, xAxis.StepFormat == StepFormat.logarithmic); if (x != X_max) X_min = x; });
        txb_Y_Max = AxisInput("Ymax", "txb_Y_Max", text => { var y = Parse(text); if (y != Y_min) Y_max = y; });
        txb_Y_Min = AxisInput("Ymin", "txb_Y_Min", text => { var y = Parse(text); if (y != Y_max) Y_min = y; });
        showDataSplitButton = Tool("showDataSplitButton", "graph-toggle-data.png", "Показать графики", (_, _) => { }, true);
        Tool("btnFitGraph", "graph-fit.png", "Вписать график", btnFitGraph_Click);
        var layout = new Grid { RowDefinitions = new RowDefinitions("Auto,*") };
        layout.Children.Add(toolbar); Grid.SetRow(graphControl, 1); layout.Children.Add(graphControl); Content = layout;
        graphControl.PointerPressed += (_, e) => { iniPos = e.GetPosition(graphControl); if (e.GetCurrentPoint(graphControl).Properties.IsRightButtonPressed) e.Pointer.Capture(graphControl); };
        graphControl.PointerReleased += (_, e) => e.Pointer.Capture(null);
        graphControl.PointerMoved += Graph_MouseMove;
        graphControl.PointerWheelChanged += Graph_MouseWheel;
        Set_X_Y_Value();
    }

    private bool Toggle(Button button)
    {
        var value = !button.Classes.Contains("graph-checked");
        button.Classes.Set("graph-checked", value); return value;
    }
    private void DashPaintButton_CheckedChanged(object sender, RoutedEventArgs e) { DashPaintFlag = Toggle(dashButton); graphControl.InvalidateVisual(); }
    private void LinePaintButton_CheckedChanged(object sender, RoutedEventArgs e) { LinePaintFlag = Toggle(lineButton); graphControl.InvalidateVisual(); }
    private void ValueButton_CheckedChanged(object sender, RoutedEventArgs e) { ValueFlag = Toggle(btnValue); graphControl.InvalidateVisual(); }
    private void btnTitle_Click(object sender, RoutedEventArgs e)
    {
        var value = Toggle(btnTitle); foreach (var data in CurrentData) data.IsTitleShown = value; graphControl.InvalidateVisual();
    }
    public void ClearData()
    {
        Header = null; CurrentData = []; showDataSplitButton.ContextMenu = new ContextMenu(); graphControl.InvalidateVisual();
    }
    public void CreateGraphData(string header, List<GraphData> data, AxisFormat xFormat, AxisFormat yFormat)
    {
        Header = header; CurrentData = data; xAxis = xFormat; yAxis = yFormat;
        // Новые серии создаются с IsTitleShown = true: без этого после отключения названий кнопка
        // оставалась отжатой, а названия новых графиков рисовались
        var titlesShown = btnTitle.Classes.Contains("graph-checked");
        foreach (var series in data) series.IsTitleShown = titlesShown;
        Set_Max_Min_X_Y(); Set_X_Y_Value(); graphControl.InvalidateVisual();
        var menu = new ContextMenu();
        foreach (var series in data)
        {
            var item = new MenuItem { Header = series.Name, Name = series.Name, ToggleType = MenuItemToggleType.CheckBox, IsChecked = series.IsShown, StaysOpenOnClick = true };
            item.Click += (_, _) => { series.IsShown = item.IsChecked; graphControl.InvalidateVisual(); };
            menu.Items.Add(item);
        }
        showDataSplitButton.ContextMenu = menu;
    }
    private void Set_Max_Min_X_Y()
    {
        var points = CurrentData.SelectMany(d => d.Points).Where(p => float.IsFinite(p.X) && float.IsFinite(p.Y)).ToArray();
        if (points.Length == 0) { X_min = Y_min = 0; X_max = Y_max = 1; return; }
        X_min = points.Min(p => p.X); X_max = points.Max(p => p.X);
        if (X_min.Equals(X_max)) { if (X_max > 0) X_min = 0; else if (X_max < 0) X_max = 0; else X_max = 1; }
        Y_min = points.Min(p => p.Y); Y_max = points.Max(p => p.Y);
        if (Y_min.Equals(Y_max)) { if (Y_max > 0) Y_min = 0; else if (Y_max < 0) Y_max = 0; else Y_max = 1; }
    }
    private void Set_X_Y_Value()
    {
        txb_X_Max.Text = (xAxis.StepFormat == StepFormat.logarithmic ? Math.Pow(10, X_max) : X_max).ToString(CultureInfo.InvariantCulture);
        txb_X_Min.Text = (xAxis.StepFormat == StepFormat.logarithmic ? Math.Pow(10, X_min) : X_min).ToString(CultureInfo.InvariantCulture);
        txb_Y_Max.Text = Y_max.ToString(CultureInfo.InvariantCulture); txb_Y_Min.Text = Y_min.ToString(CultureInfo.InvariantCulture);
    }
    private void Graph_MouseMove(object sender, PointerEventArgs e)
    {
        var finPos = e.GetPosition(graphControl);
        if (e.GetCurrentPoint(graphControl).Properties.IsRightButtonPressed && graphControl.Bounds.Width > 0 && graphControl.Bounds.Height > 0)
        {
            var stepX = (float)(finPos.X - iniPos.X) / (float)graphControl.Bounds.Width * (X_max - X_min);
            var stepY = (float)(finPos.Y - iniPos.Y) / (float)graphControl.Bounds.Height * (Y_max - Y_min);
            X_max -= stepX; X_min -= stepX; Y_max += stepY; Y_min += stepY;
            Set_X_Y_Value(); graphControl.InvalidateVisual();
        }
        iniPos = finPos;
    }
    private void Graph_MouseWheel(object sender, PointerWheelEventArgs e)
    {
        // Масштабируем диапазон относительно его центра: умножение только максимумов при ненулевом
        // минимуме (X = 100..110) делало max < min, и график переворачивался
        var factor = e.Delta.Y > 0 ? 0.9f : 1.1f;
        var xCenter = (X_max + X_min) / 2; var xHalf = (X_max - X_min) / 2 * factor;
        var yCenter = (Y_max + Y_min) / 2; var yHalf = (Y_max - Y_min) / 2 * factor;
        X_min = xCenter - xHalf; X_max = xCenter + xHalf;
        Y_min = yCenter - yHalf; Y_max = yCenter + yHalf;
        Set_X_Y_Value(); graphControl.InvalidateVisual(); e.Handled = true;
    }
    private async void btnValueToTable_Click(object sender, RoutedEventArgs e)
    {
        var builder = new StringBuilder();
        foreach (var data in CurrentData)
            foreach (var point in data.Points) builder.AppendLine(string.Format("\"{0} {1}\"", point.X.ToString("0.00"), point.Y.ToString("0.00")));
        try
        {
            var clipboard = TopLevel.GetTopLevel(this)?.Clipboard;
            if (clipboard != null) await clipboard.SetTextAsync(builder.ToString());
        }
        catch (Exception ex) { await MessageBox.Show(this, ex.Message); }
    }
    private void btnFitGraph_Click(object sender, RoutedEventArgs e) { Set_Max_Min_X_Y(); Set_X_Y_Value(); graphControl.InvalidateVisual(); }

    private sealed class GraphControl(GraphContainer owner) : Control
    {
        public override void Render(DrawingContext context)
        {
            base.Render(context);
            context.DrawRectangle(Brush.Parse("#F0F0F0"), null, new Rect(Bounds.Size));
            var points = owner.CurrentData.SelectMany(d => d.Points).Where(p => float.IsFinite(p.X) && float.IsFinite(p.Y)).ToArray();
            if (points.Length == 0 || Bounds.Width < 100 || Bounds.Height < 80) return;
            FormattedText Text(string value, double size = 12) => new(value ?? "", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Microsoft Sans Serif"), size, Brushes.Black);
            void Label(string value, Point center, double size = 12)
            {
                var text = Text(value, size); context.DrawText(text, new Point(center.X - text.Width / 2, center.Y - text.Height / 2));
            }
            var deviation = points.Max(p => Text(p.Y.ToString()).Width);
            var marginX = Math.Min(15 + deviation, Bounds.Width / 3);
            var plot = new Rect(marginX, 30, Bounds.Width - marginX * 2, Bounds.Height - 60);
            Point Map(GraphPoint p) => new(plot.Left + (p.X - owner.X_min) / (owner.X_max - owner.X_min) * plot.Width,
                plot.Bottom - (p.Y - owner.Y_min) / (owner.Y_max - owner.Y_min) * plot.Height);
            context.DrawRectangle(null, new Pen(Brush.Parse("#B4B4B4"), 1), plot);
            Label(owner.Header?.Split(',')[0], new Point(Bounds.Width / 2, 10), 14);
            for (var i = 0; i <= 5; i++)
            {
                var x = owner.X_min + (owner.X_max - owner.X_min) * i / 5;
                var y = owner.Y_min + (owner.Y_max - owner.Y_min) * i / 5;
                var xPixel = plot.Left + plot.Width * i / 5; var yPixel = plot.Bottom - plot.Height * i / 5;
                if (owner.LinePaintFlag)
                {
                    var pen = new Pen(Brushes.LightGray, .25);
                    context.DrawLine(pen, new Point(plot.Left, yPixel), new Point(plot.Right, yPixel));
                    context.DrawLine(pen, new Point(xPixel, plot.Top), new Point(xPixel, plot.Bottom));
                }
                // "0." а не "#.": с "#." ноль подписывался как ".00" (или пустой строкой при NumberOfSings = 0)
                Label((owner.xAxis.StepFormat == StepFormat.logarithmic ? Math.Pow(10, x) : x).ToString("0." + new string('0', owner.xAxis.NumberOfSings)), new Point(xPixel, Bounds.Height - 15));
                Label((owner.yAxis.StepFormat == StepFormat.logarithmic ? Math.Pow(10, y) : y).ToString("0." + new string('0', owner.yAxis.NumberOfSings)), new Point(plot.Left / 2, yPixel));
            }
            Label("X," + owner.CurrentData[0].XUnit, new Point(plot.Right + marginX / 2, plot.Bottom), 11);
            Label("Y," + owner.CurrentData[0].YUnit, new Point(plot.Left / 2, 16), 11);
            using (context.PushClip(plot))
                foreach (var series in owner.CurrentData)
                {
                    if (!series.IsShown) continue;
                    var brush = new SolidColorBrush(Avalonia.Media.Color.FromArgb(series.Color.A, series.Color.R, series.Color.G, series.Color.B));
                    var pen = new Pen(brush, series.Thickness);
                    var finite = series.Points.Where(p => float.IsFinite(p.X) && float.IsFinite(p.Y)).ToArray();
                    for (var i = 1; i < finite.Length; i++) context.DrawLine(pen, Map(finite[i - 1]), Map(finite[i]));
                    foreach (var point in finite)
                    {
                        var position = Map(point);
                        if (owner.DashPaintFlag)
                        {
                            var dash = new Pen(Brushes.Black, 1, new DashStyle([10d, 10d], 0));
                            context.DrawLine(dash, new Point(plot.Left, position.Y), position);
                            context.DrawLine(dash, new Point(position.X, plot.Bottom), position);
                        }
                        if (owner.ValueFlag && series.ValueFlag)
                        {
                            context.DrawRectangle(brush, new Pen(Brushes.Gray, .5), new Rect(position.X - 2, position.Y - 2, 5, 5));
                            Label($"[{point.X},{point.Y}]", new Point(position.X, position.Y - 10));
                        }
                    }
                    if (series.IsTitleShown && finite.Length > 0)
                    {
                        var position = Map(finite[finite.Length / 2]); Label(series.Name, new Point(position.X, position.Y - 25), 14);
                    }
                }
        }
    }
}
