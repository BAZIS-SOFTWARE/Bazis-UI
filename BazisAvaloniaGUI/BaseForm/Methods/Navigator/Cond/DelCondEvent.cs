using System;
using System.Drawing;

namespace BazisAvaloniaGUI
{
    internal partial class BaseForm
    {
        private void navigator_DelCondEvent()
        {
            try
            {
                var node = navigator.SelectedNode;

                if (project.DeleteCond(node.Index))
                    navigator.SelectedNode.Remove();

                PresentCondDataOnTree();
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }


        }
    }
}
