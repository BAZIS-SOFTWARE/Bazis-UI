using BazisAvaloniaGUI.Utilities;
using Model.Interfaces;
using System.Collections.Generic;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
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
