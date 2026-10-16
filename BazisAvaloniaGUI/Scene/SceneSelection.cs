using BazisGUI.Scene.Core;
using BazisGUI.Scene.EventsArgs;
using Geometry;
using Model.Interfaces;
using Model.Interfaces.ObjectsCollections;
using OperationalController;

namespace BazisAvaloniaGUI.Scene;

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
        var hits = new List<InfoObjectsEventArgs>();
        void CollectHit(object? sender, InfoObjectsEventArgs hit) => hits.Add(hit);

        var isPoint = !selection.IsSorted;
        scene.InfoRequested += CollectHit;
        try
        {
            if (isPoint)
            {
                var box = selection.SelectionBox;
                var point = new Point2D((box.Left + box.Right) / 2, (box.Bottom + box.Top) / 2);
                scene.SelectByPoint(sets, point, selection.IsSelected, project.GetVisible);
            }
            else
                scene.SelectByRect(sets, selection.SelectionBox, selection.IsSelected, project.GetVisible);
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
}
