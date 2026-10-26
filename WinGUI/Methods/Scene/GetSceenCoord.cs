using Geometry;

using SceneCore.Coordinates;

namespace WinGUI
{
    // Пересчёт экранных/сценовых координат теперь делает SceneCamera —
    // см. GUI/Documents/scene.avalonia.md, раздел 6.
    public partial class BaseForm
    {
        public Point3D GetSceenCoord(double x, double y, double z) => sceneController.GetCamera().GetSceenCoord(x, y, z);

        /// <inheritdoc/>
        public Point3D GetSceenCoord(Point3D point) => sceneController.GetCamera().GetSceenCoord(point);

        /// <summary>Преобразует смещение в физических пикселях окна просмотра в вектор сцены.</summary>
        public Point3D GetSceenCoord(int deltaX, int deltaY, float depth, float scaleFactor) =>
            sceneController.GetCamera().GetSceenCoord(deltaX, deltaY, depth, scaleFactor);

        /// <inheritdoc/>
        /// <summary>Возвращает координаты точки сцены относительно центра окна просмотра.</summary>
        public ViewportPoint GetScreenCoord(Point3D coord) => sceneController.GetCamera().GetScreenCoord(coord);
    }
}
