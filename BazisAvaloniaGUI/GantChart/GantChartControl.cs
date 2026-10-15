using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using BazisAvaloniaGUI.Localization;
using Project.Interfaces.Tasks;
using Project.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;

namespace BazisAvaloniaGUI.GantChart
{
    /// <summary>
    /// Avalonia-аналог cntrГант. DataGridView с отрисовкой ячейки в CellPainting заменён сеткой строк:
    /// первая колонка — текст условия, вторая — <see cref="ConditionBar"/> с той же отрисовкой полосы.
    /// </summary>
    internal sealed class cntrГант : UserControl
    {
        // Avalonia: перекрывает StyledElement.Resources, чтобы Resources.X ссылался на строковые ресурсы.
        private new class Resources : Localization.Resources { }

        // $this.Size из GantChartControl.resx: в BaseForm AddConds вызывается до размещения контрола,
        // поэтому Width в расчёте koeff равен ширине из дизайнера.
        private const double DesignWidth = 385;
        private const double RowHeight = 22;

        private readonly ResourceManager resources = new(typeof(cntrГант));
        private readonly List<(string Name, ICondData Cond)> rows = new();
        private readonly Grid grid = new();
        private string sortedColumn;
        private bool sortDescending;

        float koeff;

        public cntrГант()
        {
            Background = Brushes.White;
            Content = new ScrollViewer
            {
                HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Disabled,
                Content = grid
            };
            Present();
        }

        public void AddConds(IEnumerable<ICondData> conds)
        {
            var conditions = conds.ToList();
            rows.Clear();
            if (conditions.Count == 0)
            {
                koeff = 0;
                Present();
                return;
            }
            foreach (var item in conditions)
            {
                string kind;
                var itemParts = item.ToString().Split(" : ");

                switch (itemParts[0])
                {
                    case "Нагрев":
                        kind = Resources.cntrГант_AddConds_Нагрев;
                        break;

                    case "Материал":
                        kind = Resources.cntrГант_AddConds_Материал;
                        break;

                    case "Среда":
                        kind = Resources.cntrГант_AddConds_Среда;
                        break;

                    case "Закрепление":
                        kind = Resources.cntrГант_AddConds_Закрепление;
                        break;

                    case "Нагрузка":
                        kind = Resources.cntrГант_AddConds_Нагрузка;
                        break;

                    default: throw new ArgumentException(Resources.cntrГант_AddConds_UndefinedCondExc);
                }

                rows.Add(($"{kind} : {itemParts[1]}", item));
            }
            var max = conditions.Max(x => x.StopTime);
            var min = conditions.Min(x => x.StartTime);

            koeff = (float)((max - min) / (Bounds.Width > 0 ? Bounds.Width : DesignWidth));
            Present();
        }

        private void Present()
        {
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions = new ColumnDefinitions("22,78,*");

            grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            AddHeader("CondName", 1);
            AddHeader("CondTime", 2);

            for (var i = 0; i < rows.Count; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition(new GridLength(RowHeight)));
                AddCell(new Border(), i + 1, 0);
                AddCell(new TextBlock { Text = rows[i].Name, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(2, 0), TextTrimming = TextTrimming.CharacterEllipsis }, i + 1, 1);
                AddCell(new ConditionBar(rows[i].Cond, () => koeff), i + 1, 2);
            }
        }

        private void AddHeader(string columnName, int column)
        {
            var header = new Button
            {
                Name = columnName,
                Content = resources.GetString(columnName + ".HeaderText"),
                Height = 20,
                MinHeight = 20,
                Padding = new Thickness(2, 0),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Background = Brushes.White
            };
            header.Click += (_, _) => Sort(columnName);
            Grid.SetColumn(header, column);
            grid.Children.Add(header);
        }

        private void AddCell(Control cell, int row, int column)
        {
            var border = new Border
            {
                BorderBrush = Brush.Parse("#A0A0A0"),
                BorderThickness = new Thickness(0, 0, 1, 1),
                Child = cell
            };
            Grid.SetRow(border, row);
            Grid.SetColumn(border, column);
            grid.Children.Add(border);
        }

        /// <summary>Аналог автоматической сортировки DataGridView по щелчку на заголовке колонки.</summary>
        private void Sort(string columnName)
        {
            sortDescending = sortedColumn == columnName && !sortDescending;
            sortedColumn = columnName;

            Comparison<(string Name, ICondData Cond)> comparison = columnName == "CondTime"
                ? (row1, row2) => dataGridView_SortCompare(row1.Cond, row2.Cond)
                : (row1, row2) => string.Compare(row1.Name, row2.Name, StringComparison.CurrentCulture);
            rows.Sort(sortDescending ? (row1, row2) => comparison(row2, row1) : comparison);
            Present();
        }

        private int dataGridView_SortCompare(ICondData cellValue1, ICondData cellValue2)
        {
            var value1 = cellValue1?.ToString().Split(" : ")[1].Split(' ')[5];
            var value2 = cellValue2?.ToString().Split(" : ")[1].Split(' ')[5];

            if (float.TryParse(value1, out float floatValue1) && float.TryParse(value2, out float floatValue2))
                return floatValue1.CompareTo(floatValue2);

            return string.Compare(cellValue1?.ToString(), cellValue2?.ToString(), StringComparison.CurrentCulture);
        }

        /// <summary>Ячейка колонки CondTime: отрисовка из dataGridView_CellPainting.</summary>
        private sealed class ConditionBar(ICondData currentCond, Func<float> koeff) : Control
        {
            public override void Render(DrawingContext context)
            {
                var cellBounds = new Rect(Bounds.Size);
                int barStartPixel = (int)(currentCond.StartTime / koeff());
                int barWidthPixel = (int)(currentCond.StopTime / koeff());

                context.FillRectangle(Brushes.White, cellBounds);

                IBrush color;

                if (currentCond.Kind == DataKind.Закрепление)
                    color = Brushes.Black;
                else if (currentCond.Kind == DataKind.Материал)
                    color = Brushes.Green;
                else if (currentCond.Kind == DataKind.Среда)
                    color = Brushes.Yellow;
                else if (currentCond.Kind == DataKind.Нагрев)
                    color = Brushes.Red;
                else
                    color = Brushes.Blue;

                var barWidth = Math.Max(0, barWidthPixel - barStartPixel);
                context.FillRectangle(color, new Rect(barStartPixel, 5, barWidth, Math.Max(0, cellBounds.Height - 10)));
            }
        }
    }
}
