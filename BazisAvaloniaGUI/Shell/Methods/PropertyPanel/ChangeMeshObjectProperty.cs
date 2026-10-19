using BazisAvaloniaGUI.Properties;
using Model.Interfaces;
using Model.MeshObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private void ChangeNodeProperty(PropertyChangedEventArgs obj, int number)
        {
            var mObj = project.GetModelObject(ObjType.Узел, number);
            var c = mObj.GetCoordinates().First();

            if (Enum.TryParse(obj.Key, out NodePropertyKeys key))
            {
                switch (key)
                {
                    case NodePropertyKeys.CordX:
                        c._x = float.Parse(obj.NewValue);
                        break;

                    case NodePropertyKeys.CordY:
                        c._y = float.Parse(obj.NewValue);
                        break;

                    case NodePropertyKeys.CordZ:
                        c._z = float.Parse(obj.NewValue);
                        break;
                }

                var objTypes = new List<ObjType> { ObjType.Узел, ObjType.Элемент1D, ObjType.Элемент2D, ObjType.Элемент3D };
                foreach (var type in objTypes)
                {
                    foreach (var setInfo in project.GetModelSetsInfo(type))
                    {
                        var presentor = project.CreateModelObjectsPresentor(setInfo);
                        SetVBObjectAttribute(presentor, "координаты");
                    }
                }

                navigator.SelectedNode.Text = mObj.ToString();
                RequestRedraw();
            }
        }

        private void ChangeElementProperty(PropertyChangedEventArgs obj, ObjType objType, int number)
        {
            // получаем объект
            var mObj = project.GetModelObject(objType, number);
            var element = mObj as Element;
            element.Level = int.Parse(obj.NewValue);

            navigator.SelectedNode.Text = mObj.ToString();
            RequestRedraw();
        }
    }
}
