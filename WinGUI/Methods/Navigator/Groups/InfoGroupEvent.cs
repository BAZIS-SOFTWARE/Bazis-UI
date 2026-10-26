
using System.Drawing;

namespace WinGUI
{
    public partial class BaseForm
    {
        private void navigator_InfoGroupEvent()
        {          
            var group = project.GetModelGroup(navigator.SelectedNode.Index);
            console.PrintInfo(group.ToString(), Color.Black);
        }
    }
}
