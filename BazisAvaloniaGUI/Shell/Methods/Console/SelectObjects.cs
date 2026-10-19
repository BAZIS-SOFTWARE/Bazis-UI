using BazisAvaloniaGUI.Utilities;
using Model.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private bool TryParseObjType(string value, out ObjType objType)
        {
            switch (value?.Trim().ToLowerInvariant())
            {
                case "point":
                    objType = ObjType.Точка;
                    return true;
                case "curve":
                    objType = ObjType.Кривая;
                    return true;
                case "surface":
                    objType = ObjType.Поверхность;
                    return true;
                case "node":
                    objType = ObjType.Узел;
                    return true;
                case "line":
                    objType = ObjType.Элемент1D;
                    return true;
                case "element2d":
                    objType = ObjType.Элемент2D;
                    return true;
                case "element3d":
                    objType = ObjType.Элемент3D;
                    return true;
                default:
                    objType = default;
                    return false;
            }
        }
        private async Task<List<int>> GetSelectedObjectNumbersAsync(ObjType objType)
        {
            // TODO: Рассмотреть возможность выбирать объекты на сцене другим способом.
            // Необходима консультация с дизайнером
            SelectedObjects = Converters.ConvertObjTypeToSelectionType(objType);
            var selectedObjects = new List<int>();
            while (true)
            {
                var message = $@"Выберите {objType} и нажмите на клавишу ""E"" для подтверждения или клавишу ""ESC"" для завершения";

                var selectedObject = await SelectObjectAsync(objType, message);
                if (selectedObject is not IModelObject typedObject)
                    break;
                selectedObjects.Add(typedObject.Number);

                ResetObjectColor(objType);
            }

            return selectedObjects;
        }

        private void ResetObjectColor(ObjType objType)
        {
            var set = project?.GetModelSetsInfo(objType).FirstOrDefault();
            if (set == null)
                return;
            project.ClearSelection(objType);
        }
    }
}
