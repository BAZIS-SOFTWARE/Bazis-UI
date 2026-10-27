using Model.Interfaces.ObjectsCollections;
using Model.Interfaces;
using System.Collections.Generic;
using BazisGUI.Win.Extensions;
using BazisGUI.Win.Utilities;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        public IEnumerable<ISetInfo> GetModelSetsInfo(SelectionType objects)
        {
            if (objects == SelectionType.Objects)
            {
                foreach (var item in project.GetModelSetsInfo(ObjType.Точка))
                    yield return item;
                foreach (var item in project.GetModelSetsInfo(ObjType.Кривая))
                    yield return item;
                foreach (var item in project.GetModelSetsInfo(ObjType.Поверхность))
                    yield return item;
                foreach (var item in project.GetModelSetsInfo(ObjType.Узел))
                    yield return item;
                foreach (var item in project.GetModelSetsInfo(ObjType.Элемент1D))
                    yield return item;
                foreach (var item in project.GetModelSetsInfo(ObjType.Элемент2D))
                    yield return item;
                foreach (var item in project.GetModelSetsInfo(ObjType.Элемент3D))
                    yield return item;

            }
            else if (objects == SelectionType.Elements)
            {
                foreach (var item in project.GetModelSetsInfo(ObjType.Элемент1D))
                    yield return item;
                foreach (var item in project.GetModelSetsInfo(ObjType.Элемент2D))
                    yield return item;
                foreach (var item in project.GetModelSetsInfo(ObjType.Элемент3D))
                    yield return item;
            }

            else if (objects == SelectionType.Figures)
            {
                foreach (var item in project.GetModelSetsInfo(ObjType.Точка))
                    yield return item;
                foreach (var item in project.GetModelSetsInfo(ObjType.Кривая))
                    yield return item;
                foreach (var item in project.GetModelSetsInfo(ObjType.Поверхность))
                    yield return item;
            }
            else
            {
                var objType = Converters.ConvertSelectionTypeToObjType(objects);
                foreach (var item in project.GetModelSetsInfo(objType))
                    yield return item;
            }
        }
    }
}
