using WinGUI.Extensions;
using WinGUI.Navigator;
using WinGUI.Utilities;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinGUI
{
    public partial class BaseForm
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

                }
                else
                    return;        
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message,Color.Red);
            }

        }
    }
}
