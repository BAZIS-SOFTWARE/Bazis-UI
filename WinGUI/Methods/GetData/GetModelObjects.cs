using Model.Interfaces;
using System.Collections.Generic;
using WinGUI.Extensions;
using WinGUI.Utilities;

namespace WinGUI
{
    public partial class BaseForm
    {
        public IEnumerable<IModelObject> GetModelObjects(SelectionType selection)
        {
            if (selection == SelectionType.Objects)
                return project.GetAllModelObjects();
            else if (selection == SelectionType.Elements)
                return project.GetAllModelElements();
            else
            {
                var objType = Converters.ConvertSelectionTypeToObjType(selection);
                return project.GetModelObjects(objType);
            }
        }
    }
}
