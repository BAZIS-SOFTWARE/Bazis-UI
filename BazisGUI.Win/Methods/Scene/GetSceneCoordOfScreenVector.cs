using BazisGUI.SceneCore.Coordinates;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        /// <summary>Преобразует вектор в плоскости камеры в вектор координат модели.</summary>
        public ModelVector3D GetSceneCoordOfScreenVector(float x, float y) =>
            sceneController.GetCamera().GetSceneVectorOfScreenVector(x, y);
    }
}
