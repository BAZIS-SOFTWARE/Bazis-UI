using BazisAvaloniaGUI.Localization;
using Model.Interfaces;
using System;
using System.Drawing;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private void наПоверхностиГеометрииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteMeshObjects(ObjType.Узел);
                project.ClearModelCollection(ObjType.Узел);
                project.GenerateMesh(2);

                var error = project.GetGeometryLastError();
                if (!string.IsNullOrEmpty(error))
                    console.PrintInfo(error, Color.Red);

                DeleteVBObjsByObjsType(ObjType.Узел);
                CreateVBObjsByObjsType(ObjType.Узел);
                DeleteVBObjects("Элементы");
                CreateVBObjects("Элементы");
                PresentMeshData();
                PresentModelObjectsForSelection();
                FitObjectsToScreen();
                RequestRedraw();

                console.PrintInfo($"{Resources.GenerateMesh2DEvent_GenerateOnGeometry_GenElements_Message}" +
                    $" 1D: {project.GetModelObjects(ObjType.Элемент1D).Count()}," +
                    $" 2D: {project.GetModelObjects(ObjType.Элемент2D).Count()}." +
                    Resources.GenerateMesh2DEvent_GenerateOnGeometry_Recomendation_Message, Color.Orange);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
                return;
            }
        }
    }
}
