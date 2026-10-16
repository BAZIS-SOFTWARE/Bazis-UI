using BazisAvaloniaGUI.Extensions;
using BazisAvaloniaGUI.Navigator;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private void navigator_DelSetEvent()
        {
            try
            {
                var node = navigator.SelectedNode;
                var objInfo = node.Text.Split(' ')[0];
                var setName = node.Text.Split(' ')[1];
                ObjType objType;
                // пока заглушим обработку объема
                if (objInfo.TryToEnum(out objType))
                {

                    if (objType == ObjType.Узел)
                    {
                        DeleteVBObjects("Элементы");
                        project.ClearModelCollection(objType);
                    }

                    else if (objType == ObjType.Элемент1D |
                        objType == ObjType.Элемент2D |
                        objType == ObjType.Элемент3D)
                    {
                        project.DeleteModelSet(objType, setName);
                    }
                    else
                        return;

                    VBOController.DeleteVBObjects(setName);

                    //удаляем узел
                    node.Remove();

                    PresentGroupDataOnTree();
                    PresentCondDataOnTree();
                    PresentMeshData();
                    PresentModelObjectsForSelection();
                    if (navigator.TrySearchNodes(NodeName.Mesh, out List<TreeNode> nodes))
                    {
                        nodes.First().Collapse();
                        nodes.First().Expand();
                    }

                    RequestRedraw();

                }
                else
                    return;
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }

        }
    }
}
