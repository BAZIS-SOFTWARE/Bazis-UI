using WinGUI;
using WinGUI.Properties;
using SceneCore.Interfaces;
using SceneCore.Interfaces;
using Geometry;
using MathNet.Numerics.LinearAlgebra;
using Model.Interfaces;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static IronPython.Modules._ast;

namespace WinGUI
{
    public partial class BaseForm
    {
        private void btnMakeScreenShot_Click(object sender, EventArgs e)
        {
            try
            {
                var image = CreateScreenShot();
                var path = Path.Combine(WorkingDir, "screenShot.bmp");
                image.Save(path, System.Drawing.Imaging.ImageFormat.Bmp);
                console.PrintInfo($"{Resources.MakeScreenShot_ScreenShotTaken_Message}: {path}", Color.Black);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }
    }
}
