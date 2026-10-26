using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace WinGUI.DataBases.MechanicalGUI
{
    public partial class CreepControl : UserControl
    {
        private DataTable youngTable;
        private DataTable ultimateTable;
        private DataTable creepKoeffTable;
        private DataTable phaseTable;

        public CreepControl(List<DataTable> tables)
        {
            InitializeComponent();

            youngTable = tables.First(x => x.TableName.Contains("Модуль Юнга"));
            creepKoeffTable = tables.First(x => x.TableName.Contains("Коэффициент ползучести"));
 
            phaseTable = tables.First(table => table.TableName.Contains("Структура"));
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            var length = float.Parse(txbLength.Text);
            var rad = float.Parse(txbDiam.Text) / 2;

            var square = rad * rad * Math.PI;

            //var def = 
        }
    }
}
