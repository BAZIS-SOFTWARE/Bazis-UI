using System;
using System.Drawing;
using BazisGUI.SceneCore.Interfaces;
using BazisGUI.SceneCore.Coordinates;
using MathNet.Numerics.LinearAlgebra;

namespace BazisGUI.SceneCore.Camera
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

        public ModelPoint3D Position { get; set; } = new ModelPoint3D(0, 0, 0);

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

        /// <summary>Устанавливает аффинную матрицу вида.</summary>
        public void SetViewMatrix(Matrix<float> matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix), "View matrix cannot be null.");

            if (matrix.RowCount != 4 || matrix.ColumnCount != 4)
                throw new ArgumentException("View matrix must be 4x4.", nameof(matrix));

            for (var row = 0; row < 4; row++)
            {
                for (var column = 0; column < 4; column++)
                {
                    if (!float.IsFinite(matrix[row, column]))
                        throw new ArgumentException("View matrix must contain only finite values.", nameof(matrix));
                }
            }

            if (matrix[3, 0] != 0f || matrix[3, 1] != 0f ||
                matrix[3, 2] != 0f || matrix[3, 3] != 1f)
                throw new ArgumentException("View matrix must be affine.", nameof(matrix));

            viewMatrix = matrix.Clone();
        }

        /// <summary>Возвращает копию матрицы вида для защиты её аффинности.</summary>
        public Matrix<float> GetViewMatrix() => viewMatrix.Clone();

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

        /// <summary>Преобразует вектор в плоскости камеры в вектор координат модели.</summary>
        public ModelVector3D GetSceneVectorOfScreenVector(float x, float y)
        {
            var modelX = viewMatrix[0, 0] * x + viewMatrix[1, 0] * y;
            var modelY = viewMatrix[0, 1] * x + viewMatrix[1, 1] * y;
            var modelZ = viewMatrix[0, 2] * x + viewMatrix[1, 2] * y;
            return new ModelVector3D(modelX, modelY, modelZ);
        }

        public void Move(Point new_mousePosition, Point mousePosition, float scaleFactor)
        {
            var dx = new_mousePosition.X - mousePosition.X;
            var dy = new_mousePosition.Y - mousePosition.Y;

            var deltaX = -dx;
            var deltaY = -dy;
            var crd = GetSceneVector(deltaX, deltaY, -5, scaleFactor);

            viewMatrix = GlMatrix.Translate(viewMatrix, crd.X, crd.Y, crd.Z);
        }

        /// <summary>
        /// Масштабирование сцены (аналог BaseForm.ScaleObjs). Не входит в ISceneCamera,
        /// так как в текущем коде это отдельная операция, а не часть контракта камеры.
        /// </summary>
        public void Scale(float scaleFactor)
        {
            viewMatrix = GlMatrix.Scale(viewMatrix, scaleFactor, scaleFactor, scaleFactor);
            var crd = GetSceneVectorOfScreenVector(0, 1);
            ScaleFactor = (float)Math.Sqrt(crd.X * crd.X + crd.Y * crd.Y + crd.Z * crd.Z);
        }

        /// <summary>
        /// Перенос центра вращения в мировую точку без изменения расстояния до неё
        /// (аналог BaseForm.SetRotationCentre).
        /// </summary>
        public void SetRotationCentre(ModelPoint3D modelPoint)
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

        /// <summary>Преобразует точку модели в координаты камеры.</summary>
        public CameraPoint3D GetSceneCoord(ModelPoint3D point)
        {
            var position = Position;
            var x = (float)(point.X - position.X);
            var y = (float)(point.Y - position.Y);
            var z = (float)(point.Z - position.Z);

            var cameraX = viewMatrix[0, 0] * x + viewMatrix[0, 1] * y + viewMatrix[0, 2] * z + viewMatrix[0, 3];
            var cameraY = viewMatrix[1, 0] * x + viewMatrix[1, 1] * y + viewMatrix[1, 2] * z + viewMatrix[1, 3];
            var cameraZ = viewMatrix[2, 0] * x + viewMatrix[2, 1] * y + viewMatrix[2, 2] * z + viewMatrix[2, 3];
            return new CameraPoint3D(cameraX, cameraY, cameraZ);
        }

        /// <summary>Преобразует смещение в физических пикселях окна просмотра в вектор сцены.</summary>
        public ModelVector3D GetSceneVector(int deltaX, int deltaY, float depth, float scaleFactor)
        {
            var viewPortKoeff = (float)viewport.Height / viewport.Width;
            var tan = (float)Math.Tan(AngleOfProjection * 3.14f / 180);

            var xs = (double)deltaX * tan * depth / viewPortKoeff / viewport.Width;
            var ys = (double)deltaY * tan * depth / viewport.Height;

            xs = xs / scaleFactor / scaleFactor;
            ys = ys / scaleFactor / scaleFactor;

            return GetSceneVectorOfScreenVector((float)xs, (float)ys);
        }

        /// <summary>Проецирует точку сцены в физические пиксели относительно центра окна просмотра.</summary>
        public ViewportPoint GetScreenCoord(CameraPoint3D coord)
        {
            if (Projection == ViewProjection.Perspective)
            {
                var xn = -(coord.X / coord.Z);
                var yn = -(coord.Y / coord.Z);
                var viewPortKoeff = (float)viewport.Height / viewport.Width;
                var tan = (float)Math.Tan(AngleOfProjection * 3.14f / 180);

                var xScr = xn * viewPortKoeff * (viewport.Width / tan);
                var yScr = yn * (viewport.Height / tan);

                return new ViewportPoint(xScr, yScr);
            }
            else
            {
                var v = Vector<float>.Build.DenseOfArray(new[] { (float)coord.X, (float)coord.Y, (float)coord.Z, 1f });
                var projectionMatrix = GetProjectionMatrix();
                projectionMatrix.Multiply(v, v);

                var screenX = v[0] * 0.5f * viewport.Width;
                var screenY = v[1] * 0.5f * viewport.Height;

                return new ViewportPoint(screenX, screenY);
            }
        }

        public void Rotate(ViewAxis axis, float angle)
        {
            switch (axis)
            {
                case ViewAxis.X:
                    var crdx = GetSceneVectorOfScreenVector(-1, 0);
                    viewMatrix = GlMatrix.Rotate(viewMatrix, angle, crdx.X, crdx.Y, crdx.Z);
                    break;
                case ViewAxis.Y:
                    var crdy = GetSceneVectorOfScreenVector(0, -1);
                    viewMatrix = GlMatrix.Rotate(viewMatrix, angle, crdy.X, crdy.Y, crdy.Z);
                    break;
                case ViewAxis.Z:
                    var vectorZ = new ModelVector3D(viewMatrix[2, 0], viewMatrix[2, 1], viewMatrix[2, 2]);
                    viewMatrix = GlMatrix.Rotate(viewMatrix, angle, vectorZ.X, vectorZ.Y, vectorZ.Z);
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
                        var crdx = GetSceneVectorOfScreenVector(vector_dy > 0 ? 1 : -1, 0);
                        viewMatrix = GlMatrix.Rotate(viewMatrix, angle, crdx.X, crdx.Y, crdx.Z);
                    }
                    break;
                case ViewAxis.Y:
                    if (vector_dy == 0)
                    {
                        var crdy = GetSceneVectorOfScreenVector(0, vector_dx > 0 ? 1 : -1);
                        viewMatrix = GlMatrix.Rotate(viewMatrix, angle, crdy.X, crdy.Y, crdy.Z);
                    }
                    break;
                case ViewAxis.Z:
                    if (vector_dy == 0)
                    {
                        var z = vector_dx > 0 ? 1 : -1;
                        var vectorZ = new ModelVector3D(viewMatrix[2, 0] * z, viewMatrix[2, 1] * z, viewMatrix[2, 2] * z);
                        viewMatrix = GlMatrix.Rotate(viewMatrix, angle, vectorZ.X, vectorZ.Y, vectorZ.Z);
                    }
                    break;
                case ViewAxis.XYZ:
                    var crdxyz = GetSceneVectorOfScreenVector(-vector_dy, vector_dx);
                    viewMatrix = GlMatrix.Rotate(viewMatrix, angle, crdxyz.X, crdxyz.Y, crdxyz.Z);
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
