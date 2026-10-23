
using WinGUI.Properties;
using System;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace WinGUI.CrossSection
{
    public partial class CrossSectionControl : UserControl
    {
        public event Action<object, CreatePlaneFromTextArgs> CreateCrossFromTextArgs;
    
        public event Action CreateCrossFromNodesEvent;
        public event Action RemoveCrossEvent;
        public event Action SelectNodesEvent;
        public CrossSectionControl()
        {
            InitializeComponent();
            MinimumSize = new Size(MinimumSize.Width, MinimumSize.Height);
        }

        public void chbSelectPoints_CheckedChanged(object sender, EventArgs e)
        {
            txbPoint1.Enabled = true;
            txbPoint2.Enabled = true;
            txbPoint3.Enabled = true;
            rbtXY.Enabled = true;
            rbtXZ.Enabled = true;
            rbtYZ.Enabled = true;

            if (chbSelectPoints.Checked)
            {
                txbPoint1.Enabled = false;
                txbPoint2.Enabled = false;
                txbPoint3.Enabled = false;
                rbtXY.Enabled = false;
                rbtXZ.Enabled = false;
                rbtYZ.Enabled = false;

                SelectNodesEvent();
            }

        }

        public void RbtXY_CheckedChanged(object sender, EventArgs e)
        {
            txbPoint1.Text = "0;0;0";
            txbPoint2.Text = "1;0;0";
            txbPoint3.Text = "0;1;0";
        }

        public void RbtXZ_CheckedChanged(object sender, EventArgs e)
        {
            txbPoint1.Text = "0;0;0";
            txbPoint2.Text = "1;0;0";
            txbPoint3.Text = "0;0;1";
        }

        public void RbtYZ_CheckedChanged(object sender, EventArgs e)
        {
            txbPoint1.Text = "0;0;0";
            txbPoint2.Text = "0;1;0";
            txbPoint3.Text = "0;0;1";
        }

        public void btnCreateCrossSection_Click_1(object sender, EventArgs e)
        {
            if (txbPoint1 == null || txbPoint2 == null || txbPoint3 == null || txbPoint1.Text == txbPoint2.Text || txbPoint2.Text == txbPoint3.Text || txbPoint1.Text == txbPoint3.Text)
                throw new Exception(Resources.CrossSectionControl_InvalidSurfaceCoordsSetException);
        }

        public void btnCreatePlane_Click(object sender, EventArgs e)
        {
            if (chbSelectPoints.Checked == false)
            {
                string[] p1 = txbPoint1.Text.Split(';');
                string[] p2 = txbPoint2.Text.Split(';');
                string[] p3 = txbPoint3.Text.Split(';');

                var point1 = new Vector3(float.Parse(p1[0]), float.Parse(p1[1]), float.Parse(p1[2]));
                var point2 = new Vector3(float.Parse(p2[0]), float.Parse(p2[1]), float.Parse(p2[2]));
                var point3 = new Vector3(float.Parse(p3[0]), float.Parse(p3[1]), float.Parse(p3[2]));

                CreateCrossFromTextArgs(this, new CreatePlaneFromTextArgs(point1, point2, point3));
            }
            else if (chbSelectPoints.Checked == true)
            {
                CreateCrossFromNodesEvent();
            }
        }


        private void btnRemoveCross_Click(object sender, EventArgs e)
        {
            RemoveCrossEvent();
        }
    }
}
