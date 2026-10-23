using WinGUI.Navigator;
using Model.Interfaces;
using System.Collections.Generic;

namespace WinGUI
{
    public partial class BaseForm
    {
        //private void navigator_ShowObjectsEvent(TempNodeName obj)
        //{
        //    ChangeObjectsViewState(obj,true);
        //}

        //private void navigator_HideObjectsEvent(TempNodeName obj)
        //{
        //    ChangeObjectsViewState(obj, false);
        //}

        //private void ChangeObjectsViewState(TempNodeName obj, bool viewState)
        //{
        //    List<ObjType> types;
        //    if (obj == TempNodeName.Mesh)
        //        types = new List<ObjType>()
        //        { ObjType.Узел, ObjType.Элемент1D, ObjType.Элемент2D, ObjType.Элемент3D};
        //    else
        //        types = new List<ObjType>()
        //        { ObjType.Точка, ObjType.Кривая, ObjType.Поверхность};
            
        //    foreach (var item in types)
        //        foreach (var set in project.GetModelSetsInfo(item))
        //        {
        //            set.SetViewState(viewState);
        //            set.SetBackColor();
        //            /* 
        //             * тут удаление vbo при скрытии так как в дальнейшем 
        //             * если не удалить может 
        //             * возникнуть рассинхронизация
        //            */
        //            VBOController.DeleteVBObjects(set.Name);

        //            if (viewState)
        //            {
        //                var pres = project.CreateModelObjectsPresentor(set);
        //                var vb = CreateVBObject(pres);
        //                VBOController.AddVbo(vb);
        //            }
        //        }

        //    DisplayObjects();
        //}
    }
}
