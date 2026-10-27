using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BazisGUI.Win.DataBases.MetallurgyGUI
{
    public partial class CCTControl : UserControl
    {
        public CCTControl()
        {
            InitializeComponent();
        }

        public void AddPhase(string phaseName)
        {
            if (!cmbPhases.Items.Contains(phaseName))
            {
                cmbPhases.Items.Add(phaseName);
            }
        }

        public IEnumerable<string> GetPhases()
        {
            foreach (var item in cmbPhases.Items)
            {
                yield return (string)item;
            }
        }

        public string InitialPhase { get { return cmbPhases.SelectedItem == null ? "" : cmbPhases.SelectedItem.ToString(); } }

        public float IniTemp { get { return float.Parse(txbIniTemp.Text); } }
        public float FinTemp { get { return float.Parse(txbFinTemp.Text); } }
        public float MaxVel { get { return float.Parse(txbMaxVel.Text); } }

        public float MinVel { get { return float.Parse(txbMinVel.Text); } }

        public decimal VelNumber { get { return nudVels.Value; } }
    }
}
