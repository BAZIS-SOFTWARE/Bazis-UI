using System;
using WinGUI.Scene.Interfaces;
using MathNet.Numerics.LinearAlgebra;

namespace SceneCore.Camera
{
    /// <summary>
    /// Заменяет BaseForm.UpdateProjection() и P/Invoke glu32.gluPerspective:
    /// ортогональная и перспективная матрицы считаются в коде (см. scene.avalonia.md, раздел 6).
    /// </summary>
    public class CameraProjection
    {
        public ViewProjection Projection { get; set; } = ViewProjection.Perspective;
        public float Near { get; set; } = 1f;
        public float Far { get; set; } = 2000f;

        /// <summary>
        /// Угол обзора в градусах. Хранится здесь, а не только на SceneCamera,
        /// так как именно он нужен для расчёта обеих матриц (перспективной и ортогональной).
        /// </summary>
        public float AngleOfProjection { get; set; } = 2.5f;

        public Matrix<float> GetProjectionMatrix(Viewport viewport, float distance)
        {
            return Projection == ViewProjection.Parallel
                ? GetOrthoMatrix(viewport, distance)
                : GetPerspectiveMatrix(viewport);
        }

        private Matrix<float> GetPerspectiveMatrix(Viewport viewport)
        {
            var aspect = viewport.AspectRatio;
            var radFov = AngleOfProjection * Math.PI / 180.0;
            var f = (float)(1.0 / Math.Tan(radFov / 2));

            var m = Matrix<float>.Build.Dense(4, 4);
            m[0, 0] = f / aspect;
            m[1, 1] = f;
            m[2, 2] = (Far + Near) / (Near - Far);
            m[2, 3] = (2 * Far * Near) / (Near - Far);
            m[3, 2] = -1;
            return m;
        }

        private Matrix<float> GetOrthoMatrix(Viewport viewport, float distance)
        {
            if (Math.Abs(distance) < 1e-4f)
                distance = 1f;

            var aspect = viewport.AspectRatio;
            var radFov = AngleOfProjection * Math.PI / 180.0;
            var height = (float)(Math.Tan(radFov / 2) * distance * 2);
            var width = height * aspect;

            var sizeX = width / 2;
            var sizeY = height / 2;
            var near = -distance * 2;
            var far = distance * 2;

            var m = Matrix<float>.Build.Dense(4, 4);
            m[0, 0] = 1 / sizeX;
            m[1, 1] = 1 / sizeY;
            m[2, 2] = -2f / (far - near);
            m[2, 3] = -(far + near) / (far - near);
            m[3, 3] = 1;
            return m;
        }
    }
}
