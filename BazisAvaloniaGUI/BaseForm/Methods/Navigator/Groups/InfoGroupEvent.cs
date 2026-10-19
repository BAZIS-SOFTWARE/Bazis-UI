using System.Drawing;

namespace BazisAvaloniaGUI
{
    internal partial class BaseForm
    {
        private void navigator_InfoGroupEvent()
        {
            var group = project.GetModelGroup(navigator.SelectedNode.Index);
            console.PrintInfo(group.ToString(), Color.Black);
        }
    }
}
