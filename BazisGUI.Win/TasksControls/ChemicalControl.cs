using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BazisGUI.Win.TasksControls
{
    public partial class ChemicalControl : UserControl
    {
        public ChemicalControl()
        {
            InitializeComponent();
        }

        public bool IsMaxConcentrSwitch
        {
            get { return chbMaxConcentr.Checked; }
            set { chbMaxConcentr.Checked = value; }
        }

        public string MaxConcentr
        {
            get { return txbMaxConcentr.Text; }
            set { txbMaxConcentr.Text = value; }
        }

        public string InitConcentr
        {
            get { return txbIniConcentr.Text; }
            set { txbIniConcentr.Text = value; }
        }

        public bool GetValidationResult()
        {
            var checks = new List<bool>()
            {
                txbMaxConcentr.IsValueValid(),
                txbIniConcentr.IsValueValid(),
            };
            return checks.All(x => x);
        }
    }

}
