using Geometry;

namespace BazisGUI.SceneCore.Input
{
    public class SceneMouseEventArgs
    {
        public Point2D Position { get; set; }
        public SceneMouseButton Button { get; set; }
        public SceneModifierKeys Modifiers { get; set; }
        public int Delta { get; set; }
    }
}
