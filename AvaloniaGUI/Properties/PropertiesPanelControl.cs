using Avalonia;
using Avalonia.Automation;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using AvaloniaGUI.Localization;
using System;
using System.Collections.Generic;
using Color = System.Drawing.Color;

namespace AvaloniaGUI.Properties
{
    internal sealed partial class PropertiesPanelControl : UserControl
    {
        // Avalonia: перекрывает StyledElement.Resources, чтобы Resources.X ссылался на строковые ресурсы.
        private new class Resources : Localization.Resources { }

        public event Action<PropertyChangedEventArgs> PropertyUpdateEvent;
        public event Action<PropertyChangedEventArgs> ReDrawEvent;

        public delegate bool Validator(string header, string value, out string corrected);
        public event Validator ValidateValue;

        private string _oldValue;
        private bool _isValid;
        private string objInfo; // возможно костыль, хранит инфо об объекте сво-ва которого представлены
        private int tag; // возможно костыль, хранит инфо об источнике, где были получены сво-ва объекта

        // Avalonia: таблица строится из Grid, строки хранятся как аналог DataGridViewRowEx.
        private readonly List<DataGridViewRowEx> gridRows = new();

        public PropertiesPanelControl()
        {
            InitializeComponent();
        }

        public void ClearTable()
        {
            // Avalonia: строки очищаются до удаления редакторов — их LostFocus при отсоединении
            // не должен применять незавершённую правку к уже новой таблице (как Rows.Clear в DataGridView).
            gridRows.Clear();
            dataGridView1.Children.Clear();
            dataGridView1.RowDefinitions.Clear();
        }

        /// <summary>
        /// DrawTable
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="_objInfo">дополнительная информация об объекте</param>
        /// <param name="_tag">дополнительная информация</param>
        public void DrawTable(List<RowProperty> rows, string _objInfo = null, int _tag = 0)
        {
            objInfo = _objInfo;
            tag = _tag;
            ClearTable();
            // Тут при создании строки таблицы должно происходить автоопределение типа элемента ячейки
            // comboBox,TextBox, CheckBox etc.
            foreach (var prop in rows)// Инициализация строк через RowProperty
            {
                var row = new DataGridViewRowEx { Key = prop.Key, BackColor = prop.Color };

                row.HeaderValue = prop.LocalizedHeader; // Имя свойства

                if (prop.Value is bool chbv)
                {
                    row.CellKind = CellKind.CheckBox;
                    row.Value = chbv;
                }

                else if (prop.Value is DropDownPropertyValue ddpv)
                {
                    row.CellKind = CellKind.ComboBox;
                    row.Items = ddpv.AvailableValues.ToArray();
                    row.Value = ddpv.Value.ToString();
                }

                else if (prop.Value is NumericUpDownValue nudpv)
                {
                    row.CellKind = CellKind.NumericUpDown;
                    row.Numeric = nudpv;
                    row.Value = Convert.ToDecimal(nudpv.Value);
                }
                else if (prop.Value is ButtonPropertyValue bv)
                {
                    row.CellKind = CellKind.Button;
                    row.StyleTag = bv;
                    row.Value = bv.Text;
                }

                else
                {
                    row.CellKind = CellKind.TextBox;
                    row.Value = prop.Value.ToString();
                }

                if (prop.LocalizedHeader == Resources.Header_Color)
                    row.CellBackColor = (Color)prop.Value;

                row.Tag = prop.ValidationType.ToString();

                row.ReadOnly = prop.IsReadOnly;
                AddRow(row);
            }
            if (gridRows.Count > 0) SelectRow(0);
        }

        private void DataGridView1_CellBeginEdit(int rowIndex)
        {
            if (gridRows[rowIndex].Value != null)
                _oldValue = gridRows[rowIndex].Value.ToString();
        }

        public void CellValueChanged(int rowIndex, string key)
        {
            // TODO: получение ключа из DGVCellEx
            var row = gridRows[rowIndex];
            var header = row.HeaderValue;
            var cellValue = row.Value;
            var newValue = cellValue?.ToString() ?? string.Empty;

            if (header == Resources.Header_Color)
            {
                var color = ChangeColorCell(newValue);
                row.CellBackColor = color;
                UpdateCellBackground(row);
            }
            var eventArgs = new PropertyChangedEventArgs(key, header, newValue, _oldValue);

            if (objInfo != null)
                eventArgs.ObjInfo = objInfo;

            eventArgs.Tag = tag;

            PropertyUpdateEvent?.Invoke(eventArgs);
        }

        private async void DataGridView1_CellClick(int rowIndex)
        {
            try
            {
                var row = gridRows[rowIndex];
                var value = "";
                if (row.HeaderValue == Resources.Header_Color)
                {
                    if (TopLevel.GetTopLevel(this) is Window owner)
                    {
                        var current = row.CellBackColor ?? Color.White;
                        var color = await new ColorSelectionDialog(current).ShowDialog<Color?>(owner);
                        if (color != null)
                            value = color.Value.ToString();
                    }
                }

                else if (row.HeaderValue == Resources.Header_File)
                {
                    var top = TopLevel.GetTopLevel(this);
                    if (top != null)
                    {
                        var files = await top.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions { AllowMultiple = false });
                        if (files.Count != 0)
                            value = files[0].TryGetLocalPath() ?? "";
                    }
                }

                if (value != "" && IsCurrentRow(row, rowIndex))
                {
                    DataGridView1_CellBeginEdit(rowIndex);
                    SetCellValue(rowIndex, value);
                    SelectRow(rowIndex);
                }

                if (row.CellKind == CellKind.Button)
                {
                    var buttonSet = row.StyleTag as ButtonPropertyValue;

                    if (buttonSet != null)
                        buttonSet.OnClick?.Invoke();
                }
            }
            catch (Exception ex)
            {
                // async void: исключение действия кнопки/диалога не должно ронять приложение.
                await MessageBox.Show(this, ex.Message, "Ошибка");
            }
        }

        private void dataGridView1_CellValueChanged(int rowIndex)
        {
            var row = gridRows[rowIndex];
            if (row.Tag != ValidationType.None.ToString())
            {
                var cellValue = row.Value;
                var newValue = cellValue?.ToString() ?? string.Empty;
                var tag = row.Tag;
                var corrected = newValue;
                _isValid = ValidateValue?.Invoke(tag, newValue, out corrected) ?? true;

                if (!_isValid)
                {
                    SetCellValue(rowIndex, _oldValue, false);
                    return;
                }
                if (newValue != corrected) SetCellValue(rowIndex, corrected);
            }
            CellValueChanged(rowIndex, row.Key);
        }

        private Color ChangeColorCell(string colorName)
        {
            Color color;
            if (colorName.StartsWith("Color [A="))
            {
                string[] parts = colorName.Trim('C', 'o', 'l', 'r', ' ', '[', ']').Split(',');
                int a = int.Parse(parts[0].Split('=')[1]);
                int r = int.Parse(parts[1].Split('=')[1]);
                int g = int.Parse(parts[2].Split('=')[1]);
                int b = int.Parse(parts[3].Split('=')[1]);
                color = Color.FromArgb(a, r, g, b);
            }
            else
            {
                color = Color.FromName(colorName.Replace("Color [", "").Replace("]", ""));
            }
            return color;
        }

        // ---- Avalonia: построение ячеек вместо DataGridViewCell ----

        private enum CellKind { TextBox, CheckBox, ComboBox, NumericUpDown, Button }

        private sealed class DataGridViewRowEx
        {
            public string Key;
            public Color BackColor;
            public string HeaderValue;
            public CellKind CellKind;
            public object Value;
            public string[] Items;
            public NumericUpDownValue Numeric;
            public object StyleTag;
            public Color? CellBackColor;
            public string Tag;
            public bool ReadOnly;
            public Border HeaderCell;
            public Border ValueCell;
            public Action<object> Refresh;
        }

        /// <summary>
        /// Аналог присваивания DataGridViewCell.Value: обновляет редактор и вызывает CellValueChanged.
        /// </summary>
        private void SetCellValue(int rowIndex, object value, bool raise = true)
        {
            var row = gridRows[rowIndex];
            row.Value = value;
            row.Refresh?.Invoke(value);
            if (raise) dataGridView1_CellValueChanged(rowIndex);
        }

        private void AddRow(DataGridViewRowEx row)
        {
            var index = gridRows.Count;
            gridRows.Add(row);
            dataGridView1.RowDefinitions.Add(new RowDefinition(new GridLength(25)));

            var header = new TextBlock
            {
                Text = row.HeaderValue,
                Margin = new Thickness(17, 0, 3, 0),
                VerticalAlignment = VerticalAlignment.Center,
                TextTrimming = TextTrimming.CharacterEllipsis
            };
            AutomationProperties.SetName(header, row.HeaderValue);
            row.HeaderCell = AddCell(header, index, 0, row.BackColor);

            var editor = CreateEditor(row, index);
            AutomationProperties.SetName(editor, row.HeaderValue);
            row.ValueCell = AddCell(editor, index, 1, row.BackColor);
            UpdateCellBackground(row);
        }

        private Border AddCell(Control content, int rowIndex, int column, Color background)
        {
            var cell = new Border
            {
                Background = ColorBrush(background),
                BorderBrush = Brush.Parse("#8A8A8A"),
                BorderThickness = new Thickness(0, 0, 1, 1),
                Child = content,
                MinWidth = 0
            };
            cell.PointerPressed += (_, _) => SelectRow(rowIndex);
            Grid.SetRow(cell, rowIndex);
            Grid.SetColumn(cell, column);
            dataGridView1.Children.Add(cell);
            return cell;
        }

        /// <summary>Строка ещё показана в таблице (таблица не была перерисована во время правки).</summary>
        private bool IsCurrentRow(DataGridViewRowEx row, int rowIndex) =>
            rowIndex < gridRows.Count && ReferenceEquals(gridRows[rowIndex], row);

        private void UpdateCellBackground(DataGridViewRowEx row)
        {
            if (row.ValueCell != null && row.CellBackColor.HasValue)
                row.ValueCell.Background = ColorBrush(row.CellBackColor.Value);
        }

        private void SelectRow(int rowIndex)
        {
            for (var index = 0; index < gridRows.Count; index++)
            {
                var row = gridRows[index];
                row.HeaderCell.Background = index == rowIndex ? Brush.Parse("#A0A0A0") : ColorBrush(row.BackColor);
                if (row.HeaderCell.Child is TextBlock label)
                    label.Foreground = index == rowIndex ? Brushes.White : Brushes.Black;
            }
        }

        private Control CreateEditor(DataGridViewRowEx row, int rowIndex)
        {
            if (row.HeaderValue == Resources.Header_Color || row.HeaderValue == Resources.Header_File)
                return CreateDialogCell(row, rowIndex);
            return row.CellKind switch
            {
                CellKind.CheckBox => CreateCheckCell(row, rowIndex),
                CellKind.ComboBox => CreateChoiceCell(row, rowIndex),
                CellKind.NumericUpDown => CreateNumericCell(row, rowIndex),
                CellKind.Button => CreateButtonCell(row, rowIndex),
                _ => CreateTextCell(row, rowIndex)
            };
        }

        private Control CreateDialogCell(DataGridViewRowEx row, int rowIndex)
        {
            var text = new TextBlock
            {
                Text = row.Value?.ToString(),
                Margin = new Thickness(3, 0),
                VerticalAlignment = VerticalAlignment.Center,
                TextTrimming = TextTrimming.CharacterEllipsis
            };
            var host = new Border { Child = text, Background = Brushes.Transparent, Cursor = new Cursor(StandardCursorType.Hand) };
            row.Refresh = value => text.Text = value?.ToString();
            host.PointerReleased += (_, _) => DataGridView1_CellClick(rowIndex);
            return host;
        }

        private Control CreateButtonCell(DataGridViewRowEx row, int rowIndex)
        {
            var button = new Button
            {
                Content = row.Value?.ToString(),
                Padding = new Thickness(4, 1),
                IsEnabled = !row.ReadOnly,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Left
            };
            button.Classes.Add("property-editor");
            button.Click += (_, _) => DataGridView1_CellClick(rowIndex);
            return button;
        }

        private Control CreateNumericCell(DataGridViewRowEx row, int rowIndex)
        {
            var numeric = new NumericUpDown
            {
                Value = (decimal)row.Value,
                Minimum = row.Numeric.Minimum,
                Maximum = row.Numeric.Maximum,
                Increment = row.Numeric.Increment,
                FormatString = row.Numeric.DecimalPlaces == 0 ? "0" : "F" + row.Numeric.DecimalPlaces,
                IsReadOnly = row.ReadOnly,
                MinHeight = 20
            };
            numeric.Classes.Add("property-editor");
            row.Refresh = value => numeric.Value = Convert.ToDecimal(value);
            // Avalonia: NumericUpDown меняет Value на каждый символ; как DataGridViewNumericUpDownCell,
            // значение фиксируется только по окончании правки (Enter или уход фокуса).
            void EndEdit()
            {
                if (!IsCurrentRow(row, rowIndex)) return;
                if (numeric.Value == null)
                {
                    numeric.Value = Convert.ToDecimal(row.Value);
                    return;
                }
                if (numeric.Value == Convert.ToDecimal(row.Value)) return;
                DataGridView1_CellBeginEdit(rowIndex);
                row.Value = numeric.Value.Value;
                dataGridView1_CellValueChanged(rowIndex);
            }
            numeric.AddHandler(KeyDownEvent, (_, e) =>
            {
                if (e.Key == Key.Enter) EndEdit();
            }, RoutingStrategies.Bubble, true);
            numeric.LostFocus += (_, _) => EndEdit();
            return numeric;
        }

        private Control CreateCheckCell(DataGridViewRowEx row, int rowIndex)
        {
            var check = new CheckBox
            {
                IsChecked = (bool)row.Value,
                IsEnabled = !row.ReadOnly,
                Width = 16, Height = 16, MinWidth = 16, MinHeight = 16,
                MaxWidth = 16, MaxHeight = 16, Padding = new Thickness(0),
                VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(4, 0)
            };
            check.Classes.Add("property-editor");
            check.Template = new FuncControlTemplate<CheckBox>((control, _) =>
            {
                var mark = new TextBlock
                {
                    Text = "✓", FontFamily = new FontFamily("Segoe UI Symbol"), FontSize = 12,
                    FontWeight = FontWeight.Bold, Foreground = Brushes.Black,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                var square = new Border { Width = 16, Height = 16, CornerRadius = new CornerRadius(0), Child = mark };
                var hover = false;
                var pressed = false;
                void Update()
                {
                    square.Background = Brush.Parse(!control.IsEnabled ? "#F8F8F8" : pressed ? "#E5E5E5" : "#FFFFFF");
                    square.BorderBrush = Brush.Parse(control.IsEnabled ? "#000000" : "#CACACA");
                    square.BorderThickness = new Thickness(!control.IsEnabled ? 1 : pressed ? 3 : hover ? 2 : 1);
                    mark.IsVisible = control.IsChecked == true;
                    mark.Opacity = control.IsEnabled ? 1 : 0.5;
                }
                control.IsCheckedChanged += (_, _) => Update();
                control.PointerEntered += (_, _) => { hover = true; Update(); };
                control.PointerExited += (_, _) => { hover = false; pressed = false; Update(); };
                control.PointerPressed += (_, _) => { pressed = true; Update(); };
                control.PointerReleased += (_, _) => { pressed = false; Update(); };
                control.PropertyChanged += (_, change) =>
                {
                    if (change.Property == InputElement.IsEnabledProperty) Update();
                };
                Update();
                return square;
            });
            var updating = false;
            row.Refresh = value => { updating = true; check.IsChecked = (bool)value; updating = false; };
            check.IsCheckedChanged += (_, _) =>
            {
                if (updating) return;
                DataGridView1_CellBeginEdit(rowIndex);
                row.Value = check.IsChecked == true;
                dataGridView1_CellValueChanged(rowIndex);
            };
            return check;
        }

        private Control CreateChoiceCell(DataGridViewRowEx row, int rowIndex)
        {
            var combo = new ComboBox
            {
                ItemsSource = row.Items,
                SelectedItem = row.Value?.ToString(),
                IsEnabled = !row.ReadOnly,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Stretch,
                Background = Brushes.White, MinHeight = 20, Padding = new Thickness(3, 0)
            };
            combo.Classes.Add("property-editor");
            combo.ContainerPrepared += (_, args) =>
            {
                if (args.Container is not ComboBoxItem item) return;
                item.Height = 20;
                item.Padding = new Thickness(0);
                item.Template = new FuncControlTemplate<ComboBoxItem>((control, _) =>
                {
                    var content = new Border
                    {
                        CornerRadius = new CornerRadius(0),
                        Child = new TextBlock
                        {
                            Text = control.Content?.ToString(),
                            Margin = new Thickness(3, 0),
                            VerticalAlignment = VerticalAlignment.Center
                        }
                    };
                    var hover = false;
                    var pressed = false;
                    void Update() => content.Background = Brush.Parse(!control.IsEnabled ? "#F8F8F8" :
                        pressed ? "#D5D5D5" : hover ? "#E5E5E5" : "#FFFFFF");
                    control.PointerEntered += (_, _) => { hover = true; Update(); };
                    control.PointerExited += (_, _) => { hover = false; pressed = false; Update(); };
                    control.PointerPressed += (_, _) => { pressed = true; Update(); };
                    control.PointerReleased += (_, _) => { pressed = false; Update(); };
                    Update();
                    return content;
                });
            };
            var updating = false;
            row.Refresh = value => { updating = true; combo.SelectedItem = value?.ToString(); updating = false; };
            combo.SelectionChanged += (_, _) =>
            {
                if (updating || combo.SelectedItem is not string selected || selected == row.Value?.ToString()) return;
                DataGridView1_CellBeginEdit(rowIndex);
                row.Value = selected;
                dataGridView1_CellValueChanged(rowIndex);
            };
            return combo;
        }

        private Control CreateTextCell(DataGridViewRowEx row, int rowIndex)
        {
            var preview = new TextBlock
            {
                Text = row.Value?.ToString(), Margin = new Thickness(3, 0),
                VerticalAlignment = VerticalAlignment.Center,
                TextTrimming = TextTrimming.CharacterEllipsis
            };
            var host = new Border { Child = preview, Background = Brushes.Transparent };
            row.Refresh = value => preview.Text = value?.ToString();
            if (row.ReadOnly) return host;

            TextBox box = null;
            string editOldValue = null;
            void Finish(bool save)
            {
                if (box == null) return;
                var text = box.Text ?? string.Empty;
                box = null;
                host.Child = preview;
                // Avalonia: правка, завершённая уходом фокуса после перерисовки таблицы, к новым строкам не относится.
                if (save && IsCurrentRow(row, rowIndex) && text != row.Value?.ToString())
                {
                    // Avalonia: клик по другой строке успевает начать её правку до LostFocus этой —
                    // восстанавливаем старое значение именно этой ячейки.
                    _oldValue = editOldValue;
                    preview.Text = text;
                    row.Value = text;
                    dataGridView1_CellValueChanged(rowIndex);
                }
            }
            host.PointerPressed += (_, _) =>
            {
                if (box != null) return;
                DataGridView1_CellBeginEdit(rowIndex);
                editOldValue = _oldValue;
                box = new TextBox
                {
                    Text = row.Value?.ToString(), Height = 22, Padding = new Thickness(3, 0),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    FocusAdorner = null
                };
                box.Classes.Add("property-editor");
                box.KeyDown += (_, e) =>
                {
                    if (e.Key == Key.Enter) { Finish(true); e.Handled = true; }
                    else if (e.Key == Key.Escape) { Finish(false); e.Handled = true; }
                };
                box.LostFocus += (_, _) => Finish(true);
                host.Child = box;
                // Как EditOnEnter в DataGridView: текст ячейки выделяется целиком и заменяется вводом.
                Dispatcher.UIThread.Post(() => { box?.Focus(); box?.SelectAll(); });
            };
            return host;
        }

        internal static IBrush ColorBrush(Color color) => new SolidColorBrush(
            Avalonia.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
    }
}
