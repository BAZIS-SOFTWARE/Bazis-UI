using BazisGUI.SceneCore;
using BazisGUI.SceneCore.EventsArgs;
using BazisGUI.SceneCore.Picking;
using BazisGUI.SceneCore.Coordinates;
using Geometry;
using Model.Interfaces;
using Model.Interfaces.ObjectsCollections;
using OperationalController;

namespace BazisGUI.Avalonia.Scene;

/// <summary>
/// Итог выбора на сцене: сколько объектов затронуто, был ли это выбор точкой
/// и какой объект выбран точкой (BaseForm.SelectByPoint берёт один объект).
/// </summary>
internal sealed record SceneSelectionResult(int Count, bool IsSelected, bool IsPoint, ObjType? Type, int Number);

internal class SceneSelection
{
    /// <summary>Применяет найденные на сцене объекты к выбору в представлении модели.</summary>
    public SceneSelectionResult Apply(IProjectController project, SceneController scene, ObjType? selectedType, SelectObjectsEventArgs selection)
    {
        var sourceSets = selectedType.HasValue
            ? project.GetModelSetsInfo(selectedType.Value)
            : project.GetAllModelSetsInfo();
        var sets = new List<ISetInfo>(sourceSets);
        var pickSets = GetPickSets(sets, project);
        var hits = new List<InfoObjectsEventArgs>();
        void CollectHit(object? sender, InfoObjectsEventArgs hit) => hits.Add(hit);

        var isPoint = !selection.IsSorted;
        scene.InfoRequested += CollectHit;
        try
        {
            if (isPoint)
            {
                var box = selection.SelectionBox;
                var point = new ViewportPoint((box.Left + box.Right) / 2.0, (box.Bottom + box.Top) / 2.0);
                scene.SelectByPoint(pickSets, point, selection.IsSelected);
            }
            else
                scene.SelectByRect(pickSets, selection.SelectionBox, selection.IsSelected);
        }
        finally
        {
            scene.InfoRequested -= CollectHit;
        }

        // BaseForm.SelectByPoint: из попаданий точкой берётся первый набор и последний номер в нём.
        if (isPoint)
        {
            var hit = hits.FirstOrDefault();
            var set = hit == null ? null : sets.Find(item => item.Name == hit.ObjsName);
            var numbers = hit?.GetObjectsIndexes().ToList();
            if (set == null || numbers.Count == 0)
                return new SceneSelectionResult(0, selection.IsSelected, true, null, 0);

            var number = numbers.Last();
            if (selection.IsSelected)
                project.Select(set.ObjType, [number]);
            else
                project.Deselect(set.ObjType, [number]);
            return new SceneSelectionResult(1, selection.IsSelected, true, set.ObjType, number);
        }

        var count = 0;
        using (project.BeginViewUpdate())
        {
            foreach (var hit in hits)
            {
                var set = sets.Find(item => item.Name == hit.ObjsName);
                if (set == null)
                    continue;

                var numbers = new List<int>(hit.GetObjectsIndexes());
                count += numbers.Count;
                if (selection.IsSelected)
                    project.Select(set.ObjType, numbers);
                else
                    project.Deselect(set.ObjType, numbers);
            }
        }
        return new SceneSelectionResult(count, selection.IsSelected, false, null, 0);
    }

    /// <summary>Лениво формирует наборы геометрических объектов для выбора на сцене.</summary>
    private IEnumerable<Set> GetPickSets(IEnumerable<ISetInfo> sourceSets, IProjectController project)
    {
        foreach (var sourceSet in sourceSets)
        {
            var kind = sourceSet.ObjType switch
            {
                ObjType.Кривая => ScenePickKind.Curve,
                ObjType.Поверхность => ScenePickKind.Surface,
                _ => ScenePickKind.Other
            };
            var objects = GetVisibleObjects(sourceSet, project);
            var name = sourceSet.Name;
            yield return new Set(name, kind, objects);
        }
    }

    /// <summary>Лениво перечисляет видимые объекты исходного набора.</summary>
    private IEnumerable<ScenePickObject> GetVisibleObjects(ISetInfo sourceSet, IProjectController project)
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
