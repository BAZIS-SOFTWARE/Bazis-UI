using WinGUI.Scene.Interfaces;
using System;
using Geometry;
using System.Drawing;
using WinGUI.Utilities;
using Model.Interfaces;
using WinGUI.Properties;

namespace WinGUI
{
    public partial class BaseForm
    {
        internal void ColorObjects(string objTypeStr)
        {
            if (objTypeStr == "Объекты")
            {
                foreach (ObjType type in Enum.GetValues(typeof(ObjType)))
                    ColorVBObjsByObjsType(type);
            }
            else if (objTypeStr == "Элементы")
            {
                ColorVBObjsByObjsType(ObjType.Элемент1D);
                ColorVBObjsByObjsType(ObjType.Элемент2D);
                ColorVBObjsByObjsType(ObjType.Элемент3D);
            }
            else
            {
                ObjType objType;
                var res = Enum.TryParse(objTypeStr, out objType) ? objType :
                    throw new Exception($"{Resources.ColorObjects_ObjectConversion_Exception} {objTypeStr}");
                ColorVBObjsByObjsType(objType);
            }


            RequestRedraw();
        }

        public void ColorVBObjsByObjsType(ObjType objType)
        {
            foreach (var setInfo in project.GetModelSetsInfo(objType))
            {
                if (setInfo.NumberOfObjects > 0)
                {
                    var pre = project.CreateModelObjectsPresentor(setInfo);
                    SetVBObjectAttribute(pre, "цвет");
                }
            }
        }
    }
}
