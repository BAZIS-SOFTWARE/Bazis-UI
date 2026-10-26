using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinGUI.TasksControls
{
    public partial class BasicControl : UserControl
    {
        public BasicControl()
        {
            InitializeComponent();
        }

        public string InitTemp 
        { 
            get { return txbIniTemp.Text; }
            set { txbIniTemp.Text = value; }
        }
        public string SaveRate 
        { 
            get { return txbSaveRate.Text; }
            set { txbSaveRate.Text = value; }
        }
        public string Iterations 
        { 
            get {return txbIters.Text; }
            set { txbIters.Text = value; }
        }

        public bool GetValidationResult()
        {
            var checks = new List<bool>()
            {
                txbIniTemp.IsValueValid(),
                txbIters.IsValueValid(),
                txbSaveRate.IsValueValid(),
            };
            return checks.All(x => x);
        }
    }
}
