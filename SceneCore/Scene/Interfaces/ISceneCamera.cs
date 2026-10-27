using MathNet.Numerics.LinearAlgebra;
using SceneCore.Coordinates;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneCore.Interfaces
{
    /// <summary>
    /// ISceneCamera
    /// </summary>
    public interface ISceneCamera
    {
        /// <summary>
        /// Width
        /// </summary>
        int Width { get; set; }
        /// <summary>
        /// Height
        /// </summary>
        int Height { get; set; }
        /// <summary>
        /// AngleOfProjection
        /// </summary>
        float AngleOfProjection { get; set; }

        /// <summary>
        /// Camera position
        /// </summary>
        ModelPoint3D Position { get; set; }
        /// <summary>
        /// Устанавливает аффинную матрицу вида.
        /// </summary>
        /// <param name="matrix"></param>
        void SetViewMatrix(Matrix<float> matrix);
        /// <summary>
        /// Возвращает копию матрицы вида.
        /// </summary>
        /// <returns></returns>
        Matrix<float> GetViewMatrix();
        /// <summary>
        /// Преобразует вектор в плоскости камеры в вектор координат модели.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        ModelVector3D GetSceneVectorOfScreenVector(float x, float y);
        /// <summary>
        /// Move
        /// </summary>
        /// <param name="new_mousePosition"></param>
        /// <param name="mousePosition"></param>
        /// <param name="scaleFactor"></param>
        void Move(Point new_mousePosition, Point mousePosition, float scaleFactor);

        /// <summary>
        /// Преобразует точку модели в координаты камеры.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        CameraPoint3D GetSceneCoord(ModelPoint3D point);
        /// <summary>
        /// Преобразует смещение в физических пикселях окна просмотра в вектор сцены.
        /// </summary>
        /// <param name="deltaX">Горизонтальное смещение в пикселях.</param>
        /// <param name="deltaY">Вертикальное смещение в пикселях, ось направлена вверх.</param>
        /// <param name="depth"></param>
        /// <param name="scaleFactor"></param>
        /// <returns></returns>
        ModelVector3D GetSceneVector(int deltaX, int deltaY, float depth, float scaleFactor);
        /// <summary>Проецирует точку сцены в координаты окна просмотра с началом в центре.</summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        ViewportPoint GetScreenCoord(CameraPoint3D coord);
        /// <summary>
        /// Rotate
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="angle"></param>
        void Rotate(ViewAxis axis, float angle);
        /// <summary>
        /// Rotate
        /// </summary>
        /// <param name="vector_dx"></param>
        /// <param name="vector_dy"></param>
        /// <param name="axis"></param>
        /// <param name="angle"></param>
        void Rotate(float vector_dx, float vector_dy, ViewAxis axis, float angle);
        /// <summary>
        /// SetOnPlane
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="scaleFactor"></param>
        void SetOnPlane(ViewPlane plane, float scaleFactor);
    }
}
