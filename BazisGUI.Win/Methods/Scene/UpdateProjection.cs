namespace BazisGUI.Win
{
    // Пересчёт матрицы проекции теперь делает SceneController.UpdateProjection() —
    // ортогональная/перспективная матрицы считаются в коде (CameraProjection),
    // см. GUI/Documents/scene.avalonia.md, раздел 6.
    public partial class BaseForm
    {
        public void UpdateProjection() => sceneController.UpdateProjection();
    }
}
