
using System;
using System.Windows.Forms;

namespace BazisGUI.Win
{
    public partial class AboutLicenseControl : UserControl
    {
        public AboutLicenseControl()
        {
            InitializeComponent();
        }

        public string OwnerInfo { set { lblCompanyName.Text = value; } }

        public string AdressInfo { set { lblServerAdress.Text = value; } }

        public string KeysInfo 
        { 
            get { return lblKeyInfo.Text; } 
            set { lblKeyInfo.Text = value; } 
        }
    }
}
