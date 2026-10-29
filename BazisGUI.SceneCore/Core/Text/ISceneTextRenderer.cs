using BazisGUI.SceneCore.Rendering;

namespace BazisGUI.SceneCore.Text
{
    public interface ISceneTextRenderer
    {
        void SetFont(string family, float size);
        void DrawText3D(TextLabel3D label, IRenderContext context);
        void DrawText2D(TextLabel2D label, IRenderContext context);
    }
}
