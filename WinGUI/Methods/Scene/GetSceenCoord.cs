using Geometry;

namespace WinGUI
{
    // Пересчёт экранных/сценовых координат теперь делает SceneCamera —
    // см. GUI/Documents/scene.avalonia.md, раздел 6.
    public partial class BaseForm
    {
        public Point3D GetSceenCoord(double x, double y, double z) => sceneController.GetCamera().GetSceenCoord(x, y, z);

        /// <inheritdoc/>
        public Point3D GetSceenCoord(Point3D point) => sceneController.GetCamera().GetSceenCoord(point);

        /// <inheritdoc/>
        public Point3D GetSceenCoord(Point2D point2D, float depth, float ScaleFactor) =>
            sceneController.GetCamera().GetSceenCoord(point2D, depth, ScaleFactor);

        /// <inheritdoc/>
        public Point2D GetScreenCoord(Point3D coord) => sceneController.GetCamera().GetScreenCoord(coord);
    }
}
