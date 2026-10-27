using System;
using System.Drawing;

namespace BazisGUI.Avalonia.Shell
{
    internal partial class MainWindow
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
