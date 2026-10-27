using BazisGUI.Win.Localization;
using BazisGUI.Win.PinnedControl;
using BazisGUI.Win.Properties;
using BazisGUI.Win.PropertiesPanel.DataGridViewNumericUpDown;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UserControlsEx;

namespace BazisGUI.Win.PropertiesPanel
{
    public partial class PropertiesPanelControl : PinnedPage//, ILocalizableHeaderControl
    {
        public event Action<PropertyChangedEventArgs> PropertyUpdateEvent;
        public event Action<PropertyChangedEventArgs> ReDrawEvent;

        public delegate bool Validator(string header, string value, out string corrected);
        public event Validator ValidateValue;

        private string _oldValue;
        private bool _isValid;
        private string objInfo; // возможно костыль, хранит инфо об объекте сво-ва которого представлены
        private int tag; // возможно костыль, хранит инфо об источнике, где были получены сво-ва объекта
        public PropertiesPanelControl()
        {
            InitializeComponent();
            dataGridView1.DataError += DataGridView1_DataError;
            dataGridView1.CurrentCellDirtyStateChanged += DataGridView1_CurrentCellDirtyStateChanged;
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Header",
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = SystemColors.Control,
                    SelectionBackColor = SystemColors.ControlDark,
                    Padding = new Padding(15, 0, 0, 0)
                },
                ReadOnly = true,
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Value",
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = SystemColors.Control,
                    SelectionBackColor = SystemColors.ControlDark
                },
            });
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        //public string GetLocalizableHeaderText() => Resources.PropertiesPanelControl_headerName_text;

        private void DataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {

            if (dataGridView1.CurrentCell is DataGridViewCheckBoxCell)
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            else if (dataGridView1.CurrentCell is DataGridViewComboBoxCell)
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void DataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Заглушка
        }

        public void ClearTable()
        {
            dataGridView1.Rows.Clear();
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
            dataGridView1.Rows.Clear();
            // Тут при создании строки таблицы должно происходить автоопределение типа элемента ячейки
            // comboBox,TextBox, CheckBox etc.
            foreach (var prop in rows)// Инициализация строк через RowProperty
            {
                var row = new DataGridViewRowEx { Key = prop.Key };
                row.DefaultCellStyle.BackColor = prop.Color;

                row.Cells.Add(new DataGridViewTextBoxCell { Value = prop.LocalizedHeader }); // Имя свойства

                DataGridViewCell cell; // Значение свойства

                if(prop.Value is bool chbv)
                {
                    cell = new DataGridViewCheckBoxCell();
                    cell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    cell.Value = chbv;
                }

                else if (prop.Value is DropDownPropertyValue ddpv)
                {
                    var comboCell = new DataGridViewComboBoxCell();
                    comboCell.Items.AddRange(ddpv.AvailableValues.ToArray());
                    comboCell.Value= ddpv.Value.ToString();
                    cell = comboCell;
                }

                else if (prop.Value is NumericUpDownValue nudpv)
                { 
                    var numericUpDownCell = new DataGridViewNumericUpDownCell()
                    {
                        Minimum = nudpv.Minimum,
                        Maximum = nudpv.Maximum,
                        Increment = nudpv.Increment,
                        DecimalPlaces = nudpv.DecimalPlaces,
                    };
                    numericUpDownCell.Value = Convert.ToDecimal(nudpv.Value);
                    cell = numericUpDownCell;
                }
                else if (prop.Value is ButtonPropertyValue bv)
                {
                    var btnCell = new DataGridViewButtonCell();
                    //btnCell
                    btnCell.Style.Tag = bv;
                    btnCell.Value = bv.Text;
                    cell = btnCell;
                }

                else
                {
                    cell = new DataGridViewTextBoxCell();
                    cell.Value= prop.Value.ToString() ;
                }
                    

                if (prop.LocalizedHeader == Resources.Header_Color)
                    cell.Style.BackColor = (Color)prop.Value;

                cell.Tag = prop.ValidationType.ToString();

                row.Cells.Add(cell);
                cell.ReadOnly = prop.IsReadOnly;
                dataGridView1.Rows.Add(row);
            }
        }
        private void DataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 1)
            {
                if(dataGridView1.Rows[e.RowIndex].Cells[1].Value != null)
                    _oldValue = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            }

        }

        public void CellValueChanged(DataGridViewCell e, string key)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 1)
            {
                // TODO: получение ключа из DGVCellEx
                var header = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                var cellValue = dataGridView1.Rows[e.RowIndex].Cells[1].Value;
                var newValue = cellValue?.ToString() ?? string.Empty;

                if (header == Resources.Header_Color)
                {
                    var color = ChangeColorCell(newValue);
                    dataGridView1.Rows[e.RowIndex].Cells[1].Style.BackColor = color;
                }
                var eventArgs = new PropertyChangedEventArgs(key, header, newValue, _oldValue);
                
                if (objInfo != null)
                    eventArgs.ObjInfo = objInfo;

                eventArgs.Tag = tag;

                PropertyUpdateEvent?.Invoke(eventArgs);
            }
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0) return;

            var value = "";
            if (dataGridView1[0,e.RowIndex].Value.ToString() == Resources.Header_Color)
            {
                ColorDialog colorDialog = new ColorDialog();
                if(colorDialog.ShowDialog() == DialogResult.OK)
                {
                    value = colorDialog.Color.ToString();
                }
            }

            else if (dataGridView1[0, e.RowIndex].Value.ToString() == Resources.Header_File)
            {
                var fileDialog = new OpenFileDialog();
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    value = fileDialog.FileName;
                }
            }

            if(value != "")
            {
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = value;
                dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[0];
            }


            var cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];

            if (cell is DataGridViewButtonCell bt)
            {
                var buttonSet = cell.Style.Tag as ButtonPropertyValue;
               
                if(buttonSet != null)
                    buttonSet.OnClick?.Invoke();
            }

        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells[1].Tag.ToString() != ValidationType.None.ToString())
            {
                var cellValue = dataGridView1.Rows[e.RowIndex].Cells[1].Value;
                var newValue = cellValue?.ToString() ?? string.Empty;
                var tag = dataGridView1.Rows[e.RowIndex].Cells[1].Tag.ToString();
                var corrected = newValue;
                _isValid = ValidateValue?.Invoke(tag, newValue, out corrected) ?? true;

                if (!_isValid)
                {
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = _oldValue;
                    return;
                }
                if (newValue != corrected) dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = corrected;
            }
            var row = dataGridView1.Rows[e.RowIndex] as DataGridViewRowEx;
            CellValueChanged(row.Cells[e.ColumnIndex], row.Key);
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
    }
}
