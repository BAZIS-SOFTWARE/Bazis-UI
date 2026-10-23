using WinGUI.Properties;
using Model.Interfaces;
using System;
using System.Drawing;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security;

namespace WinGUI
{
    public partial class BaseForm
    {
        [HandleProcessCorruptedStateExceptions]
        [SecurityCritical]
        private void наПоверхностиГеометрииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteMeshObjects(ObjType.Узел);
                project.ClearModelCollection(ObjType.Узел);
                project.GenerateMesh(2);

                //gmshController.Gmsh.Model.Mesh.Generate(3);
                //var nds = gmshController.GetNodes();

                var error = project.GetGeometryLastError();
                if (!string.IsNullOrEmpty(error))
                    console.PrintInfo(error, Color.Red);

                FitObjectsToScreen();
                RequestRedraw();

                console.PrintInfo($"{Resources.GenerateMesh2DEvent_GenerateOnGeometry_GenElements_Message}"+
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
