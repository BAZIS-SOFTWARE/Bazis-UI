
namespace BazisGUI.SceneCore.Input
{
    public class SceneMouseEventArgs
    {
        public Coordinates.ScreenPoint Position { get; set; }
        public SceneMouseButton Button { get; set; }
        public SceneModifierKeys Modifiers { get; set; }
        public int Delta { get; set; }
    }
}
