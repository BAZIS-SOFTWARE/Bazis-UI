using SceneCore.Interfaces;
using SceneCore.VBO;
using System;
using Geometry;
using System.Linq;
using Model.Interfaces;

namespace WinGUI
{
    public partial class BaseForm
    {
        public void ClearAllGeometryDataOnScene()
        {
            VBOController.DeleteVBObjects(ObjType.Точка.ToString());
            VBOController.DeleteVBObjects(ObjType.Кривая.ToString());
            VBOController.DeleteVBObjects(ObjType.Поверхность.ToString());
        }

        public void ClearAllMeshDataOnScene()
        {
            VBOController.DeleteVBObjects(ObjType.Узел.ToString());
            VBOController.DeleteVBObjects(ObjType.Элемент1D.ToString());
            VBOController.DeleteVBObjects(ObjType.Элемент2D.ToString());
            VBOController.DeleteVBObjects(ObjType.Элемент3D.ToString());
        }
        public void ClearAllDataOnScene()
        {
            DisplayGeometryObjectEvent = null;
            DisplayText2DEvent = null;
            DisplayText3DEvent = null;
            VBOController.DeleteAllVBObjects();
        }

    }
}
