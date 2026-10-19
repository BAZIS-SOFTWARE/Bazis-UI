using System.Drawing;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private void navigator_InfoGroupEvent()
        {
            var group = project.GetModelGroup(navigator.SelectedNode.Index);
            console.PrintInfo(group.ToString(), Color.Black);
        }
    }
}
