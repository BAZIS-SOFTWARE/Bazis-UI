using System;
using System.Drawing;

namespace BazisAvaloniaGUI
{
    internal partial class BaseForm
    {
        private void navigator_HideAllGroupsEvent()
        {
            try
            {
                foreach (var group in project.GetAllModelGroups())
                    ChangeGroupViewState(group, false);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }

        }


        private void navigator_ShowAllGroupsEvent()
        {
            try
            {
                foreach (var group in project.GetAllModelGroups())
                    ChangeGroupViewState(group, true);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }


        private void navigator_DelAllGroupsEvent()
        {
            try
            {
                project.ClearGroupData();
                project.ClearTaskData();

                PresentGroupDataOnTree();

                PresentCondDataOnTree();
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }

        }
    }
}
