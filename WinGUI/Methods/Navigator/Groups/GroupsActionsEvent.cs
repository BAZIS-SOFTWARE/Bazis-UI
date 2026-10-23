using System;
using System.Drawing;

namespace WinGUI
{
    public partial class BaseForm
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
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }

        }
    }
}
