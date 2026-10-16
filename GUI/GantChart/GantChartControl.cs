using BazisGUI.Localization;
using BazisGUI.Properties;
using Project.Interfaces.Tasks;
using Project.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserControlsEx;

namespace BazisGUI.GantChart
{
    public partial class cntrГант : UserControl//, ILocalizableHeaderControl
    {
        float koeff;

        public cntrГант()
        {
            InitializeComponent();
        }

        public void AddConds(IEnumerable<ICondData> conds)
        {
            foreach (var item in conds)
            {
                //1. Создаём и добавляем колонки
                //2. Добавляем строку
                int rowNumber = dataGridView.Rows.Add();
                //3. Заполняем ячейки

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

                    default:  throw new ArgumentException(Resources.cntrГант_AddConds_UndefinedCondExc);
                }

                dataGridView.Rows[rowNumber].Cells[0].Value = $"{kind} : {itemParts[1]}";
                dataGridView.Rows[rowNumber].Cells[1].Value = item;
            }
            var max = conds.Max(x => x.StopTime);
            var min = conds.Min(x => x.StartTime);

            koeff = (max - min) / Width;
            minTime = min;
        }

        // Начало шкалы: полосы откладываются от самого раннего условия, а не от нуля
        float minTime;

        private void dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 1)
            {
                // Get task data for the current row
                var currentCond = (ICondData)dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                // Calculate bar position and width based on dates and column width
                // This is a simplified example; actual calculation would be more complex
                // and depend on the time scale represented by the column.
                // koeff = 0, если все условия мгновенные (max == min): полосы нулевой длины в начале шкалы
                var scale = koeff > 0 ? koeff : 1;
                int barStartPixel = (int)((currentCond.StartTime - minTime) / scale);
                int barWidthPixel = (int)((currentCond.StopTime - minTime) / scale);

                e.Graphics.FillRectangle(new SolidBrush(Color.White), e.CellBounds);

                Color color;

                if (currentCond.Kind == DataKind.Закрепление)
                    color = Color.Black;
                else if (currentCond.Kind == DataKind.Материал)
                    color = Color.Green;
                else if (currentCond.Kind == DataKind.Среда)
                    color = Color.Yellow;
                else if (currentCond.Kind == DataKind.Нагрев)
                    color = Color.Red;
                else
                    color = Color.Blue;

                var barBrush = new SolidBrush(color);

                e.Graphics.FillRectangle(barBrush, e.CellBounds.X + barStartPixel, e.CellBounds.Y + 5,
                    barWidthPixel - barStartPixel, e.CellBounds.Height - 10);

                e.Handled = true; // Prevent default cell painting

            }
        }

        private void dataGridView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Name == CondTime.Name)
            {
                var value1 = e.CellValue1?.ToString().Split(" : ")[1].Split(' ')[5];
                var value2 = e.CellValue2?.ToString().Split(" : ")[1].Split(' ')[5];

                if (float.TryParse(value1, out float floatValue1) && float.TryParse(value2, out float floatValue2)) 
                {
                    e.SortResult = floatValue1.CompareTo(floatValue2);
                    e.Handled = true;
                }
            }
        }
    }
}
