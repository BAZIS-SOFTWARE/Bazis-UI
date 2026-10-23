using SceneCore.Rendering;
using SceneCore;

namespace SceneCore.Layers
{
    /// <summary>Перенос выделения рамкой (selectionRectangle в BaseForm), см. RubberBandSelection.</summary>
    public class SelectionRectangleLayer : ISceneLayer
    {
        public string Name => "SelectionRectangle";
        public int Order { get; set; } = 70;
        public bool IsVisible { get; set; } = true;

        public ScreenRectangle Rectangle { get; } = new ScreenRectangle();

        public void Draw(IRenderContext context)
        {
            Rectangle.Display(context.Viewport.Width, context.Viewport.Height);
        }
    }
}
