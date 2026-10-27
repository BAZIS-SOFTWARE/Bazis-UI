namespace BazisGUI.SceneCore.Input
{
    public enum SceneKey
    {
        None,
        Escape,
        C,
        F
    }

    public class SceneKeyEventArgs
    {
        public SceneKey Key { get; set; }
        public SceneModifierKeys Modifiers { get; set; }
    }
}
