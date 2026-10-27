using System;
using BazisGUI.SceneCore.Coordinates;

namespace BazisGUI.SceneCore.Picking
{
    /// <summary>Треугольник в экранных координатах для проверки выбора.</summary>
    internal readonly struct ViewportTriangle
    {
        public ViewportTriangle(ViewportPoint first, ViewportPoint second, ViewportPoint third)
        {
            First = first;
            Second = second;
            Third = third;
        }

        public ViewportPoint First { get; }
        public ViewportPoint Second { get; }
        public ViewportPoint Third { get; }

        /// <summary>Проверяет принадлежность точки невырожденному треугольнику вместе с границей.</summary>
        public bool Contains(ViewportPoint point)
        {
            var area = (Second.X - First.X) * (Third.Y - First.Y) - (Second.Y - First.Y) * (Third.X - First.X);
            if (Math.Abs(area) < 1e-9)
                return false;

            var firstSide = (Second.X - First.X) * (point.Y - First.Y) - (Second.Y - First.Y) * (point.X - First.X);
            var secondSide = (Third.X - Second.X) * (point.Y - Second.Y) - (Third.Y - Second.Y) * (point.X - Second.X);
            var thirdSide = (First.X - Third.X) * (point.Y - Third.Y) - (First.Y - Third.Y) * (point.X - Third.X);
            var hasNegative = firstSide < -1e-9 || secondSide < -1e-9 || thirdSide < -1e-9;
            var hasPositive = firstSide > 1e-9 || secondSide > 1e-9 || thirdSide > 1e-9;
            return !hasNegative || !hasPositive;
        }
    }
}
