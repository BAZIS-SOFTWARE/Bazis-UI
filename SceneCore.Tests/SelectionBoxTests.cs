using Geometry;
using System.Linq;
using SceneCore.Coordinates;
using RectangleBox = SceneCore.Picking.RectangleBox;

namespace SceneCore.Tests
{
    public class SelectionBoxTests
    {
        /// <summary>
        /// Проверяет выбор многоугольника прямоугольной рамкой.
        /// Принимает границы рамки и точки многоугольника.
        /// </summary>
        private void TestIsSelectedByRectangleBase(int left, int right, int bottom, int top, bool expected, params int[][] pointValues)
        {
            var creator = new Hull2DCreator();
            var polygon = creator.CreateHullGraham(pointValues.Select(x => new Point2D(x[0], x[1])));

            var points = polygon.GetPoints();
            var viewportPoints = points.Select(point => new ViewportPoint(point._x, point._y));
            var polygonBox = new RectangleBox(viewportPoints);
            var rectangleBox = new RectangleBox(left, right, bottom, top);

            Assert.That(IsSelectedByRectangle(polygonBox, rectangleBox), Is.EqualTo(expected));
        }

        /// <summary>Проверяет пересечение или вложенность рамок.</summary>
        private bool IsSelectedByRectangle(RectangleBox polygonBox, RectangleBox rectangle)
        {
            if (polygonBox.IsIntersectWithOther(rectangle))
                return true;
            if (polygonBox.IsInnerOther(rectangle))
                return true;

            return false;
        }

        [TestCase(1, 3, 2, 3, false, new[] { 0, 0 }, new[] { 1, 1 }, new[] { 1, 0 }, new[] { 0, 1 })]
        [TestCase(1, 2, -3, -1, false, new[] { 0, 0 }, new[] { 1, 1 }, new[] { 1, 0 }, new[] { 0, 1 })]
        [TestCase(-2, -1, -2, -1, false, new[] { 0, 0 }, new[] { 1, 1 }, new[] { 1, 0 }, new[] { 0, 1 })]
        [TestCase(-3, -1, 1, 3, false, new[] { 0, 0 }, new[] { 1, 1 }, new[] { 1, 0 }, new[] { 0, 1 })]
        public void SelectionBox_IsSelectedByRectangle_ShouldBeFalse_WhenEmptyIntersection(int left, int right, int bottom, int top, bool expected, params int[][] pointValues) =>
            TestIsSelectedByRectangleBase(left, right, bottom, top, expected, pointValues);

        [TestCase(new[] { -3, 0 }, new[] { 0, 2 }, new[] { -1, 6 }, new[] { -3, 7 }, new[] { -5, 4 })]
        [TestCase(new[] { 3, 0 }, new[] { 0, 4 }, new[] { 1, 6 }, new[] { 3, 7 }, new[] { 5, 4 })]
        [TestCase(new[] { 3, 0 }, new[] { 0, -4 }, new[] { 1, -6 }, new[] { 3, -7 }, new[] { 5, -4 })]
        [TestCase(new[] { -3, 0 }, new[] { 0, -4 }, new[] { -1, -6 }, new[] { -3, -7 }, new[] { -5, -4 })]
        [TestCase(new[] { -2, 3 }, new[] { -3, 4 }, new[] { -5, 2 }, new[] { -2, -7 }, new[] { -3, -4 })]
        [TestCase(new[] { -2, -3 }, new[] { -3, -4 }, new[] { -5, -2 }, new[] { -2, 7 }, new[] { -3, 4 })]
        [TestCase(new[] { 2, 3 }, new[] { 3, 4 }, new[] { 5, -2 }, new[] { 2, -7 }, new[] { 3, -4 })]
        [TestCase(new[] { 2, -3 }, new[] { 3, -4 }, new[] { 5, 2 }, new[] { 2, 7 }, new[] { 3, 4 })]
        public void SelectionBox_IsSelectedByRectangle_ShouldBeTrue_WhenOneRectanglePointInsidePolygon(params int[][] pointValues) => TestIsSelectedByRectangleBase(-2, 2, -3, 3, true, pointValues);

        [TestCase(new[] { -3, 2 }, new[] { -4, 6 }, new[] { 0, 8 }, new[] { 1, 7 }, new[] { 5, 4 }, new[] { 3, 2 })]
        [TestCase(new[] { 1, -4 }, new[] { 1, 4 }, new[] { 2, 6 }, new[] { 5, 0 }, new[] { -4, -7 })]
        [TestCase(new[] { -3, -2 }, new[] { -4, -6 }, new[] { 0, -8 }, new[] { 1, -7 }, new[] { 5, -4 }, new[] { 3, -2 })]
        [TestCase(new[] { -1, -4 }, new[] { -1, 4 }, new[] { -2, 6 }, new[] { -5, 0 }, new[] { -4, -7 })]
        [TestCase(new[] { -1, -4 }, new[] { 1, 4 }, new[] { 2, 6 }, new[] { 5, 0 }, new[] { 4, 7 })]
        public void SelectionBox_IsSelectedByRectangle_ShouldBeTrue_WhenTwoRectanglePointInsidePolygon(params int[][] pointValues) => TestIsSelectedByRectangleBase(-2, 2, -3, 3, true, pointValues);

        [TestCase(new[] { -4, -4 }, new[] { 2, 5 }, new[] { 5, 1 }, new[] { 3, -7 }, new[] { -3, -6 })]
        [TestCase(new[] { 4, -4 }, new[] { -2, 5 }, new[] { -5, 1 }, new[] { -3, -7 }, new[] { 3, -6 })]
        public void SelectionBox_IsSelectedByRectangle_ShouldBeTrue_WhenThreeRectanglePointInsidePolygon(params int[][] pointValues) => TestIsSelectedByRectangleBase(-2, 2, -3, 3, true, pointValues);

        [TestCase(new[] { 0, 0 }, new[] { 2, 2 }, new[] { 2, 0 }, new[] { 0, 2 })]
        [TestCase(new[] { -1, 0 }, new[] { 1, 2 }, new[] { 1, 0 })]
        [TestCase(
            new[] { -1, 0 }, new[] { 1, 2 }, new[] { 1, 1 },
            new[] { 2, 1 }, new[] { 0, 1 }, new[] { 0, 0 }
            )]
        [TestCase(new[] { 2, 2 }, new[] { 2, -2 }, new[] { -2, -2 }, new[] { -2, 2 })]
        public void SelectionBox_IsSelectedByRectangle_ShouldBeTrue_WhenRectangleInsidePolygon(params int[][] pointValues) => TestIsSelectedByRectangleBase(0, 1, 0, 1, true, pointValues);

        [TestCase(new[] { 0, 0 }, new[] { 1, 1 }, new[] { 1, 0 }, new[] { 0, 1 })]
        [TestCase(new[] { -5, 0 }, new[] { -2, 1 }, new[] { 1, 3 }, new[] { 2, -2 }, new[] { -4, -3 })]
        [TestCase(new[] { -5, 4 }, new[] { 0, 4 }, new[] { 0, 0 }, new[] { -5, 0 })]
        [TestCase(new[] { 0, 0 }, new[] { 0, 4 }, new[] { 5, 4 }, new[] { 5, 0 })]
        [TestCase(new[] { 0, 0 }, new[] { 5, 0 }, new[] { 5, -4 }, new[] { 0, -4 })]
        [TestCase(new[] { 0, 0 }, new[] { 0, -4 }, new[] { -5, -4 }, new[] { -5, 0 })]
        [TestCase(new[] { 0, 0 }, new[] { -5, 0 }, new[] { -5, 4 }, new[] { 0, 4 })]
        public void SelectionBox_IsSelectedByRectangle_ShouldBeTrue_WhenPolygonInsideRectangle(params int[][] pointValues) => TestIsSelectedByRectangleBase(-5, 5, -4, 4, true, pointValues);

        [TestCase(new[] { 0, 0 }, new[] { 2, 2 }, new[] { 2, 0 }, new[] { 0, 2 })]
        [TestCase(new[] { 3, 2 }, new[] { 2, 2 }, new[] { 2, 5 }, new[] { 1, 5 }, new[] { 1, 6 }, new[] { 3, 5 })]
        [TestCase(new[] { 3, 1 }, new[] { 3, 2 }, new[] { 5, 3 }, new[] { 5, 2 })]
        [TestCase(new[] { 2, 2 }, new[] { 3, 2 }, new[] { 3, -3 }, new[] { 2, -3 })]
        public void SelectionBox_IsSelectedByRectangle_ShouldBeTrue_WhenRectangleIntersectsWithPolygon(params int[][] pointValues) => TestIsSelectedByRectangleBase(1, 4, -1, 3, true, pointValues);

        [TestCase(-2, 2, -2, 2, true, new[] { -2, -2 }, new[] { -2, 2 }, new[] { 2, -2 }, new[] { 2, 2 })]
        [TestCase(-20, 25, -12, 10, true, new[] { -20, 10 }, new[] { -20, -12 }, new[] { 25, -12 }, new[] { 25, 10 })]
        public void SelectionBox_IsSelectedByRectangle_ShouldBeTrue_WhenPolygonEqualsRectangle(int left, int right, int bottom, int top, bool expected, params int[][] pointValues) =>
            TestIsSelectedByRectangleBase(left, right, bottom, top, expected, pointValues);

        [TestCase(new[] { 1, 1 }, new[] { 1, 0 }, new[] { 2, 1 }, new[] { 2, 0 })]
        [TestCase(new[] { -1, 0 }, new[] { -1, 1 }, new[] { 0, 0 }, new[] { 0, 1 })]
        [TestCase(new[] { 0, 1 }, new[] { 0, 2 }, new[] { 1, 1 }, new[] { 1, 2 })]
        [TestCase(new[] { 0, -1 }, new[] { 0, 0 }, new[] { 1, -1 }, new[] { 1, 0 })]
        public void SelectionBox_IsSelectedByRectangle_ShouldBeTrue_WhenRectangleHasCommonSideWithPolygon(params int[][] pointValues) => TestIsSelectedByRectangleBase(0, 1, 0, 1, true, pointValues);

        // Выпуклый восьмиугольник.
        [TestCase(
            new[] { -1, 4 }, new[] { -4, 1 }, new[] { -4, -1 }, new[] { -1, -4 },
            new[] { 1, -4 }, new[] { 4, -1 }, new[] { 4, 1 }, new[] { 1, 4 }
            )]
        // Ромб.
        [TestCase(new[] { 0, 4 }, new[] { -4, 0 }, new[] { 0, -4 }, new[] { 4, 0 })]
        // Шестиугольник.
        [TestCase(
            new[] { 0, 4 }, new[] { -3, 2 }, new[] { -3, -2 },
            new[] { 0, -4 }, new[] { 3, -2 }, new[] { 3, 2 }
            )]
        public void SelectionBox_IsSelectedByRectangle_ShouldBeTrue_WhenConvexPolygon(params int[][] pointsValues) => TestIsSelectedByRectangleBase(-4, 4, -4, 4, true, pointsValues);

    }
}
