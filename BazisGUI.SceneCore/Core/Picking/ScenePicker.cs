using System;
using System.Collections.Generic;
using System.Linq;
using BazisGUI.SceneCore.Interfaces;
using BazisGUI.SceneCore.Coordinates;

namespace BazisGUI.SceneCore.Picking
{
    /// <summary>
    /// Выбирает объекты сцены по экранным точкам, сегментам и треугольникам.
    /// О найденных объектах сообщает через ObjectsHit; выбор в модели выполняет вызывающий код.
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
            var selectedNumbers = new Dictionary<Set, List<int>>();

            foreach (var set in sets)
            {
                foreach (var item in set.Objects)
                {
                    var scrPoints = new List<ViewportPoint>();
                    foreach (var point in item.Points)
                    {
                        var scenePoint = camera.GetSceneCoord(point);
                        var screenPoint = camera.GetScreenCoord(scenePoint);
                        scrPoints.Add(screenPoint);
                    }

                    if (!CheckSelection(selectionBox, set.Kind, scrPoints))
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
            if (scrPoints.Count % 3 != 0)
                throw new ArgumentException("Surface points must form complete triangles.", nameof(scrPoints));

            var count = scrPoints.Count / 3;
            for (var i = 0; i < count; i++)
            {
                var triangle = new ViewportTriangle(scrPoints[3 * i], scrPoints[3 * i + 1], scrPoints[3 * i + 2]);
                if (triangle.Contains(selectionPoint))
                    return true;
            }
            return false;
        }

        private static bool IsCurveSelected(ViewportPoint selectionPoint, List<ViewportPoint> scrPoints)
        {
            for (var i = 1; i < scrPoints.Count; i++)
            {
                var start = scrPoints[i - 1];
                var end = scrPoints[i];
                var deltaX = end.X - start.X;
                var deltaY = end.Y - start.Y;
                var lengthSquared = deltaX * deltaX + deltaY * deltaY;
                var projection = lengthSquared == 0 ? 0 : ((selectionPoint.X - start.X) * deltaX + (selectionPoint.Y - start.Y) * deltaY) / lengthSquared;
                var fraction = Math.Clamp(projection, 0, 1);
                var nearestX = start.X + fraction * deltaX;
                var nearestY = start.Y + fraction * deltaY;
                var distanceX = selectionPoint.X - nearestX;
                var distanceY = selectionPoint.Y - nearestY;
                if (distanceX * distanceX + distanceY * distanceY <= 25)
                    return true;
            }
            return false;
        }

        private static bool IsObjectSelected(ViewportPoint selectionPoint, List<ViewportPoint> scrPoints)
        {
            if (scrPoints.Count != 1)
                return false;
            return scrPoints[0].X > selectionPoint.X - 10 && scrPoints[0].X < selectionPoint.X + 5
                && scrPoints[0].Y > selectionPoint.Y - 5 && scrPoints[0].Y < selectionPoint.Y + 5;
        }

        /// <summary>Проверяет примитивы объекта по очереди до первого попадания в рамку.</summary>
        private static bool CheckSelection(RectangleBox selectionBox, ScenePickKind kind, List<ViewportPoint> scrPoints)
        {
            if (kind == ScenePickKind.Curve)
            {
                for (var i = 1; i < scrPoints.Count; i++)
                {
                    if (selectionBox.IntersectsSegment(scrPoints[i - 1], scrPoints[i]))
                        return true;
                }
                return false;
            }

            if (kind == ScenePickKind.Surface)
            {
                if (scrPoints.Count % 3 != 0)
                    throw new ArgumentException("Surface points must form complete triangles.", nameof(scrPoints));

                for (var i = 0; i < scrPoints.Count; i += 3)
                {
                    var triangle = new ViewportTriangle(scrPoints[i], scrPoints[i + 1], scrPoints[i + 2]);
                    if (selectionBox.IntersectsTriangle(triangle))
                        return true;
                }
                return false;
            }

            return scrPoints.Count == 1 && selectionBox.Contains(scrPoints[0]);
        }
    }
}
