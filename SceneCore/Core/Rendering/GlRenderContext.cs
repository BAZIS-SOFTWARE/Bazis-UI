using SceneCore.Interfaces;

namespace SceneCore.Rendering
{
    public class GlRenderContext : IRenderContext
    {
        public ISceneCamera Camera { get; set; }
        public Camera.Viewport Viewport { get; set; }
        public SceneRenderSettings Settings { get; set; }
        public float ScaleFactor { get; set; }

        /// <summary>
        /// Буфер, переданный Avalonia в OnOpenGlRender — рисуем в него, а не в 0
        /// (см. scene.avalonia.md, таблицу отличий GLControl/SceneView).
        /// </summary>
        public int TargetFramebuffer { get; set; }
    }
}
