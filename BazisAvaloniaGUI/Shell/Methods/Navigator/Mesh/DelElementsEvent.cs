using Model.Interfaces;
using System;
using System.Drawing;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private void DelElements(int obj)
        {
            try
            {
                ObjType objType;
                if (obj == 1)
                    objType = ObjType.Элемент1D;
                else if (obj == 2)
                    objType = ObjType.Элемент2D;
                else
                    objType = ObjType.Элемент3D;

                var names = project.GetModelSetsInfo(objType).
    Select(x => x.Name).ToList();
                foreach (var item in names)
                {
                    project.DeleteModelSet(objType, item);
                    VBOController.DeleteVBObjects(item);
                }
                RequestRedraw();
                PresentMeshData();
                PresentModelObjectsForSelection();
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }
    }
}
