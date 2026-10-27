namespace BazisGUI.Win
{
    // Масштаб сцены и его пересчёт теперь на стороне SceneCamera —
    // см. GUI/Documents/scene.avalonia.md, раздел 6.
    public partial class BaseForm
    {
        public float ScaleFactor => sceneController.ScaleFactor;

        public void ScaleObjs(float scaleFactor) => sceneController.ScaleObjs(scaleFactor);
    }
}
