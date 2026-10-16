using System;

namespace BazisAvaloniaGUI.Properties
{
    public class ButtonPropertyValue
    {
        public string Text { get; set; }
        public Action OnClick { get; set; }

        public ButtonPropertyValue(string text, Action onClick)
        {
            Text = text;
            OnClick = onClick;
        }
    }
}
