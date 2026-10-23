using Model.Interfaces;

namespace BazisGUI
{
    public partial class BaseForm
    { 

        //[HandleProcessCorruptedStateExceptions]
        //[SecurityCritical]
        //private void MeshGenerator_generate2DMeshEvent(object sender, double meshDencity)
        //{
        //    try
        //    {
        //        var cntr = (GMSHGeneralMeshControl)sender;
        //        GmshController.Gmsh.Option.SetNumber("Mesh.MeshSizeFactor", meshDencity);

        //        DeleteGMSHMeshObjects(ObjType.Узел);
        //        GmshController.Gmsh.Model.Mesh.Generate(1);
        //        GmshController.Gmsh.Model.Mesh.Generate(2);
        //    }
        //    catch (Exception ex)
        //    {
        //        console.PrintInfo(ex.Message, Color.Red);
        //        return;
        //    }
        //    var error = GmshController.Gmsh.Logger.GetLastError();
        //    if (!string.IsNullOrEmpty(error))
        //        console.PrintInfo(error, Color.Red);

        //    project.ModelData.ObjectData.Clear(ObjType.Узел);//Удаляем только элементы сетки, геометрию не трогаем

        //    FitObjectsToScreen();
        //    DisplayObjects();
        //}

   
    }
}
