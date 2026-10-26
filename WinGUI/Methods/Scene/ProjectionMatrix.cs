using SceneCore.Camera;
using MathNet.Numerics.LinearAlgebra;

namespace WinGUI
{
    // Матрица проекции считается в коде (CameraProjection), а не читается из GL_PROJECTION_MATRIX —
    // см. GUI/Documents/scene.avalonia.md, раздел 6.
    public partial class BaseForm
    {
        public Matrix<float> ProjectionMatrix => ((SceneCamera)sceneController.GetCamera()).GetProjectionMatrix();
    }
}
