using BazisGUI.SceneCore.Rendering;

namespace BazisGUI.SceneCore.Text
{
    public interface ISceneTextRenderer
    {
        void SetFont(string family, float size);
        void DrawText3D(TextLabel label, IRenderContext context);
        void DrawText2D(TextLabel label, IRenderContext context);
    }
}
