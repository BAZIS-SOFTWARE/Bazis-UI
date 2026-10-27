using System;
using System.Drawing;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        private void navigator_DelCondEvent()
        {
			try
			{
                var node = navigator.SelectedNode;

                project.DeleteCond(node.Index);
            }
			catch (Exception ex)
			{
                console.PrintInfo(ex.Message, Color.Red);
			}


        }
    }
}
