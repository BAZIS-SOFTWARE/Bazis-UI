using BazisGUI.SceneCore.Coordinates;

namespace BazisGUI.SceneCore.Tests
{
    [TestFixture]
    public class SceneGeometryPrecisionTests
    {
        /// <summary>Проверяет длины сцены для малых отрезков вдали от начала координат.</summary>
        [Test]
        public void BoundingBoxAndPathPreserveSmallWorldDistances()
        {
            var first = new ModelPoint3D(1000000000.125, 0, 0);
            var second = new ModelPoint3D(1000000000.375, 0, 0);
            var third = new ModelPoint3D(1000000000.5, 0, 0);
            var bounds = new BoundingBox(first, third);
            var pathPoints = new[]
            {
                first,
                second,
                third
            };
            var path = new ScenePath(pathPoints);

            Assert.That(bounds.GetSqrCoordsSum(), Is.EqualTo(0.140625).Within(1e-12));
            Assert.That(bounds.GetDiagonalLength(), Is.EqualTo(0.375).Within(1e-12));
            Assert.That(path.Length, Is.EqualTo(0.375).Within(1e-12));
        }

        /// <summary>Проверяет объединение ограничивающих боксов без изменения исходных координат.</summary>
        [Test]
        public void BoundingBoxMergePreservesSourceBounds()
        {
            var first = new BoundingBox(new ModelPoint3D(-1, 2, 3), new ModelPoint3D(2, -2, -3));
            var second = new BoundingBox(new ModelPoint3D(-4, 5, 1), new ModelPoint3D(3, -1, -6));

            var merged = first.Merge(second);

            Assert.That(merged.LeftUpNear, Is.EqualTo(new ModelPoint3D(-4, 5, 3)));
            Assert.That(merged.RightDownFar, Is.EqualTo(new ModelPoint3D(3, -2, -6)));
            Assert.That(first.LeftUpNear, Is.EqualTo(new ModelPoint3D(-1, 2, 3)));
            Assert.That(first.RightDownFar, Is.EqualTo(new ModelPoint3D(2, -2, -3)));
        }
    }
}
