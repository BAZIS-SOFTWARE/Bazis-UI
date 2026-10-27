using System;
using System.Collections.Generic;
using System.Linq;
using BazisGUI.SceneCore.Interfaces;
using BazisGUI.SceneCore.Coordinates;
using Geometry;

namespace BazisGUI.SceneCore.Picking
{
    /// <summary>
    /// Перенос BaseForm.SelectByPoint/SelectByRect почти без правок в геометрии отбора
    /// (см. scene.avalonia.md, раздел 6). Из него убраны побочные эффекты — console.PrintInfo,
    /// propertiesPanel.DrawTable, project.ModelView.Select/Deselect, DispatchSelection —
    /// ScenePicker не знает о "project"/IModelView. Вместо них он сообщает о найденных
    /// объектах через ObjectsHit; SceneController превращает это в события SelectionChanged
    /// и InfoRequested, а фактический выбор в модели выполняет вызывающий код.
    /// </summary>
    public class ScenePicker
    {
        private readonly ISceneCamera camera;

        public event Action<string, IEnumerable<int>> ObjectsHit;

        public ScenePicker(ISceneCamera camera)
        {
            this.camera = camera;
        }

        public bool SelectByPoint(IEnumerable<Set> sets, ViewportPoint selectionPoint, bool isSelected)
        {
            var selFlag = false;
            var tempNumbs = new List<int>();
            Set tempSetInfo = null;
            var curZDepth = 0.0;

            foreach (var set in sets)
            {
                foreach (var item in set.Objects)
                {
                    var scrPoints = new List<ViewportPoint>();
                    var scnPoints = new List<CameraPoint3D>();

                    foreach (var point in item.Points)
                    {
                        var scnPoint = camera.GetSceneCoord(point);
                        scnPoints.Add(scnPoint);
                        scrPoints.Add(camera.GetScreenCoord(scnPoint));
                    }

                    if (!IsObjectSelected(selectionPoint, set.Kind, scrPoints))
                        continue;

                    selFlag = true;
                    var tempZDepth = scnPoints.Count == 1
                        ? scnPoints[0].Z
                        : scnPoints.Sum(x => x.Z) / scnPoints.Count;

                    var isObjectCloser = curZDepth == 0 || tempZDepth > curZDepth;
                    if (isObjectCloser)
                    {
                        tempNumbs.Add(item.Number);
                        tempSetInfo = set;
                        curZDepth = tempZDepth;
                    }
                    else
                    {
                        tempNumbs.Insert(0, item.Number);
                    }
                }
            }

            if (selFlag)
                ObjectsHit?.Invoke(tempSetInfo.Name, new[] { tempNumbs.Last() });

            return selFlag;
        }

        public void SelectByRect(IEnumerable<Set> sets, RectangleBox selectionBox, bool isSelected)
        {
            var creator = new Hull2DCreator();
            var selectedNumbers = new Dictionary<Set, List<int>>();

            foreach (var set in sets)
            {
                foreach (var item in set.Objects)
                {
                    var scrPoints = new HashSet<ViewportPoint>();
                    foreach (var point in item.Points)
                    {
                        var scenePoint = camera.GetSceneCoord(point);
                        var screenPoint = camera.GetScreenCoord(scenePoint);
                        scrPoints.Add(screenPoint);
                    }

                    if (!CheckSelection(selectionBox, creator, scrPoints))
                        continue;

                    if (!selectedNumbers.TryGetValue(set, out var numbers))
                    {
                        numbers = new List<int>();
                        selectedNumbers.Add(set, numbers);
                    }
                    numbers.Add(item.Number);
                }
            }

            foreach (var pair in selectedNumbers)
                ObjectsHit?.Invoke(pair.Key.Name, pair.Value);
        }

        private static bool IsObjectSelected(ViewportPoint selectionPoint, ScenePickKind kind, List<ViewportPoint> scrPoints)
        {
            if (kind == ScenePickKind.Curve)
                return IsCurveSelected(selectionPoint, scrPoints);
            if (kind == ScenePickKind.Surface)
                return IsSurfaceSelected(selectionPoint, scrPoints);
            return IsObjectSelected(selectionPoint, scrPoints);
        }

        private static bool IsSurfaceSelected(ViewportPoint selectionPoint, List<ViewportPoint> scrPoints)
        {
            var rect = new RectangleBox(scrPoints);
            if (!rect.IsPointInside(selectionPoint))
                return false;

            var creator = new Hull2DCreator();
            var geometrySelectionPoint = new Point2D(selectionPoint.X, selectionPoint.Y);
            var count = scrPoints.Count / 3;
            for (var i = 0; i < count; i++)
            {
                var triangle = new[]
                {
                    new Point2D(scrPoints[3 * i + 0].X, scrPoints[3 * i + 0].Y),
                    new Point2D(scrPoints[3 * i + 1].X, scrPoints[3 * i + 1].Y),
                    new Point2D(scrPoints[3 * i + 2].X, scrPoints[3 * i + 2].Y)
                };
                if (creator.TryCreateHullGraham(triangle, out var polygon) && polygon.IsPointInsidePolygon(geometrySelectionPoint))
                    return true;
            }
            return false;
        }

        private static bool IsCurveSelected(ViewportPoint selectionPoint, List<ViewportPoint> scrPoints)
        {
            var rect = new RectangleBox(scrPoints);
            if (!rect.IsPointInside(selectionPoint))
                return false;

            var geometrySelectionPoint = new Point2D(selectionPoint.X, selectionPoint.Y);
            if (scrPoints.Count == 2)
            {
                var start = new Point2D(scrPoints[0].X, scrPoints[0].Y);
                var end = new Point2D(scrPoints[1].X, scrPoints[1].Y);
                var segment = new Segment2D(start, end);
                return segment.IsPointBelongSegment(geometrySelectionPoint, 5);
            }

            var count = scrPoints.Count / 2;
            for (var i = 0; i < count; i++)
            {
                var start = new Point2D(scrPoints[2 * i + 0].X, scrPoints[2 * i + 0].Y);
                var end = new Point2D(scrPoints[2 * i + 1].X, scrPoints[2 * i + 1].Y);
                var seg = new Segment2D(start, end);
                if (seg.IsPointBelongSegment(geometrySelectionPoint, 5))
                    return true;
            }
            return false;
        }

        private static bool IsObjectSelected(ViewportPoint selectionPoint, List<ViewportPoint> scrPoints)
        {
            if (scrPoints.Count == 1)
            {
                return scrPoints[0].X > selectionPoint.X - 10 && scrPoints[0].X < selectionPoint.X + 5
                    && scrPoints[0].Y > selectionPoint.Y - 5 && scrPoints[0].Y < selectionPoint.Y + 5;
            }

            var rect = new RectangleBox(scrPoints);
            if (!rect.IsPointInside(selectionPoint))
                return false;

            var creator = new Hull2DCreator();
            var geometryPoints = scrPoints.Select(point => new Point2D(point.X, point.Y)).ToArray();
            var geometrySelectionPoint = new Point2D(selectionPoint.X, selectionPoint.Y);
            return creator.TryCreateHullGraham(geometryPoints, out var polygon) && polygon.IsPointInsidePolygon(geometrySelectionPoint);
        }

        private static bool CheckSelection(RectangleBox selectionBox, Hull2DCreator creator, HashSet<ViewportPoint> scrPoints)
        {
            if (scrPoints.Count == 1)
                return selectionBox.IsPointInside(scrPoints.First());

            if (scrPoints.Count == 2)
                return scrPoints.Any(selectionBox.IsPointInside);

            var polygonBox = new RectangleBox(scrPoints);
            if (polygonBox.IsInnerOther(selectionBox))
                return true;

            if (!polygonBox.IsIntersectWithOther(selectionBox))
                return false;

            if (selectionBox.Left <= polygonBox.Left && selectionBox.Right >= polygonBox.Right)
                return true;
            if (selectionBox.Bottom <= polygonBox.Bottom && selectionBox.Top >= polygonBox.Top)
                return true;

            var geometryPoints = scrPoints.Select(point => new Point2D(point.X, point.Y)).ToArray();
            if (creator.TryCreateHullGraham(geometryPoints, out var polygon))
            {
                var rectanglePoints = selectionBox.GetPoints();
                var geometryRectanglePoints = rectanglePoints.Select(point => new Point2D(point.X, point.Y));
                if (geometryRectanglePoints.Any(p => polygon.IsPointInsidePolygon(p)))
                    return true;
                if (polygon.GetPoints().Any(point => selectionBox.IsPointInside(new ViewportPoint(point._x, point._y))))
                    return true;
            }
            return false;
        }
    }
}
