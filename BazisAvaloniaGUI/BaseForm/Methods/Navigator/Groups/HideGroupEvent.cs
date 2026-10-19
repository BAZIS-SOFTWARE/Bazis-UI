using System;
using System.Drawing;

namespace BazisAvaloniaGUI
{
    internal partial class BaseForm
    {
        private void navigator_HideGroupEvent()
        {
            try
            {
                var group = project.GetModelGroup(navigator.SelectedNode.Index);
                ChangeGroupViewState(group, false);

            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }
    }
}
