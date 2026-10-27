using BazisGUI.SceneCore.Camera;
using BazisGUI.SceneCore.Interfaces;
using Geometry;
using BazisGUI.SceneCore.Coordinates;
using System.Drawing;

namespace BazisGUI.Win
{
    // Матрица вида и её преобразования теперь считает SceneCamera (sceneController.GetCamera()) —
    // см. GUI/Documents/scene.avalonia.md, раздел 6 и BazisGUI.Win.Scene.Core.Camera.SceneCamera.
    // Сигнатуры методов оставлены прежними, чтобы не трогать вызывающий код (Buttons/ViewToolStrip.cs и т.д.).
    public partial class BaseForm
    {
        public ModelPoint3D Position
        {
            get => sceneController.GetCamera().Position;
            set => sceneController.GetCamera().Position = value;
        }

        public void SetPositionBack() => ((SceneCamera)sceneController.GetCamera()).ResetPan();

        public void MoveCamera(Point new_mousePosition, Point mousePosition, float ScaleFactor) =>
            sceneController.GetCamera().Move(new_mousePosition, mousePosition, ScaleFactor);

        public void RotateCamera(ViewAxis axis, float angle) =>
            sceneController.GetCamera().Rotate(axis, angle);

        /// <inheritdoc/>
        public void RotateCamera(float vector_dx, float vector_dy, ViewAxis axis, float angle) =>
            sceneController.GetCamera().Rotate(vector_dx, vector_dy, axis, angle);

        /// <inheritdoc/>
        public void SetOnPlane(ViewPlane plane, float ScaleFactor) =>
            sceneController.GetCamera().SetOnPlane(plane, ScaleFactor);
    }
}
