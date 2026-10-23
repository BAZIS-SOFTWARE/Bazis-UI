using System;
using System.Drawing;
using WinGUI.Scene.Interfaces;
using Geometry;
using MathNet.Numerics.LinearAlgebra;

namespace SceneCore.Camera
{
    /// <summary>
    /// Видовая матрица хранится в поле, а не в стеке GL_MODELVIEW (как раньше в BaseForm) —
    /// это и есть ключевое изменение перехода, см. scene.avalonia.md, раздел 6.
    /// Камера больше не требует живого GL-контекста и может быть покрыта тестами.
    /// </summary>
    public class SceneCamera : ISceneCamera
    {
        private Matrix<float> viewMatrix = GlMatrix.Identity();
        private Viewport viewport;
        private readonly CameraProjection projection = new CameraProjection();

        public int Width
        {
            get => viewport.Width;
            set => viewport.Width = value;
        }

        public int Height
        {
            get => viewport.Height;
            set => viewport.Height = value;
        }

        public float AngleOfProjection
        {
            get => projection.AngleOfProjection;
            set => projection.AngleOfProjection = value;
        }

        public ViewProjection Projection
        {
            get => projection.Projection;
            set => projection.Projection = value;
        }

        public Point3D Position { get; set; } = new Point3D();

        public float ScaleFactor { get; private set; } = 1f;

        public Viewport Viewport
        {
            get => viewport;
            set => viewport = value;
        }

        public CameraProjection CameraProjection => projection;

        public void Initialize(float moveX, float moveY, float moveZ)
        {
            viewMatrix = GlMatrix.Translate(GlMatrix.Identity(), moveX, moveY, moveZ);
        }

        public void SetViewMatrix(Matrix<float> matrix) => viewMatrix = matrix;

        public Matrix<float> GetViewMatrix() => viewMatrix;

        /// <summary>
        /// Матрица проекции, рассчитанная от текущей дистанции камеры до центра вращения
        /// (заменяет BaseForm.UpdateProjection()).
        /// </summary>
        public Matrix<float> GetProjectionMatrix() => projection.GetProjectionMatrix(viewport, ComputeDistance());

        /// <summary>
        /// Дистанция камеры для параллельной проекции — сдвиг вида по оси Z.
        /// В BaseForm.UpdateProjection бралась длина всего столбца переноса, но матрица проекции там
        /// пересчитывалась только при смене размера/настроек, поэтому панорамирование её не меняло.
        /// Здесь проекция считается на лету (и при отрисовке, и при выборе объектов), поэтому
        /// панорамирование (X/Y столбца переноса) не должно менять масштаб: Move, Rotate, Scale и
        /// SetOnPlane сдвиг по Z не меняют, и размер параллельной проекции остаётся постоянным.
        /// </summary>
        private float ComputeDistance() => Math.Abs(viewMatrix[2, 3]);

        public Point3D GetSceneCoordOfScreenVector(float x, float y)
        {
            return GlMatrix.Transform(viewMatrix.Transpose(), x, y, 0, 0);
        }

        public void Move(Point new_mousePosition, Point mousePosition, float scaleFactor)
        {
            var dx = new_mousePosition.X - mousePosition.X;
            var dy = new_mousePosition.Y - mousePosition.Y;

            var pos = new Point2D(-dx, -dy);
            var crd = GetSceenCoord(pos, -5, scaleFactor);

            viewMatrix = GlMatrix.Translate(viewMatrix, crd._x, crd._y, crd._z);
        }

        /// <summary>
        /// Масштабирование сцены (аналог BaseForm.ScaleObjs). Не входит в ISceneCamera,
        /// так как в текущем коде это отдельная операция, а не часть контракта камеры.
        /// </summary>
        public void Scale(float scaleFactor)
        {
            viewMatrix = GlMatrix.Scale(viewMatrix, scaleFactor, scaleFactor, scaleFactor);
            var crd = GetSceneCoordOfScreenVector(0, 1);
            ScaleFactor = (float)Math.Sqrt(crd._x * crd._x + crd._y * crd._y + crd._z * crd._z);
        }

        /// <summary>
        /// Перенос центра вращения в мировую точку без изменения расстояния до неё
        /// (аналог BaseForm.SetRotationCentre).
        /// </summary>
        public void SetRotationCentre(Point3D modelPoint)
        {
            Position = modelPoint;
            viewMatrix[0, 3] = 0;
            viewMatrix[1, 3] = 0;
        }

        /// <summary>Аналог BaseForm.SetPositionBack(): убирает панорамирование, не трогая Position.</summary>
        public void ResetPan()
        {
            viewMatrix[0, 3] = 0;
            viewMatrix[1, 3] = 0;
        }

        public Point3D GetSceenCoord(float x, float y, float z)
        {
            return GlMatrix.Transform(viewMatrix, x - Position._x, y - Position._y, z - Position._z, 1);
        }

        public Point3D GetSceenCoord(Point3D point)
        {
            var shift = point.Sub(Position);
            return GlMatrix.Transform(viewMatrix, shift._x, shift._y, shift._z, 1);
        }

        public Point3D GetSceenCoord(Point2D point2D, float depth, float scaleFactor)
        {
            var viewPortKoeff = (float)viewport.Height / viewport.Width;
            var tan = (float)Math.Tan(AngleOfProjection * 3.14f / 180);

            var xs = point2D._x * tan * depth / viewPortKoeff / viewport.Width;
            var ys = point2D._y * tan * depth / viewport.Height;

            xs = xs / scaleFactor / scaleFactor;
            ys = ys / scaleFactor / scaleFactor;

            return GetSceneCoordOfScreenVector((float)xs, (float)ys);
        }

        public Point2D GetScreenCoord(Point3D coord)
        {
            if (Projection == ViewProjection.Perspective)
            {
                var xn = -(coord._x / coord._z);
                var yn = -(coord._y / coord._z);
                var viewPortKoeff = (float)viewport.Height / viewport.Width;
                var tan = (float)Math.Tan(AngleOfProjection * 3.14f / 180);

                var xScr = xn * viewPortKoeff * (viewport.Width / tan);
                var yScr = yn * (viewport.Height / tan);

                return new Point2D(xScr, yScr);
            }
            else
            {
                var v = Vector<float>.Build.DenseOfArray(new[] { coord._x, coord._y, coord._z, 1 });
                GetProjectionMatrix().Multiply(v, v);

                var screenX = v[0] * 0.5f * viewport.Width;
                var screenY = v[1] * 0.5f * viewport.Height;

                return new Point2D(screenX, screenY);
            }
        }

        public void Rotate(ViewAxis axis, float angle)
        {
            switch (axis)
            {
                case ViewAxis.X:
                    var crdx = GetSceneCoordOfScreenVector(-1, 0);
                    viewMatrix = GlMatrix.Rotate(viewMatrix, angle, crdx._x, crdx._y, crdx._z);
                    break;
                case ViewAxis.Y:
                    var crdy = GetSceneCoordOfScreenVector(0, -1);
                    viewMatrix = GlMatrix.Rotate(viewMatrix, angle, crdy._x, crdy._y, crdy._z);
                    break;
                case ViewAxis.Z:
                    var vectorZ = GlMatrix.Transform(viewMatrix.Transpose(), 0, 0, 1, 0);
                    viewMatrix = GlMatrix.Rotate(viewMatrix, angle, vectorZ._x, vectorZ._y, vectorZ._z);
                    break;
            }
        }

        public void Rotate(float vector_dx, float vector_dy, ViewAxis axis, float angle)
        {
            switch (axis)
            {
                case ViewAxis.X:
                    if (vector_dx == 0)
                    {
                        var crdx = GetSceneCoordOfScreenVector(vector_dy > 0 ? 1 : -1, 0);
                        viewMatrix = GlMatrix.Rotate(viewMatrix, angle, crdx._x, crdx._y, crdx._z);
                    }
                    break;
                case ViewAxis.Y:
                    if (vector_dy == 0)
                    {
                        var crdy = GetSceneCoordOfScreenVector(0, vector_dx > 0 ? 1 : -1);
                        viewMatrix = GlMatrix.Rotate(viewMatrix, angle, crdy._x, crdy._y, crdy._z);
                    }
                    break;
                case ViewAxis.Z:
                    if (vector_dy == 0)
                    {
                        var z = vector_dx > 0 ? 1 : -1;
                        var vectorZ = GlMatrix.Transform(viewMatrix.Transpose(), 0, 0, z, 0);
                        viewMatrix = GlMatrix.Rotate(viewMatrix, angle, vectorZ._x, vectorZ._y, vectorZ._z);
                    }
                    break;
                case ViewAxis.XYZ:
                    var crdxyz = GetSceneCoordOfScreenVector(-vector_dy, vector_dx);
                    viewMatrix = GlMatrix.Rotate(viewMatrix, angle, crdxyz._x, crdxyz._y, crdxyz._z);
                    break;
            }
        }

        public void SetOnPlane(ViewPlane plane, float scaleFactor)
        {
            var x = viewMatrix[0, 3];
            var y = viewMatrix[1, 3];
            var z = viewMatrix[2, 3];

            var m = Matrix<float>.Build.Dense(4, 4);
            m[0, 0] = scaleFactor;
            m[1, 1] = scaleFactor;
            m[2, 2] = scaleFactor;
            m[0, 3] = x;
            m[1, 3] = y;
            m[2, 3] = z;
            m[3, 3] = 1;
            viewMatrix = m;

            switch (plane)
            {
                case ViewPlane.XZ:
                    Rotate(ViewAxis.X, -90);
                    break;
                case ViewPlane.YZ:
                    Rotate(ViewAxis.Y, 90);
                    break;
            }
        }
    }
}
