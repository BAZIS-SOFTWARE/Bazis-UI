using WinGUI.Extensions;
using WinGUI.Scene.VBO;
using Model.Interfaces;

namespace WinGUI
{
    public partial class BaseForm
    {
        public void DeleteVBObjects(string objects)
        {
            if (objects == "Объекты")
            {
                VBOController.DeleteAllVBObjects();
            }
            else if (objects == "Элементы")
            {
                DeleteVBObjsByObjsType(ObjType.Элемент1D);
                DeleteVBObjsByObjsType(ObjType.Элемент2D);
                DeleteVBObjsByObjsType(ObjType.Элемент3D);
            }
            else if (objects == "Геометрия")
            {
                DeleteVBObjsByObjsType(ObjType.Точка);
                DeleteVBObjsByObjsType(ObjType.Кривая);
                DeleteVBObjsByObjsType(ObjType.Поверхность);
            }
            else
            {
                var objType = objects.ToEnum<ObjType>();
                DeleteVBObjsByObjsType(objType);
            }
                
        }
        

        public void DeleteVBObjsByObjsType(ObjType objType)
        {
            foreach (var setInfo in project.GetModelSetsInfo(objType))
            {
                if (setInfo.NumberOfObjects > 0)
                {
                    VBOController.DeleteVBObjects(setInfo.Name);
                }
            }
        }
    }
}
