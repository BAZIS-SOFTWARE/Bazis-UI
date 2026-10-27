using System;
using System.Collections.Generic;
using System.Linq;
using Geometry;
using Model.Interfaces;
using Model.Interfaces.MeshObjects;
using Model.Interfaces.ObjectsCollections;
using BazisGUI.SceneCore.Coordinates;
using BazisGUI.SceneCore.Picking;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        /// <summary>Лениво формирует наборы геометрических объектов для выбора на сцене.</summary>
        private IEnumerable<Set> GetPickSets(IEnumerable<ISetInfo> sourceSets)
        {
            var elements2D = new Lazy<Dictionary<int, ISurfaceElement>>(() => project.GetModelSurfaceElements(2).ToDictionary(element => element.Number));
            var elements3D = new Lazy<Dictionary<int, ISurfaceElement>>(() => project.GetModelSurfaceElements(3).ToDictionary(element => element.Number));

            foreach (var sourceSet in sourceSets)
            {
                var kind = sourceSet.ObjType switch
                {
                    ObjType.Кривая or ObjType.Элемент1D => ScenePickKind.Curve,
                    ObjType.Поверхность or ObjType.Элемент2D or ObjType.Элемент3D => ScenePickKind.Surface,
                    _ => ScenePickKind.Other
                };
                var elementByNumber = sourceSet.ObjType switch
                {
                    ObjType.Элемент2D => elements2D.Value,
                    ObjType.Элемент3D => elements3D.Value,
                    _ => null
                };
                var objects = GetVisiblePickObjects(sourceSet, elementByNumber);
                var name = sourceSet.Name;
                yield return new Set(name, kind, objects);
            }
        }

        /// <summary>Лениво перечисляет видимые объекты набора с подготовленными примитивами выбора.</summary>
        private IEnumerable<ScenePickObject> GetVisiblePickObjects(ISetInfo sourceSet, IReadOnlyDictionary<int, ISurfaceElement> elementByNumber)
        {
            foreach (var number in sourceSet.GetNumbers())
            {
                if (!project.GetVisible(sourceSet.ObjType, number))
                    continue;

                var points = sourceSet.ObjType switch
                {
                    ObjType.Поверхность => GetCadSurfacePoints(number),
                    ObjType.Элемент2D or ObjType.Элемент3D => GetElementSurfacePoints(elementByNumber[number]),
                    _ => sourceSet.GetCoords(number)
                };
                var modelPoints = points.Select(point => new ModelPoint3D(point._x, point._y, point._z));
                yield return new ScenePickObject(number, modelPoints);
            }
        }

        /// <summary>Лениво триангулирует грани CAD-поверхности в порядке отображения.</summary>
        private IEnumerable<Point3D> GetCadSurfacePoints(int number)
        {
            var figure = project.GetModelSurface(number);
            foreach (var surface in figure.GetSurfaces())
            {
                var first = surface[0].Position;
                for (var index = 1; index < surface.NumberOfPoints - 1; index++)
                {
                    yield return first;
                    yield return surface[index].Position;
                    yield return surface[index + 1].Position;
                }
            }
        }

        /// <summary>Лениво триангулирует видимые выпуклые грани конечного элемента.</summary>
        private IEnumerable<Point3D> GetElementSurfacePoints(ISurfaceElement element)
        {
            foreach (var surface in element.GetSurfaces())
            {
                if (project.HideInsideSurfaces && surface.IsInner)
                {
                    var visibleNeighbour = surface.GetElements().Any(owner => owner.Number != element.Number && project.GetVisible(owner.ObjType, owner.Number));
                    if (visibleNeighbour)
                        continue;
                }

                var first = surface[0].Position;
                for (var index = 1; index < surface.NumberOfPoints - 1; index++)
                {
                    yield return first;
                    yield return surface[index].Position;
                    yield return surface[index + 1].Position;
                }
            }
        }
    }
}
