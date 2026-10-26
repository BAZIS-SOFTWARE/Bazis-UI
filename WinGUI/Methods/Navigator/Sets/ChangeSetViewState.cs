using WinGUI.Extensions;
using WinGUI.PropertiesPanel;
using SceneCore.VBO;
using WinGUI.Utilities;
using Model.Interfaces;
using Model.Interfaces.ObjectsCollections;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinGUI
{
    public partial class BaseForm
    {
        private void ChangeSetViewState(string objInfo, string setName, bool viewState)
        {
            ISetInfo set;
            ObjType objType;
            // пока заглушим обработку объема
            if (!objInfo.TryToEnum(out objType))
            {
                set = project.GetModelSetInfo(ObjType.Поверхность, ObjType.Поверхность.ToString());

                var numbers = set.GetNumbers();
                project.SetVisible(set.ObjType, numbers, viewState);
            }
            else
            {
                //if (objType == ObjType.Поверхность)
                //{
                //    var ar = nodeText.Split(' ');
                //    setName = string.Join(" ", ar, 1, ar.Length - 2);
                //}
                //var objType = Converters.ConvertNavigatorNodeNameToObjType(nodeName);
                set = project.GetModelSetInfo(objType, setName);
                var numbers = set.GetNumbers();
                project.SetVisible(objType, numbers, viewState);
            }
            // Сделать выключение vbo не получиться. Потеряется синхронизация.
            //VBOController.SwitchVBObject(setName, viewState);

        }
    }
}
