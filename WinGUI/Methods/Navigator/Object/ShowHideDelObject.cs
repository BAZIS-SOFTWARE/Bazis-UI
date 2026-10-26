using WinGUI.Extensions;
using WinGUI.Properties;
using WinGUI.Utilities;
using Model.Interfaces;
using Model.Interfaces.ObjectsCollections;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace WinGUI
{
    public partial class BaseForm
    {
        private void navigator_ShowObjectEvent()
        {
            var number = int.Parse(navigator.SelectedNode.Text.Split(' ')[0]);
            var objInfo = navigator.SelectedNode.Text.Split(' ')[1];
            ShowHideObject(objInfo, number, true);
        }

        private void navigator_HideObjectEvent()
        {
            var number = int.Parse(navigator.SelectedNode.Text.Split(' ')[0]);
            var objInfo = navigator.SelectedNode.Text.Split(' ')[1];
            ShowHideObject(objInfo, number, false);
        }

        public void ShowHideObject(string objInfo ,int number,bool flag)
        {
            try
            {
                ISetInfo set;
                ObjType objType;
                // пока заглушим обработку объема
                if (objInfo.TryToEnum(out objType))
                {
                    //var objType = Converters.ConvertNavigatorNodeNameToObjType(nodeName);
                    set = project.GetModelSetInfo(objType, number);
                    project.SetVisible(objType, [number], flag);
                }
                else
                {
                    set = project.GetModelSetsInfo(ObjType.Поверхность).First();
                    var vol = project.GetModelVolumes().First(x => x.Number == number);
                    var numbers = vol.GetSurfaceFigures().Select(x => x.Number);
                    project.SetVisible(ObjType.Поверхность, numbers, flag);
                }


            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }
        
        private void navigator_DelObjectEvent()
        {
            var node = navigator.SelectedNode;
            var info = node.Text.Split(' ');
            var number = int.Parse(info[0]);

            if(node.Parent.Parent.Text == Resources.Navigator_TreeView_Node_Text_Geometry)
            {
                if (info[1].TryToEnum(out ObjType objType))
                    Navigator_DeleteGeometry((int)objType, number);
                else
                {
                    Navigator_DeleteGeometry(3, number);
                }
            }
            else if(node.Parent.Parent.Text == Resources.Navigator_TreeView_Node_Text_Mesh)
            {
                if (info[1].TryToEnum(out ObjType objType))
                {
                    var obj = project.GetModelObject(objType, number);
                    obj.ExistState = false;

                    project.ClearNotExistedModelData();
                }
            }
        }

        private void Navigator_DeleteGeometry(int dim, int number)
        {
            try
            {
                project.DeleteGeometryObject(dim, number);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }
    }
}
