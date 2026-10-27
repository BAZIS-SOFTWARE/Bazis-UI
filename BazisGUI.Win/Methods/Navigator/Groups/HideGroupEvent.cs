using BazisGUI.Win.PropertiesPanel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazisGUI.Win
{
    public partial class BaseForm
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
