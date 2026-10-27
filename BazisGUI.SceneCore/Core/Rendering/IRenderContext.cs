using BazisGUI.SceneCore.Interfaces;

namespace BazisGUI.SceneCore.Rendering
{
    public interface IRenderContext
    {
        ISceneCamera Camera { get; }
        Camera.Viewport Viewport { get; }
        SceneRenderSettings Settings { get; }
        int TargetFramebuffer { get; }

        /// <summary>
        /// Текущий масштаб сцены (BaseForm.ScaleFactor). Не входит в ISceneCamera
        /// (переиспользуемый контракт), но нужен слоям для отрисовки виджетов
        /// постоянного экранного размера (базис, компас, точка вращения).
        /// </summary>
        float ScaleFactor { get; }
    }
}
