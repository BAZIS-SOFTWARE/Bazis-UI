using System;
using System.Collections.Generic;
using System.Linq;
using SceneCore.Interfaces;
using Geometry;

namespace SceneCore.Picking
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

        public bool SelectByPoint(IEnumerable<Set> sets, Point2D selectionPoint, bool isSelected)
        {
            var selFlag = false;
            var tempNumbs = new List<int>();
            Set tempSetInfo = null;
            var curZDepth = 0.0f;

            foreach (var set in sets)
            {
                foreach (var item in set.Objects)
                {
                    var scrPoints = new List<Point2D>();
                    var scnPoints = new List<Point3D>();

                    foreach (var point in item.Points)
                    {
                        var scnPoint = camera.GetSceenCoord(point);
                        scnPoints.Add(scnPoint);
                        scrPoints.Add(camera.GetScreenCoord(scnPoint));
                    }

                    if (!IsObjectSelected(selectionPoint, set.Kind, scrPoints))
                        continue;

                    selFlag = true;
                    var tempZDepth = scnPoints.Count == 1
                        ? scnPoints[0]._z
                        : scnPoints.Sum(x => x._z) / scnPoints.Count;

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
                    var scrPoints = new HashSet<Point2D>();
                    foreach (var point in item.Points)
                        scrPoints.Add(camera.GetScreenCoord(camera.GetSceenCoord(point)));

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

        private static bool IsObjectSelected(Point2D selectionPoint, ScenePickKind kind, List<Point2D> scrPoints)
        {
            if (kind == ScenePickKind.Curve)
                return IsCurveSelected(selectionPoint, scrPoints);
            if (kind == ScenePickKind.Surface)
                return IsSurfaceSelected(selectionPoint, scrPoints);
            return IsObjectSelected(selectionPoint, scrPoints);
        }

        private static bool IsSurfaceSelected(Point2D selectionPoint, List<Point2D> scrPoints)
        {
            var rect = new RectangleBox(scrPoints);
            if (!rect.IsPointInside(selectionPoint))
                return false;

            var creator = new Hull2DCreator();
            var count = scrPoints.Count / 3;
            for (var i = 0; i < count; i++)
            {
                var triangle = new[] { scrPoints[3 * i + 0], scrPoints[3 * i + 1], scrPoints[3 * i + 2] };
                if (creator.TryCreateHullGraham(triangle, out var polygon) && polygon.IsPointInsidePolygon(selectionPoint))
                    return true;
            }
            return false;
        }

        private static bool IsCurveSelected(Point2D selectionPoint, List<Point2D> scrPoints)
        {
            var rect = new RectangleBox(scrPoints);
            if (!rect.IsPointInside(selectionPoint))
                return false;

            if (scrPoints.Count == 2)
                return new Segment2D(scrPoints[0], scrPoints[1]).IsPointBelongSegment(selectionPoint, 5);

            var count = scrPoints.Count / 2;
            for (var i = 0; i < count; i++)
            {
                var seg = new Segment2D(scrPoints[2 * i + 0], scrPoints[2 * i + 1]);
                if (seg.IsPointBelongSegment(selectionPoint, 5))
                    return true;
            }
            return false;
        }

        private static bool IsObjectSelected(Point2D selectionPoint, List<Point2D> scrPoints)
        {
            if (scrPoints.Count == 1)
            {
                return scrPoints[0]._x > selectionPoint._x - 10 && scrPoints[0]._x < selectionPoint._x + 5
                    && scrPoints[0]._y > selectionPoint._y - 5 && scrPoints[0]._y < selectionPoint._y + 5;
            }

            var rect = new RectangleBox(scrPoints);
            if (!rect.IsPointInside(selectionPoint))
                return false;

            var creator = new Hull2DCreator();
            return creator.TryCreateHullGraham(scrPoints, out var polygon) && polygon.IsPointInsidePolygon(selectionPoint);
        }

        private static bool CheckSelection(RectangleBox selectionBox, Hull2DCreator creator, HashSet<Point2D> scrPoints)
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

            if (creator.TryCreateHullGraham(scrPoints, out var polygon))
            {
                var rectanglePoints = selectionBox.GetPoints();
                if (rectanglePoints.Any(p => polygon.IsPointInsidePolygon(p)))
                    return true;
                if (polygon.GetPoints().Any(selectionBox.IsPointInside))
                    return true;
            }
            return false;
        }
    }
}
