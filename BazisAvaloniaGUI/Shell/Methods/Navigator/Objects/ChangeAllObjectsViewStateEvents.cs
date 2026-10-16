using BazisAvaloniaGUI.Extensions;
using BazisAvaloniaGUI.Navigator;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {


        private void navigator_ChangeAllObjectsViewStateEvent(bool state)
        {
            try
            {
                var node = navigator.SelectedNode.Name.ToEnum<NodeName>();
                var types = new List<ObjType>();
                if (node == NodeName.Geometry)
                {
                    types = new List<ObjType>()
                    {
                        ObjType.Точка,
                        ObjType.Кривая,
                        ObjType.Поверхность
                    };
                }

                else if (node == NodeName.Mesh)
                {
                    types = new List<ObjType>()
                    {
                        ObjType.Узел,
                        ObjType.Элемент1D,
                        ObjType.Элемент2D,
                        ObjType.Элемент3D
                    };
                }

                foreach (var type in types)
                {
                    foreach (var set in project.GetModelSetsInfo(type))
                        project.SetVisible(type, set.GetNumbers(), state);
                }

            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }

        }
    }
}
