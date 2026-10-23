using WinGUI.Scene.Interfaces;
using WinGUI.Scene.Interfaces;
using System;
using Geometry;
using System.Drawing;
using MathNet.Numerics.LinearAlgebra;
using WinGUI;
using System.Windows.Forms;
using Model.Interfaces;
using System.Linq;

namespace WinGUI
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
