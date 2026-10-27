using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BazisGUI.Win.TasksControls
{
    public partial class MechTaskControl_v2 : UserControl
    {

        public MechTaskControl_v2()
        {
            InitializeComponent();
        }

        public string MaxDU
        {
            get { return txbMaxDU.Text; }
            set { txbMaxDU.Text = value; }
        }

        public string MaxU
        {
            get { return txbMaxU.Text; }
            set { txbMaxU.Text = value; }
        }

        public string MaxSiSt
        {
            get { return txbMaxSiSt.Text; }
            set { txbMaxSiSt.Text = value; }
        }

        public bool GetValidationResult()
        {
            var checks = new List<bool>()
            {
                txbMaxDU.IsValueValid(),
                txbMaxU.IsValueValid(),
                txbMaxSiSt.IsValueValid()
            };
            return checks.All(x => x);
        }       
    }
}
