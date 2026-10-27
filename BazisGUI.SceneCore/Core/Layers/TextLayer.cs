using BazisGUI.SceneCore.Rendering;
using BazisGUI.SceneCore.Text;
using OpenTK.Graphics.OpenGL;

namespace BazisGUI.SceneCore.Layers
{
    /// <summary>Заменяет DisplayText3DEvent/DisplayText2DEvent.</summary>
    public class TextLayer : ISceneLayer
    {
        public string Name => "Text";
        public int Order { get; set; } = 55;
        public bool IsVisible { get; set; } = true;

        public TextLabelCollection Labels { get; } = new TextLabelCollection();

        private readonly ISceneTextRenderer textRenderer;

        public TextLayer(ISceneTextRenderer textRenderer)
        {
            this.textRenderer = textRenderer;
        }

        public void Draw(IRenderContext context)
        {
            foreach (var label in Labels)
            {
                if (label.IsScreenSpace)
                    textRenderer.DrawText2D(label, context);
                else
                {
                    // Как в старом DisplayText3DTemplate: подписи в координатах модели сдвигаются
                    // на центр вращения (GL.Translate(-Position)), как и сама модель
                    var position = context.Camera.Position;
                    GL.PushMatrix();
                    GL.Translate(-position.X, -position.Y, -position.Z);
                    textRenderer.DrawText3D(label, context);
                    GL.PopMatrix();
                }
            }
        }
    }
}
