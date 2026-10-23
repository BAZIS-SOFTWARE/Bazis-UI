using WinGUI.Scene.Interfaces;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinGUI
{
    public partial class BaseForm
    {
        private void btnRot_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            var btn = sender as Button;

            if (bool.Parse(btn.Tag.ToString()))
                g.DrawRectangle(new Pen(Color.Black, 1.5f),
                    1, 1, btn.Width - 3, btn.Height - 3);
        }
        private void btnXY_Click(object sender, EventArgs e)
        {
            SetOnPlane(ViewPlane.XY, ScaleFactor);
            RequestRedraw();
        }

        private void btnZX_Click(object sender, EventArgs e)
        {
            SetOnPlane(ViewPlane.XZ, ScaleFactor);
            RequestRedraw();
        }

        private void btnZY_Click(object sender, EventArgs e)
        {
            SetOnPlane(ViewPlane.YZ, ScaleFactor);
            RequestRedraw();
        }

        private void btnRotX_Click(object sender, EventArgs e)
        {
            if (!bool.Parse(btnRotX.Tag.ToString()))
            {
                btnRotX.Tag = true;

                settingsConfig.RotationAxis = ViewAxis.X;

                btnRotY.Tag = false;
                btnRotZ.Tag = false;
            }
            else
            {
                btnRotX.Tag = false;
                settingsConfig.RotationAxis = ViewAxis.XYZ;
            }
            RequestRedraw();
        }

        private void btnRotY_Click(object sender, EventArgs e)
        {
            if (!bool.Parse(btnRotY.Tag.ToString()))
            {
                btnRotY.Tag = true;

                settingsConfig.RotationAxis = ViewAxis.Y;

                btnRotX.Tag = false;
                btnRotZ.Tag = false;
            }
            else
            {
                btnRotY.Tag = false;
                settingsConfig.RotationAxis = ViewAxis.XYZ;
            }
            RequestRedraw();
        }

        private void btnRotZ_Click(object sender, EventArgs e)
        {
            if (!bool.Parse(btnRotZ.Tag.ToString()))
            {
                btnRotZ.Tag = true;

                settingsConfig.RotationAxis = ViewAxis.Z;

                btnRotX.Tag = false;
                btnRotY.Tag = false;
            }
            else
            {
                btnRotZ.Tag = false;
                settingsConfig.RotationAxis = ViewAxis.XYZ;
            }
            RequestRedraw();
        }

        private void btnRotHor90_Click(object sender, EventArgs e)
        {
            RotateCamera(ViewAxis.Y, 90);
            RequestRedraw();
        }

        private void btnRotVert90_Click(object sender, EventArgs e)
        {
            RotateCamera(ViewAxis.X, 90);
            RequestRedraw();
        }  
    }
}
