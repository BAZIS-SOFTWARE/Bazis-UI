using Geometry;

namespace BazisGUI.SceneCore.Tests
{
    [TestFixture]
    public class SceneGeometryPrecisionTests
    {
        /// <summary>Проверяет длины сцены для малых отрезков вдали от начала координат.</summary>
        [Test]
        public void BoundingBoxAndPathPreserveSmallWorldDistances()
        {
            var first = new Point3D(1000000000.125, 0, 0);
            var second = new Point3D(1000000000.375, 0, 0);
            var third = new Point3D(1000000000.5, 0, 0);
            var bounds = new BoundingBox(first, third);
            var path = new ScenePath(new[] { first, second, third });

            Assert.That(bounds.GetSqrCoordsSum(), Is.EqualTo(0.140625).Within(1e-12));
            Assert.That(bounds.GetDiagonalLength(), Is.EqualTo(0.375).Within(1e-12));
            Assert.That(path.Length, Is.EqualTo(0.375).Within(1e-12));
        }
    }
}
