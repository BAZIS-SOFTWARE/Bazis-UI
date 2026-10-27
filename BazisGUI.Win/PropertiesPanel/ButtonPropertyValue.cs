using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BazisGUI.Win.PropertiesPanel
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
