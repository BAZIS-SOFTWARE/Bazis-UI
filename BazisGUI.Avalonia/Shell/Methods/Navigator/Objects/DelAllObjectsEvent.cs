using BazisGUI.Avalonia.Navigator;
using Model.Interfaces;
using System;
using System.Drawing;

namespace BazisGUI.Avalonia.Shell
{
    internal partial class MainWindow
    {
        private void navigator_DelAllObjectsEvent()
        {
            try
            {
                // TODO Подумать над очисткой данных геометрии
                if (navigator.SelectedNode.Name == NodeName.Mesh.ToString())
                {
                    project.ClearModelCollection(ObjType.Узел);

                    PresentMeshData();
                    PresentGroupDataOnTree();
                    PresentCondDataOnTree();
                    PresentModelObjectsForSelection();
                    ClearAllDataOnScene();
                    // ClearAllDataOnScene удаляет и буферы геометрии — пересоздаём оставшиеся объекты.
                    CreateVBObjects("Объекты");
                }

                RequestRedraw();
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }

        }
    }
}
