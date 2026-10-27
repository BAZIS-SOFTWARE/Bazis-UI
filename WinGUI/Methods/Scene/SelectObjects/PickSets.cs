using System.Collections.Generic;
using System.Linq;
using Model.Interfaces;
using Model.Interfaces.ObjectsCollections;
using SceneCore.Coordinates;
using SceneCore.Picking;

namespace WinGUI
{
    public partial class BaseForm
    {
        /// <summary>Лениво формирует наборы геометрических объектов для выбора на сцене.</summary>
        private IEnumerable<Set> GetPickSets(IEnumerable<ISetInfo> sourceSets)
        {
            foreach (var sourceSet in sourceSets)
            {
                var kind = sourceSet.ObjType switch
                {
                    ObjType.Кривая => ScenePickKind.Curve,
                    ObjType.Поверхность => ScenePickKind.Surface,
                    _ => ScenePickKind.Other
                };
                var objects = GetVisiblePickObjects(sourceSet);
                var name = sourceSet.Name;
                yield return new Set(name, kind, objects);
            }
        }

        /// <summary>Лениво перечисляет видимые объекты набора без копирования координат.</summary>
        private IEnumerable<ScenePickObject> GetVisiblePickObjects(ISetInfo sourceSet)
        {
            foreach (var number in sourceSet.GetNumbers())
            {
                if (!project.GetVisible(sourceSet.ObjType, number))
                    continue;

                var points = sourceSet.GetCoords(number);
                var modelPoints = points.Select(point => new ModelPoint3D(point._x, point._y, point._z));
                yield return new ScenePickObject(number, modelPoints);
            }
        }
    }
}
