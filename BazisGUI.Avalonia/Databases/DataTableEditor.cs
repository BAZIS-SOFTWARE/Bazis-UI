using Avalonia;
using global::Avalonia.Controls;
using global::Avalonia.Input;
using global::Avalonia.Layout;
using global::Avalonia.Media;
using global::Avalonia.Controls.Shapes;
using global::Avalonia.Threading;
using System;
using System.Data;
using System.Globalization;
using System.Collections.Generic;

namespace BazisGUI.Avalonia.Databases;

internal sealed class DataGridViewCellEventArgs(int column, int row) : EventArgs
{
    public int ColumnIndex { get; } = column;
    public int RowIndex { get; } = row;
}
internal sealed class TableCell(DataTableEditor editor, int row, int column)
{
    public object Value { get => editor.DataSource.Rows[row][column]; set => editor.DataSource.Rows[row][column] = value; }
    public DataColumn OwningColumn => editor.DataSource.Columns[column];
}
internal sealed class TableRow(DataTableEditor editor, int row)
{
    public DataTableEditor DataGridView => editor;
    public TableCell[] Cells { get; } = GetCells(editor, row);
    private static TableCell[] GetCells(DataTableEditor editor, int row)
    {
        var cells = new TableCell[editor.DataSource.Columns.Count];
        for (var i = 0; i < cells.Length; i++) cells[i] = new TableCell(editor, row, i);
        return cells;
    }
}
internal sealed class DataGridViewRowCancelEventArgs(TableRow row) : EventArgs
{
    public TableRow Row { get; } = row;
    public bool Cancel { get; set; }
}

// Разбор чисел, введённых пользователем. Культура приложения — Invariant, а Convert.ChangeType/float.Parse
// разрешают разделитель тысяч: "1,5" молча превращалось в 15. Запятая принимается как десятичный разделитель.
internal static class DatabaseNumber
{
    public static float ParseFloat(string text) => (float)Parse(text, typeof(float));
    public static object Parse(string text, Type type)
    {
        if (type != typeof(float) && type != typeof(double) && type != typeof(decimal))
            return Convert.ChangeType(text, type, CultureInfo.CurrentCulture);
        var normalized = text?.Trim().Replace(',', '.');
        if (!double.TryParse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture, out var value) || !double.IsFinite(value)
            || type == typeof(float) && !float.IsFinite((float)value))
            throw new FormatException($"\"{text}\": неправильный формат числа");
        return type == typeof(decimal) ? decimal.Parse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture)
            : type == typeof(float) ? (object)float.Parse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture) : value;
    }
}

// DataTable remains the single editable source, including structure-driven column changes.
internal sealed class DataTableEditor : UserControl
{
    private DataTable table;
    private bool refreshPending;
    private bool committing;
    private readonly ScrollViewer scroll = new() { HorizontalScrollBarVisibility = global::Avalonia.Controls.Primitives.ScrollBarVisibility.Auto };
    public event EventHandler<DataGridViewCellEventArgs> CellBeginEdit;
    public event EventHandler<DataGridViewCellEventArgs> CellEndEdit;
    public event EventHandler<DataGridViewRowCancelEventArgs> UserDeletingRow;
    public event Action<int> ColumnHeaderClick;
    public int SelectedColumn { get; set; } = -1;
    public int SelectedRow { get; private set; } = -1;
    private readonly List<(TextBox editor, int row, int column)> cells = new();
    private readonly List<global::Avalonia.Controls.Shapes.Path> rowIndicators = new();
    private readonly List<DataColumn> displayColumns = new();
    public TableCell this[int column, int row] => new(this, row, column);
    // Фиксация ячейки, находящейся в фокусе: вызывается до смены источника, чтобы правка
    // не потерялась и не записалась (по LostFocus) в уже другую таблицу
    private Action focusedCommit;
    private bool subscribed, attached;
    public DataTable DataSource
    {
        get => table;
        set
        {
            var commit = focusedCommit; focusedCommit = null;
            commit?.Invoke();
            Unsubscribe();
            table = value;
            displayColumns.Clear();
            SelectedRow = table?.Rows.Count > 0 ? 0 : -1;
            SelectedColumn = table?.Columns.Count > 0 ? 0 : -1;
            if (attached) Subscribe();
            Refresh();
        }
    }
    // Подписка на события таблицы только пока редактор в визуальном дереве: таблицы принадлежат базе
    // проекта и живут дольше страницы, иначе закрытая вкладка/окно реакции удерживались бы таблицей
    private void Subscribe()
    {
        if (subscribed || table == null) return;
        table.RowChanged += OnChanged; table.RowDeleted += OnChanged; table.TableCleared += OnCleared; table.Columns.CollectionChanged += OnColumnsChanged;
        subscribed = true;
    }
    private void Unsubscribe()
    {
        if (!subscribed || table == null) { subscribed = false; return; }
        table.RowChanged -= OnChanged; table.RowDeleted -= OnChanged; table.TableCleared -= OnCleared; table.Columns.CollectionChanged -= OnColumnsChanged;
        subscribed = false;
    }
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        attached = true; Subscribe(); Refresh();
    }
    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        attached = false; Unsubscribe();
    }
    public DataTableEditor()
    {
        Content = scroll; scroll.Classes.Add("database-table-scroll");
        SizeChanged += (_, _) => { if (scroll.Content is Grid grid) grid.MinWidth = Math.Max(0, Bounds.Width - 16); };
        AddHandler(KeyDownEvent, (_, e) =>
        {
            // A selected cell or row header deletes its row; an active text edit keeps normal Delete behavior.
            if (e.Key == Key.Delete && SelectedRow >= 0 && (e.Source is Button || e.Source is TextBox { IsReadOnly: true }))
            { DeleteRow(SelectedRow); e.Handled = true; }
        }, global::Avalonia.Interactivity.RoutingStrategies.Tunnel);
    }
    private void OnChanged(object sender, DataRowChangeEventArgs e) => ScheduleRefresh();
    // DataTable.Clear() не поднимает RowDeleted: без этого после "Очистить" на экране оставались
    // удалённые строки, а Delete по ним бросал IndexOutOfRange
    private void OnCleared(object sender, DataTableClearEventArgs e) => ScheduleRefresh();
    private void OnColumnsChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e) => ScheduleRefresh();
    private void ScheduleRefresh()
    {
        if (refreshPending) return;
        refreshPending = true;
        Dispatcher.UIThread.Post(() => { refreshPending = false; Refresh(); });
    }
    public void EditValue(int row, int column, object value)
    {
        var e = new DataGridViewCellEventArgs(column, row);
        CellBeginEdit?.Invoke(this, e);
        table.Rows[row][column] = value;
        CellEndEdit?.Invoke(this, e);
        Refresh();
    }
    public void DeleteRow(int row)
    {
        if (table == null || row < 0 || row >= table.Rows.Count) return;
        var e = new DataGridViewRowCancelEventArgs(new TableRow(this, row));
        UserDeletingRow?.Invoke(this, e);
        if (!e.Cancel) table.Rows.RemoveAt(row);
        SelectedRow = Math.Min(SelectedRow, table.Rows.Count - 1);
        Refresh();
    }
    public void Refresh()
    {
        var grid = new Grid { MinWidth = Math.Max(0, Bounds.Width - 16) };
        cells.Clear(); rowIndicators.Clear();
        scroll.Content = grid;
        if (table == null) return;
        displayColumns.RemoveAll(column => column.Table != table);
        foreach (DataColumn column in table.Columns) if (!displayColumns.Contains(column)) displayColumns.Add(column);
        grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(22)));
        for (var col = 0; col < table.Columns.Count; col++) grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star) { MinWidth = 60 });
        for (var row = 0; row <= table.Rows.Count; row++) grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        var headers = new List<(DataColumn column, Button button)>();
        for (var col = 0; col < displayColumns.Count; col++)
        {
            var column = displayColumns[col];
            var index = column.Ordinal;
            var header = new Button { Name = "ColumnHeader", Content = column.ColumnName, Padding = new Thickness(2, 0), Height = 20, MinHeight = 20,
                HorizontalAlignment = HorizontalAlignment.Stretch, HorizontalContentAlignment = HorizontalAlignment.Left,
                Background = Brushes.White };
            headers.Add((column, header));
            Point? dragStart = null;
            header.AddHandler(PointerPressedEvent, (_, e) => { if (e.GetCurrentPoint(header).Properties.IsLeftButtonPressed) dragStart = e.GetPosition(this); }, global::Avalonia.Interactivity.RoutingStrategies.Tunnel);
            header.AddHandler(PointerReleasedEvent, (_, e) =>
            {
                var position = e.GetPosition(this);
                if (!dragStart.HasValue || Math.Abs(position.X - dragStart.Value.X) < 4) { dragStart = null; return; }
                dragStart = null;
                foreach (var (targetColumn, target) in headers)
                {
                    var origin = target.TranslatePoint(new Point(), this);
                    if (!origin.HasValue || !new Rect(origin.Value, target.Bounds.Size).Contains(position)) continue;
                    var oldIndex = displayColumns.IndexOf(column); var newIndex = displayColumns.IndexOf(targetColumn);
                    displayColumns.RemoveAt(oldIndex); displayColumns.Insert(newIndex, column);
                    e.Pointer.Capture(null); e.Handled = true; Refresh(); break;
                }
            }, global::Avalonia.Interactivity.RoutingStrategies.Tunnel);
            header.Click += (_, _) => { SelectedColumn = index; ColumnHeaderClick?.Invoke(index); Refresh(); };
            Grid.SetColumn(header, col + 1); grid.Children.Add(header);
        }
        for (var row = 0; row < table.Rows.Count; row++)
        {
            var rowIndex = row;
            var indicator = new global::Avalonia.Controls.Shapes.Path { Data = global::Avalonia.Media.Geometry.Parse("M0,0 L5,5 L0,10 Z"), Fill = Brushes.Black, Width = 5, Height = 10 };
            rowIndicators.Add(indicator);
            var rowHeader = new Button { Content = indicator, Padding = new Thickness(2, 0), Name = "RowHeader", Height = 20, MinHeight = 20, MinWidth = 0 };
            rowHeader.Click += (_, _) => { SelectedRow = rowIndex; UpdateSelection(); };
            Grid.SetRow(rowHeader, row + 1); grid.Children.Add(rowHeader);
            for (var col = 0; col < displayColumns.Count; col++)
            {
                var columnIndex = displayColumns[col].Ordinal;
                var editor = new TextBox { Text = Convert.ToString(table.Rows[row][columnIndex], CultureInfo.CurrentCulture), Height = 20, MinHeight = 20, Padding = new Thickness(2, 0), FontSize = 11, IsReadOnly = true };
                editor.Classes.Add("database-cell");
                cells.Add((editor, rowIndex, columnIndex));
                var original = editor.Text;
                var source = table;
                void Commit()
                {
                    // source: LostFocus может прийти уже после смены DataSource — тогда rowIndex/columnIndex
                    // относятся к другой таблице, и значение записалось бы не туда (правку фиксирует сеттер DataSource)
                    if (committing || editor.Text == original || table != source || rowIndex >= table.Rows.Count) return;
                    try
                    {
                        committing = true;
                        var value = string.IsNullOrEmpty(editor.Text) && table.Columns[columnIndex].DataType != typeof(string)
                            ? DBNull.Value : DatabaseNumber.Parse(editor.Text, table.Columns[columnIndex].DataType);
                        EditValue(rowIndex, columnIndex, value);
                        original = editor.Text;
                    }
                    catch (Exception ex)
                    {
                        editor.Text = original = Convert.ToString(table.Rows[rowIndex][columnIndex], CultureInfo.CurrentCulture);
                        _ = MessageBox.Show(this, ex.Message);
                    }
                    finally { committing = false; }
                }
                editor.GotFocus += (_, _) => { focusedCommit = Commit; SelectedRow = rowIndex; SelectedColumn = columnIndex; UpdateSelection(); };
                editor.DoubleTapped += (_, _) => { editor.IsReadOnly = false; editor.SelectAll(); };
                editor.AddHandler(TextInputEvent, (_, _) => { if (editor.IsReadOnly) { editor.IsReadOnly = false; editor.SelectAll(); } }, global::Avalonia.Interactivity.RoutingStrategies.Tunnel);
                editor.LostFocus += (_, _) => { Commit(); editor.IsReadOnly = true; };
                editor.KeyDown += (_, e) =>
                {
                    if (e.Key == Key.Enter) { Commit(); editor.IsReadOnly = true; e.Handled = true; }
                    if (e.Key == Key.F2) { editor.IsReadOnly = false; editor.SelectAll(); e.Handled = true; }
                    if (e.Key == Key.Escape) { editor.Text = original; editor.IsReadOnly = true; e.Handled = true; }
                };
                Grid.SetRow(editor, row + 1); Grid.SetColumn(editor, col + 1); grid.Children.Add(editor);
            }
        }
        UpdateSelection();
    }
    private void UpdateSelection()
    {
        for (var i = 0; i < rowIndicators.Count; i++) rowIndicators[i].IsVisible = i == SelectedRow;
        foreach (var (editor, row, column) in cells)
        {
            var selected = row == SelectedRow && column == SelectedColumn;
            editor.Background = selected ? Brush.Parse("#0078D7") : Brushes.White;
            editor.Foreground = selected ? Brushes.White : Brushes.Black;
        }
    }
}
