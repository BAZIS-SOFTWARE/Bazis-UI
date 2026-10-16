using Model.Interfaces;
using System;
using System.Drawing;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private void квадратизацияСуществующейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            project.RecombineMesh();

            var error = project.GetGeometryLastError();
            if (!string.IsNullOrEmpty(error))
                console.PrintInfo(error, Color.Red);

            DeleteVBObjsByObjsType(ObjType.Узел);
            CreateVBObjsByObjsType(ObjType.Узел);
            DeleteVBObjects("Элементы");
            CreateVBObjects("Элементы");
            PresentMeshData();
            PresentModelObjectsForSelection();
            RequestRedraw();
        }
    }
}
