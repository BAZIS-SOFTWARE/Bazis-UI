using Model.Interfaces;
using System;
using System.Drawing;

namespace WinGUI
{
    public partial class BaseForm
    {
        private void уплотнитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            project.RefineMesh();

            var error = project.GetGeometryLastError();
            if (!string.IsNullOrEmpty(error))
                console.PrintInfo(error, Color.Red);

            FitObjectsToScreen();
            RequestRedraw();
        }
    }
}
