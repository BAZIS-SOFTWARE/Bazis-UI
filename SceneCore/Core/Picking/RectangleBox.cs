using System;
using System.Collections.Generic;
using System.Linq;
using SceneCore.Coordinates;

namespace SceneCore.Picking
{
    /// <summary>Описывает прямоугольник выбора объектов в экранных координатах.</summary>
    public class RectangleBox
    {
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
    }
}
