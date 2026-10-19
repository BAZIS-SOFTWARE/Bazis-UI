using BazisAvaloniaGUI.Localization;
using Model.Interfaces;
using System;
using System.Drawing;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private void создать3DСеткуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (project == null || !project.IsGeometryInitialized)
                    throw new InvalidOperationException(Resources.GenerateMesh3DEvents_Generate3D_GMSHNull_Exception);

                DeleteMeshObjects(ObjType.Узел);
                project.ClearModelCollection(ObjType.Узел);
                project.GenerateMesh(3);

                if (!settingsConfig.ShowAllMeshWhenGeneration)
                {
                    project.HideMesh(1);
                    project.HideMesh(2);
                    project.HideMesh(3);
                }

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

                console.PrintInfo(Resources.GenerateMesh3DEvents_Generate3D_GeneratedElements_Message +
                    $" 1D: {project.GetModelObjects(ObjType.Элемент1D).Count()}," +
                    $" 2D: {project.GetModelObjects(ObjType.Элемент2D).Count()}," +
                    $" 3D: {project.GetModelObjects(ObjType.Элемент3D).Count()}." +
                    $" {Resources.GenerateMesh3DEvents_Generate3D_CheckRecommendation_Message}", Color.Orange);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
                return;
            }
        }

        private void DeleteMeshObjects(ObjType type)
        {
            switch (type)
            {
                case ObjType.Узел:
                    project.ClearGeometryMesh();
                    break;
                case ObjType.Элемент1D:
                    project.ClearGeometryMesh(1);
                    break;
                case ObjType.Элемент2D:
                    project.ClearGeometryMesh(2);
                    break;
                case ObjType.Элемент3D:
                    project.ClearGeometryMesh(3);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type));
            }
        }
    }
}
