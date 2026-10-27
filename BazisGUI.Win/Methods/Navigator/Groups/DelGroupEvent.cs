using System.Linq;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        private void navigator_DelGroupEvent()
        {
            var node = navigator.SelectedNode;
            var group = project.GetModelGroup(node.Index);

            project.DeleteModelGroup(group.Name);
        }
    }
}
