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

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        private void btnFitToScreen_Click(object sender, EventArgs e)
        {
            try
            {
                FitObjectsToScreen();
                RequestRedraw();
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }

        }
    }
}
