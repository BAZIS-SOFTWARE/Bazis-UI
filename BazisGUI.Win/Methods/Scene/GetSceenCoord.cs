using Geometry;

using BazisGUI.SceneCore.Coordinates;

namespace BazisGUI.Win
{
    // Пересчёт экранных/сценовых координат теперь делает SceneCamera —
    // см. GUI/Documents/scene.avalonia.md, раздел 6.
    public partial class BaseForm
    {
        /// <summary>Преобразует точку модели в координаты камеры.</summary>
        public CameraPoint3D GetSceenCoord(double x, double y, double z)
        {
            var modelPoint = new ModelPoint3D(x, y, z);
            return sceneController.GetCamera().GetSceneCoord(modelPoint);
        }

        /// <summary>Преобразует точку модели в координаты камеры.</summary>
        public CameraPoint3D GetSceenCoord(Point3D point)
        {
            var modelPoint = new ModelPoint3D(point._x, point._y, point._z);
            return sceneController.GetCamera().GetSceneCoord(modelPoint);
        }

        /// <summary>Преобразует смещение в физических пикселях окна просмотра в вектор сцены.</summary>
        public ModelVector3D GetSceenCoord(int deltaX, int deltaY, float depth, float scaleFactor) =>
            sceneController.GetCamera().GetSceneVector(deltaX, deltaY, depth, scaleFactor);

        /// <inheritdoc/>
        /// <summary>Возвращает координаты точки сцены относительно центра окна просмотра.</summary>
        public ViewportPoint GetScreenCoord(CameraPoint3D coord) => sceneController.GetCamera().GetScreenCoord(coord);
    }
}
