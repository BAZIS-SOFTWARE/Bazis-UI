using BazisGUI.SceneCore.Camera;
using BazisGUI.SceneCore.Interfaces;
using BazisGUI.SceneCore.Coordinates;
using System;
using MathNet.Numerics.LinearAlgebra;

namespace BazisGUI.SceneCore.Tests
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

        /// <summary>Проверяет, что матрица вида не принимает проективную строку.</summary>
        [TestCase(0f, 0f, 0f, 0f)]
        [TestCase(0f, 0f, -1f, 0f)]
        [TestCase(0f, 0f, 0f, 0.9999995f)]
        public void SetViewMatrix_RejectsNonAffineBottomRow(float x, float y, float z, float w)
        {
            var camera = new SceneCamera();
            var matrix = Matrix<float>.Build.DenseIdentity(4);
            matrix[3, 0] = x;
            matrix[3, 1] = y;
            matrix[3, 2] = z;
            matrix[3, 3] = w;

            Assert.Throws<ArgumentException>(() => camera.SetViewMatrix(matrix));
        }

        /// <summary>Проверяет, что внешние ссылки не могут изменить матрицу камеры.</summary>
        [Test]
        public void ViewMatrix_ExternalMutation_DoesNotChangeCamera()
        {
            var camera = new SceneCamera();
            var input = Matrix<float>.Build.DenseIdentity(4);
            camera.SetViewMatrix(input);

            input[3, 2] = -1f;
            var output = camera.GetViewMatrix();
            output[3, 2] = -1f;

            var actual = camera.GetViewMatrix();
            Assert.That(actual[3, 2], Is.EqualTo(0f));
            Assert.That(actual[3, 3], Is.EqualTo(1f));
        }

        /// <summary>Проверяет, что перенос камеры не влияет на преобразование вектора.</summary>
        [Test]
        public void GetSceneCoordOfScreenVector_IgnoresViewTranslation()
        {
            var camera = new SceneCamera();
            camera.Initialize(3, 4, -5);

            var vector = camera.GetSceneVectorOfScreenVector(2, -1);

            Assert.That(vector.X, Is.EqualTo(2).Within(1e-6));
            Assert.That(vector.Y, Is.EqualTo(-1).Within(1e-6));
            Assert.That(vector.Z, Is.EqualTo(0).Within(1e-6));
        }

        [Test]
        public void GetSceenCoord_TranslatesPointByCameraOffset()
        {
            var camera = new SceneCamera();
            camera.Initialize(0, 0, -5);

            var modelPoint = new ModelPoint3D(1, 2, 3);
            var result = camera.GetSceneCoord(modelPoint);

            Assert.That(result.X, Is.EqualTo(1f).Within(1e-5));
            Assert.That(result.Y, Is.EqualTo(2f).Within(1e-5));
            Assert.That(result.Z, Is.EqualTo(-2f).Within(1e-5)); // 3 + (-5)
        }

        /// <summary>Проверяет разные правила преобразования точки и направления при повороте и переносе вида.</summary>
        [Test]
        public void GetSceneCoordAndVector_ApplyRotationAndTranslation()
        {
            var camera = new SceneCamera();
            var view = Matrix<float>.Build.DenseIdentity(4);
            view[0, 0] = 0;
            view[0, 1] = -1;
            view[1, 0] = 1;
            view[1, 1] = 0;
            view[0, 3] = 4;
            view[1, 3] = -2;
            view[2, 3] = -5;
            camera.SetViewMatrix(view);
            camera.Position = new ModelPoint3D(10, 20, 30);

            var cameraPoint = camera.GetSceneCoord(new ModelPoint3D(13, 22, 35));
            var modelVector = camera.GetSceneVectorOfScreenVector(2, -1);

            Assert.That(cameraPoint.X, Is.EqualTo(2).Within(1e-6));
            Assert.That(cameraPoint.Y, Is.EqualTo(1).Within(1e-6));
            Assert.That(cameraPoint.Z, Is.EqualTo(0).Within(1e-6));
            Assert.That(modelVector.X, Is.EqualTo(-1).Within(1e-6));
            Assert.That(modelVector.Y, Is.EqualTo(-2).Within(1e-6));
            Assert.That(modelVector.Z, Is.EqualTo(0).Within(1e-6));
        }

        /// <summary>Проверяет сохранение малого смещения относительно удалённого центра вращения.</summary>
        [Test]
        public void GetSceenCoord_PreservesLocalOffsetAtLargeWorldPosition()
        {
            var camera = new SceneCamera();
            var centre = new ModelPoint3D(1000000000, -1000000000, 1000000000);
            camera.SetRotationCentre(centre);

            var modelPoint = new ModelPoint3D(1000000000.125, -999999999.75, 1000000000.375);
            var result = camera.GetSceneCoord(modelPoint);

            Assert.That(result.X, Is.EqualTo(0.125).Within(1e-6));
            Assert.That(result.Y, Is.EqualTo(0.25).Within(1e-6));
            Assert.That(result.Z, Is.EqualTo(0.375).Within(1e-6));
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
            var point = new ModelPoint3D(10, 20, 30);

            camera.SetRotationCentre(point);
            var view = camera.GetViewMatrix();

            Assert.That(view[0, 3], Is.EqualTo(0f));
            Assert.That(view[1, 3], Is.EqualTo(0f));
            Assert.That(view[2, 3], Is.EqualTo(-5f)); // расстояние до центра вращения не меняется
            Assert.That(camera.Position.X, Is.EqualTo(10f));
            Assert.That(camera.Position.Y, Is.EqualTo(20f));
            Assert.That(camera.Position.Z, Is.EqualTo(30f));
        }

        [Test]
        public void ResetPan_ZeroesTranslation_WithoutChangingPosition()
        {
            var camera = new SceneCamera();
            camera.Initialize(3, 4, -5);
            camera.Position = new ModelPoint3D(1, 1, 1);

            camera.ResetPan();
            var view = camera.GetViewMatrix();

            Assert.That(view[0, 3], Is.EqualTo(0f));
            Assert.That(view[1, 3], Is.EqualTo(0f));
            Assert.That(camera.Position.X, Is.EqualTo(1f));
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

        /// <summary>Проверяет направление оси Z после уже выполненного поворота вида.</summary>
        [TestCase(false)]
        [TestCase(true)]
        public void Rotate_AroundZ_AfterViewRotation_UsesModelDirection(bool usePointerOverload)
        {
            var camera = new SceneCamera();
            var view = Matrix<float>.Build.DenseIdentity(4);
            view[1, 1] = 0;
            view[1, 2] = -1;
            view[2, 1] = 1;
            view[2, 2] = 0;
            camera.SetViewMatrix(view);

            if (usePointerOverload)
                camera.Rotate(1, 0, ViewAxis.Z, 90f);
            else
                camera.Rotate(ViewAxis.Z, 90f);

            var rotatedView = camera.GetViewMatrix();
            Assert.That(rotatedView[0, 0], Is.EqualTo(0).Within(1e-5));
            Assert.That(rotatedView[0, 2], Is.EqualTo(1).Within(1e-5));
            Assert.That(rotatedView[1, 0], Is.EqualTo(1).Within(1e-5));
            Assert.That(rotatedView[1, 2], Is.EqualTo(0).Within(1e-5));
            Assert.That(rotatedView[2, 1], Is.EqualTo(1).Within(1e-5));
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
