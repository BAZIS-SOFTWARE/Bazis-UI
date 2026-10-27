using System;
using System.Drawing;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        private void navigator_ShowGroupEvent()
        {
            try
            {   
                var group = project.GetModelGroup(navigator.SelectedNode.Index);
                ChangeGroupViewState(group, true);

            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }
    }
}
