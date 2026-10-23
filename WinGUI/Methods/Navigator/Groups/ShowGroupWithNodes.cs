using WinGUI.PropertiesPanel;
using Model.Interfaces.MeshObjects;
using Model.Interfaces;
using Model.Utilities;
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
        private void ShowGroupWithNodes(IGroup group)
        {
            //var group = project.GetModelGroup(obj);
            using (project.BeginViewUpdate())
            {
                foreach (var iobj in group)
                {
                    var elem = (IElement)iobj;
                    project.SetVisible(elem.ObjType, [elem.Number], true);
                    var numbers = elem.GetVertexes().Select(x => x.Number);
                    project.SetVisible(ObjType.Узел, numbers, true);
                }
            }
        }
    }
}
