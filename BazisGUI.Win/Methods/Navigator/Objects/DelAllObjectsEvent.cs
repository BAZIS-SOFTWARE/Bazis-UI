using BazisGUI.Win.Extensions;
using BazisGUI.Win.Navigator;
using Model.Interfaces;
using System;
using System.Drawing;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        private void navigator_DelAllObjectsEvent()
        {
            try
            {
                // TODO Подумать над очисткой данных геометрии
                if (navigator.SelectedNode.Name == NodeName.Mesh.ToString())
                {
                    project.ClearModelCollection(ObjType.Узел);

                    // Ядро перестраивает только буферы наборов: вспомогательные объекты и подписи
                    // удалённой сетки убираем сами (полный ClearAllDataOnScene удалил бы и буферы геометрии)
                    DisplayGeometryObjectEvent = null;
                    DisplayText2DEvent = null;
                    DisplayText3DEvent = null;
                    foreach (var name in new[] { "Boundary", "Normals", "transPoints", "crossSection" })
                        VBOController.DeleteVBObjects(name);
                    RequestRedraw();
                }
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }

        }
    }
}
