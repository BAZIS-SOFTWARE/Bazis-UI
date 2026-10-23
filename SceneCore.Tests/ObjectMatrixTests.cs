using WinGUI.Scene;
using SceneCore;
using SceneCore.Camera;
using Geometry;
using MathNet.Numerics.LinearAlgebra;

namespace SceneCore.Tests
{
    /// <summary>
    /// GlMatrix — точечная замена GL.Translate/Rotate/Scale, из которой строятся все модельные
    /// (объектные) матрицы в Core: и матрица вида камеры, и матрица объекта сечения ниже.
    /// </summary>
    [TestFixture]
    public class GlMatrixTests
    {
        [Test]
        public void Translate_SetsTranslationColumn()
        {
            var m = GlMatrix.Translate(GlMatrix.Identity(), 1f, 2f, 3f);

            Assert.That(m[0, 3], Is.EqualTo(1f));
            Assert.That(m[1, 3], Is.EqualTo(2f));
            Assert.That(m[2, 3], Is.EqualTo(3f));
            Assert.That(m[0, 0], Is.EqualTo(1f));
        }

        [Test]
        public void Scale_SetsDiagonal()
        {
            var m = GlMatrix.Scale(GlMatrix.Identity(), 2f, 3f, 4f);

            Assert.That(m[0, 0], Is.EqualTo(2f));
            Assert.That(m[1, 1], Is.EqualTo(3f));
            Assert.That(m[2, 2], Is.EqualTo(4f));
        }

        [Test]
        public void RotationMatrix_ZeroLengthAxis_FallsBackToIdentity()
        {
            var m = GlMatrix.RotationMatrix(45f, 0, 0, 0);

            Assert.That(m, Is.EqualTo(Matrix<float>.Build.DenseIdentity(4)));
        }

        [Test]
        public void RotationMatrix_90DegreesAroundZ_MatchesExpected()
        {
            var m = GlMatrix.RotationMatrix(90f, 0, 0, 1);

            Assert.That(m[0, 0], Is.EqualTo(0f).Within(1e-5));
            Assert.That(m[0, 1], Is.EqualTo(-1f).Within(1e-5));
            Assert.That(m[1, 0], Is.EqualTo(1f).Within(1e-5));
            Assert.That(m[1, 1], Is.EqualTo(0f).Within(1e-5));
            Assert.That(m[2, 2], Is.EqualTo(1f).Within(1e-5));
        }

        [Test]
        public void Transform_AppliesMatrixToVector()
        {
            var m = GlMatrix.Translate(GlMatrix.Identity(), 5, 0, 0);

            var p = GlMatrix.Transform(m, 1, 2, 3, 1);

            Assert.That(p._x, Is.EqualTo(6f).Within(1e-5));
            Assert.That(p._y, Is.EqualTo(2f).Within(1e-5));
            Assert.That(p._z, Is.EqualTo(3f).Within(1e-5));
        }
    }

    /// <summary>
    /// Матрица объекта: SceneController.ComputeClipPlaneModelMatrix — чистая математика,
    /// вынесенная из ChangeClipPlane именно для того, чтобы её можно было проверить без VBO/GL
    /// (см. комментарий на самом методе). Ожидаемые значения посчитаны вручную по формуле
    /// переноса+поворота, а не переписыванием той же формулы.
    /// </summary>
    [TestFixture]
    public class ObjectModelMatrixTests
    {
        [Test]
        public void ComputeClipPlaneModelMatrix_VerticalPlane_MatchesHandDerivedMatrix()
        {
            var plane = new Plane(new Point3D(1, 0, 0), 3f);
            var bounds = new BoundingBox(new Point3D(-1, 1, 1), new Point3D(1, -1, -1)); // центр в (0,0,0)

            var model = SceneController.ComputeClipPlaneModelMatrix(plane, bounds);

            // Перенос на (3,0,0) и поворот на 90° вокруг оси (0,-1,0),
            // выведено вручную из формулы: translate(center) * translate(sign(normal)*normal*shifting) * rotate(angle, axis)
            var expected = new[,]
            {
                { 0f, 0f, -1f, 3f },
                { 0f, 1f,  0f, 0f },
                { 1f, 0f,  0f, 0f },
                { 0f, 0f,  0f, 1f }
            };

            for (var r = 0; r < 4; r++)
                for (var c = 0; c < 4; c++)
                    Assert.That(model[r, c], Is.EqualTo(expected[r, c]).Within(1e-4), $"element [{r},{c}]");
        }

        [Test]
        public void ComputeClipPlaneModelMatrix_NormalAntiParallelToViewAxis_KeepsTranslationOnly()
        {
            // normal (0,0,1) антипараллельна (0,0,-1): ось поворота вырождена (нулевой вектор),
            // GlMatrix.RotationMatrix в этом случае возвращает единичную матрицу — остаётся перенос.
            var plane = new Plane(new Point3D(0, 0, 1), 5f);
            var bounds = new BoundingBox(new Point3D(-1, 1, 1), new Point3D(1, -1, -1));

            var model = SceneController.ComputeClipPlaneModelMatrix(plane, bounds);

            Assert.That(model[0, 0], Is.EqualTo(1f).Within(1e-5));
            Assert.That(model[1, 1], Is.EqualTo(1f).Within(1e-5));
            Assert.That(model[2, 2], Is.EqualTo(1f).Within(1e-5));
            Assert.That(model[0, 3], Is.EqualTo(0f).Within(1e-5));
            Assert.That(model[1, 3], Is.EqualTo(0f).Within(1e-5));
            Assert.That(model[2, 3], Is.EqualTo(5f).Within(1e-5));
        }

        [Test]
        public void ComputeClipPlaneModelMatrix_OffsetBoundingBoxCentre_ShiftsTranslation()
        {
            var plane = new Plane(new Point3D(0, 1, 0), 0f); // shifting=0 => origin вклада в перенос нет
            var bounds = new BoundingBox(new Point3D(1, 5, 3), new Point3D(3, 1, -1)); // центр (2,3,1)

            var model = SceneController.ComputeClipPlaneModelMatrix(plane, bounds);

            Assert.That(model[0, 3], Is.EqualTo(2f).Within(1e-4));
            Assert.That(model[1, 3], Is.EqualTo(3f).Within(1e-4));
            Assert.That(model[2, 3], Is.EqualTo(1f).Within(1e-4));
        }
    }
}
