using System;
using BazisGUI.SceneCore.Camera;
using BazisGUI.SceneCore.Interfaces;

namespace BazisGUI.SceneCore.Tests
{
    /// <summary>
    /// Матрица проекции считается в коде (CameraProjection), а не через glu32.gluPerspective —
    /// значения сверяются с формулой из scene.avalonia.md/BaseForm.UpdateProjection напрямую,
    /// без обращения к самому CameraProjection, чтобы тест был независимой проверкой.
    /// </summary>
    [TestFixture]
    public class ProjectionMatrixTests
    {
        [Test]
        public void GetProjectionMatrix_Perspective_MatchesStandardFormula()
        {
            var projection = new CameraProjection
            {
                Projection = ViewProjection.Perspective,
                AngleOfProjection = 90f,
                Near = 1f,
                Far = 100f
            };
            var viewport = new Viewport(800, 600);

            var matrix = projection.GetProjectionMatrix(viewport, distance: 10f); // distance игнорируется в перспективе

            var aspect = 800f / 600f;
            var f = (float)(1.0 / Math.Tan(90.0 * Math.PI / 180.0 / 2));

            Assert.That(matrix[0, 0], Is.EqualTo(f / aspect).Within(1e-4));
            Assert.That(matrix[1, 1], Is.EqualTo(f).Within(1e-4));
            Assert.That(matrix[2, 2], Is.EqualTo((100f + 1f) / (1f - 100f)).Within(1e-4));
            Assert.That(matrix[2, 3], Is.EqualTo((2f * 100f * 1f) / (1f - 100f)).Within(1e-4));
            Assert.That(matrix[3, 2], Is.EqualTo(-1f).Within(1e-6));
        }

        [Test]
        public void GetProjectionMatrix_Parallel_MatchesExpectedOrthoSizing()
        {
            var projection = new CameraProjection
            {
                Projection = ViewProjection.Parallel,
                AngleOfProjection = 60f
            };
            var viewport = new Viewport(400, 200);
            const float distance = 10f;

            var matrix = projection.GetProjectionMatrix(viewport, distance);

            var aspect = 400f / 200f;
            var height = (float)(Math.Tan(60.0 * Math.PI / 180.0 / 2) * distance * 2);
            var width = height * aspect;
            var sizeX = width / 2;
            var sizeY = height / 2;
            var near = -distance * 2;
            var far = distance * 2;

            Assert.That(matrix[0, 0], Is.EqualTo(1f / sizeX).Within(1e-4));
            Assert.That(matrix[1, 1], Is.EqualTo(1f / sizeY).Within(1e-4));
            Assert.That(matrix[2, 2], Is.EqualTo(-2f / (far - near)).Within(1e-4));
            Assert.That(matrix[3, 3], Is.EqualTo(1f).Within(1e-6));
        }

        [Test]
        public void GetProjectionMatrix_Parallel_ClampsNearZeroDistanceToOne()
        {
            var projection = new CameraProjection { Projection = ViewProjection.Parallel, AngleOfProjection = 60f };
            var viewport = new Viewport(200, 200);

            var matrixNearZero = projection.GetProjectionMatrix(viewport, 0f);
            var matrixOne = projection.GetProjectionMatrix(viewport, 1f);

            Assert.That(matrixNearZero[0, 0], Is.EqualTo(matrixOne[0, 0]).Within(1e-5));
            Assert.That(matrixNearZero[1, 1], Is.EqualTo(matrixOne[1, 1]).Within(1e-5));
        }

        [Test]
        public void Camera_GetProjectionMatrix_DerivesDistanceFromViewMatrixAndDelegates()
        {
            var camera = new SceneCamera { Width = 400, Height = 400 };
            camera.Initialize(0, 0, -20);
            camera.Projection = ViewProjection.Perspective;
            camera.AngleOfProjection = 90f;

            var matrix = camera.GetProjectionMatrix();

            var f = (float)(1.0 / Math.Tan(90.0 * Math.PI / 180.0 / 2));
            Assert.That(matrix[1, 1], Is.EqualTo(f).Within(1e-4));
            Assert.That(matrix[0, 0], Is.EqualTo(f).Within(1e-4)); // aspect 400/400 = 1
        }

        [Test]
        public void Camera_ParallelProjection_DoesNotChangeOnPanRotateAndScale()
        {
            var camera = new SceneCamera { Width = 400, Height = 300 };
            camera.Initialize(0, 0, -5);
            camera.Projection = ViewProjection.Parallel;
            var before = camera.GetProjectionMatrix();

            // Панорамирование, поворот и масштаб не должны менять размер параллельной проекции —
            // иначе выбор объектов расходится с нарисованной картинкой.
            camera.Rotate(ViewAxis.X, 30);
            camera.Scale(1.7f);
            camera.Move(new System.Drawing.Point(120, -80), new System.Drawing.Point(0, 0), camera.ScaleFactor);
            camera.Rotate(ViewAxis.Z, 45);
            var after = camera.GetProjectionMatrix();

            Assert.That(after[0, 0], Is.EqualTo(before[0, 0]).Within(1e-5));
            Assert.That(after[1, 1], Is.EqualTo(before[1, 1]).Within(1e-5));
            Assert.That(after[2, 2], Is.EqualTo(before[2, 2]).Within(1e-5));
        }
    }
}
