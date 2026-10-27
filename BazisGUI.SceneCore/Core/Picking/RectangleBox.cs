using System;
using System.Collections.Generic;
using System.Linq;
using BazisGUI.SceneCore.Coordinates;

namespace BazisGUI.SceneCore.Picking
{
    /// <summary>Описывает прямоугольник выбора объектов в экранных координатах.</summary>
    public class RectangleBox
    {
        private const double IntersectionTolerance = 1e-7;

        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }

        /// <summary>Создаёт рамку по набору экранных точек.</summary>
        public RectangleBox(IEnumerable<ViewportPoint> points)
        {
            if (points.Count() < 2)
                throw new ArgumentException("At least two points are required to create a rectangle.");
            Right = (int)points.Select(p => p.X).Max();
            Left = (int)points.Select(p => p.X).Min();

            Top = (int)points.Select(p => p.Y).Max();
            Bottom = (int)points.Select(p => p.Y).Min();
        }

        /// <summary>Создаёт рамку по координатам сторон и упорядочивает её границы.</summary>
        public RectangleBox(int left, int right, int bottom, int top)
        {
            Right = right;
            Left = left;

            if (left > right)
            {
                Right = left;
                Left = right;
            }

            Bottom = bottom;
            Top = top;

            if (bottom > top)
            {
                Bottom = top;
                Top = bottom;
            }
        }

        /// <summary>Проверяет пересечение с другой рамкой.</summary>
        public bool IsIntersectWithOther(RectangleBox other)
        {
            return Left <= other.Right && Right >= other.Left &&
                   Top >= other.Bottom && Bottom <= other.Top;
        }

        /// <summary>Проверяет, находится ли рамка внутри другой рамки.</summary>
        public bool IsInnerOther(RectangleBox other)
        {
            if (other.Left <= Left && Right <= other.Right &&
                other.Top >= Top && Bottom >= other.Bottom)
                return true;
            return false;
        }

        /// <summary>Возвращает углы рамки в экранных координатах.</summary>
        public ViewportPoint[] GetPoints()
        {
            return new[]
            {
                new ViewportPoint(Left, Top), new ViewportPoint(Left, Bottom),
                new ViewportPoint(Right, Bottom), new ViewportPoint(Right, Top)
            };
        }

        /// <summary>Проверяет попадание всех точек внутрь рамки с существующим допуском выбора.</summary>
        public bool IsPointsInside(ViewportPoint[] points)
        {
            var counter = 0;
            for (var i = 0; i < points.Length; i++)
            {
                if (points[i].X > Left - 5 &&
                    points[i].X < Right &&
                    points[i].Y > Bottom &&
                    points[i].Y < Top + 5)
                {
                    counter++;
                }
            }

            if (counter == points.Length)
                return true;
            else
                return false;
        }

        /// <summary>Проверяет попадание точки внутрь рамки с существующим допуском выбора.</summary>
        public bool IsPointInside(ViewportPoint point)
        {
            if (point.X > Left - 5 &&
                point.X < Right &&
                point.Y > Bottom &&
                point.Y < Top + 5)
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>Проверяет пересечение замкнутой рамки с отрезком.</summary>
        internal bool IntersectsSegment(ViewportPoint first, ViewportPoint second)
        {
            if (Contains(first) && Contains(second))
                return true;

            var corners = GetPoints();
            for (var index = 0; index < corners.Length; index++)
            {
                var next = (index + 1) % corners.Length;
                if (SegmentsIntersect(first, second, corners[index], corners[next]))
                    return true;
            }
            return false;
        }

        /// <summary>Проверяет пересечение замкнутой рамки с одним треугольником.</summary>
        internal bool IntersectsTriangle(ViewportTriangle triangle)
        {
            var area = (triangle.Second.X - triangle.First.X) * (triangle.Third.Y - triangle.First.Y) -
                       (triangle.Second.Y - triangle.First.Y) * (triangle.Third.X - triangle.First.X);
            if (Math.Abs(area) < 1e-9)
                return false;

            if (Contains(triangle.First) && Contains(triangle.Second) && Contains(triangle.Third))
                return true;

            if (IntersectsSegment(triangle.First, triangle.Second) ||
                IntersectsSegment(triangle.Second, triangle.Third) ||
                IntersectsSegment(triangle.Third, triangle.First))
                return true;

            var corners = GetPoints();
            foreach (var corner in corners)
            {
                if (!triangle.Contains(corner))
                    return false;
            }
            return true;
        }

        /// <summary>Проверяет попадание точки в замкнутую рамку с малым численным допуском.</summary>
        internal bool Contains(ViewportPoint point) => point.X >= Left - IntersectionTolerance && point.X <= Right + IntersectionTolerance &&
                                                     point.Y >= Bottom - IntersectionTolerance && point.Y <= Top + IntersectionTolerance;

        /// <summary>Проверяет пересечение двух замкнутых отрезков, включая касание и наложение.</summary>
        private bool SegmentsIntersect(ViewportPoint first, ViewportPoint second, ViewportPoint third, ViewportPoint fourth)
        {
            var firstX = second.X - first.X;
            var firstY = second.Y - first.Y;
            var secondX = fourth.X - third.X;
            var secondY = fourth.Y - third.Y;
            var denominator = firstX * secondY - firstY * secondX;
            var offsetX = third.X - first.X;
            var offsetY = third.Y - first.Y;

            if (Math.Abs(denominator) < 1e-9)
            {
                var cross = offsetX * firstY - offsetY * firstX;
                if (Math.Abs(cross) >= 1e-9)
                    return false;
                return Math.Max(first.X, second.X) + IntersectionTolerance >= Math.Min(third.X, fourth.X) &&
                       Math.Max(third.X, fourth.X) + IntersectionTolerance >= Math.Min(first.X, second.X) &&
                       Math.Max(first.Y, second.Y) + IntersectionTolerance >= Math.Min(third.Y, fourth.Y) &&
                       Math.Max(third.Y, fourth.Y) + IntersectionTolerance >= Math.Min(first.Y, second.Y);
            }

            var firstParameter = (offsetX * secondY - offsetY * secondX) / denominator;
            var secondParameter = (offsetX * firstY - offsetY * firstX) / denominator;
            return firstParameter >= -IntersectionTolerance && firstParameter <= 1 + IntersectionTolerance &&
                   secondParameter >= -IntersectionTolerance && secondParameter <= 1 + IntersectionTolerance;
        }
    }
}
