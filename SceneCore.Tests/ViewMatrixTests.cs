using SceneCore.Camera;
using WinGUI.Scene.Interfaces;
using Geometry;
using MathNet.Numerics.LinearAlgebra;

namespace SceneCore.Tests
{
    /// <summary>
    /// Матрица вида хранится в поле SceneCamera, а не читается из стека GL_MODELVIEW —
    /// именно это и делает камеру тестируемой без живого GL-контекста (см. scene.avalonia.md).
    /// </summary>
    [TestFixture]
    public class ViewMatrixTests
    {
        [Test]
        public void Initialize_SetsTranslationColumn_RotationStaysIdentity()
        {
            var camera = new SceneCamera();
            camera.Initialize(1f, 2f, -5f);

            var view = camera.GetViewMatrix();

            Assert.That(view[0, 3], Is.EqualTo(1f).Within(1e-5));
            Assert.That(view[1, 3], Is.EqualTo(2f).Within(1e-5));
            Assert.That(view[2, 3], Is.EqualTo(-5f).Within(1e-5));
            Assert.That(view[0, 0], Is.EqualTo(1f).Within(1e-5));
            Assert.That(view[1, 1], Is.EqualTo(1f).Within(1e-5));
            Assert.That(view[2, 2], Is.EqualTo(1f).Within(1e-5));
        }

        [Test]
        public void SetViewMatrix_GetViewMatrix_RoundTrips()
        {
            var camera = new SceneCamera();
            var custom = Matrix<float>.Build.DenseIdentity(4);
            custom[0, 3] = 7f;
            custom[1, 1] = 2f;

            camera.SetViewMatrix(custom);
            var view = camera.GetViewMatrix();

            Assert.That(view[0, 3], Is.EqualTo(7f));
            Assert.That(view[1, 1], Is.EqualTo(2f));
        }

        [Test]
        public void GetSceenCoord_TranslatesPointByCameraOffset()
        {
            var camera = new SceneCamera();
            camera.Initialize(0, 0, -5);

            var result = camera.GetSceenCoord(new Point3D(1, 2, 3));

            Assert.That(result._x, Is.EqualTo(1f).Within(1e-5));
            Assert.That(result._y, Is.EqualTo(2f).Within(1e-5));
            Assert.That(result._z, Is.EqualTo(-2f).Within(1e-5)); // 3 + (-5)
        }

        [Test]
        public void Scale_UpdatesScaleFactorToAppliedFactor()
        {
            var camera = new SceneCamera();
            camera.Initialize(0, 0, -5);

            camera.Scale(2f);

            Assert.That(camera.ScaleFactor, Is.EqualTo(2f).Within(1e-4));
        }

        [Test]
        public void SetRotationCentre_ZeroesPanKeepsDepthAndSetsPosition()
        {
            var camera = new SceneCamera();
            camera.Initialize(3, 4, -5);
            var point = new Point3D(10, 20, 30);

            camera.SetRotationCentre(point);
            var view = camera.GetViewMatrix();

            Assert.That(view[0, 3], Is.EqualTo(0f));
            Assert.That(view[1, 3], Is.EqualTo(0f));
            Assert.That(view[2, 3], Is.EqualTo(-5f)); // расстояние до центра вращения не меняется
            Assert.That(camera.Position._x, Is.EqualTo(10f));
            Assert.That(camera.Position._y, Is.EqualTo(20f));
            Assert.That(camera.Position._z, Is.EqualTo(30f));
        }

        [Test]
        public void ResetPan_ZeroesTranslation_WithoutChangingPosition()
        {
            var camera = new SceneCamera();
            camera.Initialize(3, 4, -5);
            camera.Position = new Point3D(1, 1, 1);

            camera.ResetPan();
            var view = camera.GetViewMatrix();

            Assert.That(view[0, 3], Is.EqualTo(0f));
            Assert.That(view[1, 3], Is.EqualTo(0f));
            Assert.That(camera.Position._x, Is.EqualTo(1f));
        }

        [Test]
        public void Rotate_AroundZ_FromIdentity_MatchesStandardRotationMatrix()
        {
            var camera = new SceneCamera();

            camera.Rotate(ViewAxis.Z, 90f);
            var view = camera.GetViewMatrix();

            Assert.That(view[0, 0], Is.EqualTo(0f).Within(1e-4));
            Assert.That(view[0, 1], Is.EqualTo(-1f).Within(1e-4));
            Assert.That(view[1, 0], Is.EqualTo(1f).Within(1e-4));
            Assert.That(view[1, 1], Is.EqualTo(0f).Within(1e-4));
            Assert.That(view[2, 2], Is.EqualTo(1f).Within(1e-4));
        }

        [Test]
        public void SetOnPlane_XY_ScalesDiagonalAndPreservesTranslation()
        {
            var camera = new SceneCamera();
            camera.Initialize(1, 2, -5);

            camera.SetOnPlane(ViewPlane.XY, 3f);
            var view = camera.GetViewMatrix();

            Assert.That(view[0, 0], Is.EqualTo(3f));
            Assert.That(view[1, 1], Is.EqualTo(3f));
            Assert.That(view[2, 2], Is.EqualTo(3f));
            Assert.That(view[0, 3], Is.EqualTo(1f));
            Assert.That(view[1, 3], Is.EqualTo(2f));
            Assert.That(view[2, 3], Is.EqualTo(-5f));
            Assert.That(view[3, 3], Is.EqualTo(1f));
        }
    }
}
