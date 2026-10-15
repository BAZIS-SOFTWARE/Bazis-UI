using Avalonia;
using Avalonia.Automation;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Avalonia.VisualTree;
using Avalonia.Markup.Xaml.Styling;
using MaterialDB.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Color = System.Drawing.Color;

namespace BazisAvaloniaGUI.Databases;

internal class DataBasePage : UserControl
{
    protected new class Resources : Localization.Resources { }
    public event Action LoadEvent;
    public event Action<string> SaveEvent;
    public string DataExtension { get; set; }
    public ILoader Loader;
    public ISaver Saver;
    private Color headColor = Color.Silver;
    private readonly List<Border> headings = new();
    public Color HeadColor
    {
        get => headColor;
        set
        {
            headColor = value;
            foreach (var heading in headings) heading.Background = new SolidColorBrush(Avalonia.Media.Color.FromArgb(value.A, value.R, value.G, value.B));
        }
    }
    internal DatabaseTree TreeView { get; } = new();
    public DataTableEditor DataGridView { get; } = new();
    public GraphContainer GraphContainer { get; } = new();
    public bool LabelEditFlag { get; set; }
    public TableCell EditCell { get; private set; }
    public object OldCellValue { get; private set; }
    public object NewCellValue { get; private set; }
    internal List<Window> AuxiliaryWindows { get; } = new();
    internal IStorageProvider StorageProvider { get; set; }
    protected IStorageProvider FileStorage => StorageProvider ?? TopLevel.GetTopLevel(this).StorageProvider;

    public DataBasePage()
    {
        FontFamily = new FontFamily("Microsoft Sans Serif"); FontSize = 11;
        Styles.Add(new StyleInclude(new Uri("avares://BazisAvaloniaGUI/")) { Source = new Uri("avares://BazisAvaloniaGUI/Databases/DatabaseStyles.axaml") });
        var treeTools = new DatabaseToolbar();
        void Tool(DatabaseToolbar panel, string image, string name, Action<object, EventArgs> action)
        {
            var button = new Button { Content = DatabaseIcons.Create(image), Name = name };
            button.Classes.Add("database-tool");
            AutomationProperties.SetName(button, name);
            var key = name switch { "OpenFileDB" => "btnOpenDB.Text", "AddDB" => "btnAddDB.Text", "SaveDB" => "btnSafeFile.Text", "AddBranch" => "addBranchButton.Text", "DeleteBranch" => "delBrachButton.Text", "CreateCopy" => "btnCreateCopy.Text", "AddRow" => "btnAddNewRow.Text", "ClearRows" => "btnDelRow.Text", _ => "btnAscSort.Text" };
            ToolTip.SetTip(button, DatabaseControlResources.Get("DataBasePage", key));
            button.Click += (sender, e) => action(sender, e); panel.Add(button);
        }
        Tool(treeTools, "database-open.png", "OpenFileDB", (s,e) => OpenFileDB_Click(s, new RoutedEventArgs()));
        Tool(treeTools, "database-add.png", "AddDB", (s,e) => AddDB_Click(s, new RoutedEventArgs()));
        Tool(treeTools, "database-save.png", "SaveDB", SafeFileButton_Click);
        Tool(treeTools, "branch-add.png", "AddBranch", AddBranchButton_Click);
        Tool(treeTools, "branch-delete.png", "DeleteBranch", DelBrachButton_Click);
        Tool(treeTools, "database-copy.png", "CreateCopy", CreateCopy_Click);
        var tableTools = new DatabaseToolbar();
        Tool(tableTools, "table-row-add.png", "AddRow", AddNewRowButton_Click);
        Tool(tableTools, "table-rows-clear.png", "ClearRows", DelAllRowsButton_Click);
        Tool(tableTools, "table-sort-ascending.png", "SortRows", Resort_Click);
        Control Pane(string title, Control content, Control toolbar = null)
        {
            var grid = new Grid { RowDefinitions = new RowDefinitions("17,Auto,*") };
            var heading = new Border { Background = Brush.Parse("#C0C0C0"), Child = new TextBlock { Text = title, Margin = new Thickness(4,0) } };
            headings.Add(heading); grid.Children.Add(heading);
            if (toolbar != null) { Grid.SetRow(toolbar, 1); grid.Children.Add(toolbar); }
            Grid.SetRow(content, 2); grid.Children.Add(content);
            return new Border { BorderBrush = Brush.Parse("#7A7A7A"), BorderThickness = new Thickness(1), Child = grid };
        }
        var treePane = Pane(Resources.List, TreeView, treeTools);
        var tablePane = Pane(Resources.Data, DataGridView, tableTools);
        var graphPane = Pane(Resources.Graph, GraphContainer);
        var layout = new Grid { ColumnDefinitions = new ColumnDefinitions("29*,3,71*"), RowDefinitions = new RowDefinitions("47.55*,3,52.45*") };
        Grid.SetRowSpan(treePane, 3); layout.Children.Add(treePane);
        Grid.SetColumn(tablePane, 2); layout.Children.Add(tablePane);
        Grid.SetColumn(graphPane, 2); Grid.SetRow(graphPane, 2); layout.Children.Add(graphPane);
        var vertical = new GridSplitter { ResizeDirection = GridResizeDirection.Columns, HorizontalAlignment = HorizontalAlignment.Stretch };
        Grid.SetColumn(vertical, 1); Grid.SetRowSpan(vertical, 3); layout.Children.Add(vertical);
        var horizontal = new GridSplitter { ResizeDirection = GridResizeDirection.Rows, VerticalAlignment = VerticalAlignment.Stretch };
        Grid.SetColumn(horizontal, 2); Grid.SetRow(horizontal, 1); layout.Children.Add(horizontal);
        Content = layout;
        TreeView.SelectionChanged += (_,_) =>
        {
            if (TreeView.SelectedNode != null) TreeView_AfterSelect(TreeView, new TreeViewEventArgs(TreeView.SelectedNode));
            // Выделение снято (например, удалён выбранный узел): таблица удалённой ветки не должна
            // оставаться редактируемой — правки ушли бы в данные, которых уже нет в дереве
            else { DataGridView.DataSource = null; GraphContainer.ClearData(); }
        };
        TreeView.AddHandler(PointerPressedEvent, (_,e) =>
        {
            if (e.GetCurrentPoint(TreeView).Properties.IsRightButtonPressed && e.Source is Avalonia.Visual visual)
            {
                while (visual != null && visual is not TreeNode) visual = visual.GetVisualParent();
                if (visual is TreeNode node) TreeView.SelectedNode = node;
            }
        }, Avalonia.Interactivity.RoutingStrategies.Tunnel);
        DataGridView.CellBeginEdit += (_,e) => { EditCell = DataGridView[e.ColumnIndex,e.RowIndex]; OldCellValue = EditCell.Value; };
        DataGridView.CellEndEdit += DataGridView_CellEndEdit;
        DataGridView.UserDeletingRow += DataGridView_UserDeletingRow;
    }
    public virtual void OpenFileDB_Click(object sender, RoutedEventArgs e) => LoadEvent?.Invoke();
    public virtual void AddDB_Click(object sender, RoutedEventArgs e) => LoadEvent?.Invoke();
    public async void SafeFileButton_Click(object sender, EventArgs e)
    {
        try
        {
            var file = await FileStorage.SaveFilePickerAsync(new FilePickerSaveOptions
            { DefaultExtension = "jsf", FileTypeChoices = [new FilePickerFileType("(*.jsf)") { Patterns = ["*.jsf"] }] });
            if (file?.TryGetLocalPath() is string path && path.Length > 0) SaveEvent?.Invoke(path);
        }
        catch (Exception ex)
        {
            // Ошибка записи базы (нет прав, файл занят и т.п.) не должна ронять приложение.
            await MessageBox.Show(this, ex.Message, "Ошибка");
        }
    }
    protected void BeginLabelEdit(TreeNode node)
    {
        if (!LabelEditFlag || node == null) return;
        var edit = new TextBox { Text = node.Text, Height = 20, MinHeight = 20, Padding = new Thickness(2,0) };
        node.Header = edit; var finished = false;
        void Finish(string label)
        {
            if (finished) return; finished = true;
            var e = new NodeLabelEditEventArgs(node,label); TreeView_AfterLabelEdit(TreeView,e);
            node.Text = e.CancelEdit || label == null ? node.Text : label;
        }
        edit.KeyDown += (_,e) => { if (e.Key == Key.Enter) Finish(edit.Text); if (e.Key == Key.Escape) Finish(null); };
        edit.LostFocus += (_,_) => Finish(edit.Text);
        Avalonia.Threading.Dispatcher.UIThread.Post(() => { edit.Focus(); edit.SelectAll(); });
    }
    protected void RemoveNode(TreeNode node)
    {
        if (node.ParentNode != null) node.ParentNode.Nodes.Remove(node);
        else TreeView.Nodes.Remove(node);
    }
    internal void RenameNode(TreeNode node, string label)
    {
        LabelEditFlag = true; var e = new NodeLabelEditEventArgs(node,label); TreeView_AfterLabelEdit(TreeView,e);
        if (!e.CancelEdit) node.Text = label;
    }
    protected void ShowAuxiliaryWindow(string name, string title, UserControl control, bool modal)
    {
        var window = new Window { Name = name, Title = title, Width = 500, Height = 500, Topmost = true,
            FontFamily = FontFamily, FontSize = FontSize, Content = control, WindowStartupLocation = WindowStartupLocation.CenterOwner };
        AuxiliaryWindows.Add(window); window.Closed += (_,_) => AuxiliaryWindows.Remove(window);
        if (TopLevel.GetTopLevel(this) is Window owner)
        { if (modal) _ = window.ShowDialog(owner); else window.Show(owner); }
        else window.Show();
    }
    public virtual void AddBranchButton_Click(object sender, EventArgs e) => throw new NotImplementedException();
    public virtual void DelBrachButton_Click(object sender, EventArgs e) => throw new NotImplementedException();
    public virtual void AddNewRowButton_Click(object sender, EventArgs e) => throw new NotImplementedException();
    public virtual void DelAllRowsButton_Click(object sender, EventArgs e) => throw new NotImplementedException();
    public virtual void Resort_Click(object sender, EventArgs e) => throw new NotImplementedException();
    public virtual void CreateCopy_Click(object sender, EventArgs e) => throw new NotImplementedException();
    public virtual void TreeView_AfterSelect(object sender, TreeViewEventArgs e) => throw new NotImplementedException();
    public virtual void TreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e) => throw new NotImplementedException();
    public virtual void DataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e) => throw new NotImplementedException();
    public virtual void DataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e) => NewCellValue = DataGridView[e.ColumnIndex,e.RowIndex].Value;
    public DataTable Resort(DataTable dt, string colName, string direction)
    { dt.DefaultView.Sort = colName + " " + direction; return dt.DefaultView.ToTable(); }
    public List<GraphData> SetGraphData(DataTable table, string header, Color color, string xUnit, string yUnit)
    {
        var range = new List<GraphData>();
        if (table.Columns[0].DataType != typeof(string))
            for (var i = 1; i < table.Columns.Count; i++)
            {
                var points = new List<GraphPoint>();
                // Пустая ячейка (DBNull) пропускается: Convert.ToSingle(DBNull) бросал исключение, и график не строился вовсе
                for (var j = 0; j < table.Rows.Count; j++)
                    if (table.Rows[j][0] is not DBNull && table.Rows[j][i] is not DBNull)
                        points.Add(new GraphPoint(Convert.ToSingle(table.Rows[j][0]), Convert.ToSingle(table.Rows[j][i])));
                if (points.Count != 0) range.Add(new GraphData($"{header}_{table.Columns[i].ColumnName}", color,xUnit,yUnit,points.ToArray()) { ValueFlag = true });
            }
        return range;
    }
    public string GetNextName(string basePrefix, IEnumerable<string> keys)
    {
        var numbers = keys.Where(k => k.StartsWith(basePrefix)).Select(k => int.TryParse(k.Substring(basePrefix.Length),out var n) ? (int?)n : null)
            .Where(n => n.HasValue).Select(n => n.Value).OrderBy(n => n).ToList();
        var next = 1; foreach (var n in numbers) { if (n == next) next++; else break; } return $"{basePrefix}{next}";
    }
}
