using System;

namespace WinGUI.TasksControls
{
    internal interface ITaskControl
    {

        event Action<string> BtnSave_ClickEvent;

        bool GetValidationResult();
    }
}
