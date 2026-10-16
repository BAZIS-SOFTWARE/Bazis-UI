using System;
using System.Drawing;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private void navigator_HideSetEvent()
        {

            try
            {
                var node = navigator.SelectedNode;
                var objInfo = node.Text.Split(' ')[0];
                var setName = node.Text.Split(' ')[1];
                ChangeSetViewState(objInfo, setName, false);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }

        private void navigator_ShowSetEvent()
        {
            try
            {
                var node = navigator.SelectedNode;
                var objInfo = node.Text.Split(' ')[0];
                var setName = node.Text.Split(' ')[1];
                ChangeSetViewState(objInfo, setName, true);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }
    }
}
