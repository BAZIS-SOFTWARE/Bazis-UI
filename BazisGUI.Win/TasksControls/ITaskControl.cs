using System;

namespace BazisGUI.Win.TasksControls
{
    internal interface ITaskControl
    {

        event Action<string> BtnSave_ClickEvent;

        bool GetValidationResult();
    }
}
