using SceneCore.Interfaces;
using SceneCore.Interfaces;
using System;
using Geometry;
using System.Drawing;
using MathNet.Numerics.LinearAlgebra;
using WinGUI;
using System.Windows.Forms;
using Model.Interfaces;
using System.Linq;
using WinGUI.Properties;

namespace WinGUI
{
    public partial class BaseForm
    {
        private void btnDisplayViews_Click(object sender, EventArgs e)
        {
            //var btnSelect = sender as Button;
            var flag = bool.Parse(btnDisplayViews.Tag.ToString());
            if (!flag)
            {
                flag = true;
                btnDisplayViews.Image = Resources.arrow_d;
            }

            else
            {
                flag = false;
                btnDisplayViews.Image = Resources.arrow_r;
            }


            btnDisplayViews.Tag = flag;


            //TODO
            btnXY.Visible = flag;
            btnZX.Visible = flag;
            btnZY.Visible = flag;
            btnRotX.Visible = flag;
            btnRotY.Visible = flag;
            btnRotZ.Visible = flag;
            btnRotVert90.Visible = flag;
            btnRotHor90.Visible = flag;
        }
    }
}
