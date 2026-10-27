using BazisGUI.SceneCore.Interfaces;
using BazisGUI.SceneCore.Interfaces;
using System;
using Geometry;
using System.Drawing;
using MathNet.Numerics.LinearAlgebra;
using BazisGUI.Win;
using System.Windows.Forms;
using Model.Interfaces;
using System.Linq;
using BazisGUI.Win.Properties;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        private void btnBazis_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            if(bool.Parse(btnBazis.Tag.ToString()))
                g.DrawRectangle(new Pen(Color.Black, 1.5f), 
                    1,1, btnBazis.Width - 3,btnBazis.Height - 3);
        }
        private void btnBazis_Click(object sender, EventArgs e)
        {
            var flag = bool.Parse(btnBazis.Tag.ToString());
            if (!flag)
            {
                flag = true;              
            }

            else
            {
                flag = false;
            }
            btnBazis.Tag = flag;
            settingsConfig.DisplayBasis = flag;
            sceneController.DisplayBasis = flag;
            RequestRedraw();
        }
    }
}
