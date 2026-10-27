using MathNet.Numerics.LinearAlgebra;

namespace BazisGUI.Win
{
    // Матрица вида больше не читается из стека GL_MODELVIEW — она хранится в SceneCamera
    // (sceneController.GetCamera()), см. GUI/Documents/scene.avalonia.md, раздел 6.
    public partial class BaseForm
    {
        public Matrix<float> ViewMatrix
        {
            get => sceneController.GetCamera().GetViewMatrix();
            set => sceneController.GetCamera().SetViewMatrix(value);
        }
    }
}
