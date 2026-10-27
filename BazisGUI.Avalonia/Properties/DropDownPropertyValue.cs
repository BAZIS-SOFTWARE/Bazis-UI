using System.Collections.Generic;

namespace BazisGUI.Avalonia.Properties
{
    public class DropDownPropertyValue
    {
        public object Value { get; set; }
        public List<string> AvailableValues { get; } = new List<string>();

        public DropDownPropertyValue(object value, List<string> _availableValues)
        {
            AvailableValues = _availableValues;

            Value = value;
        }
    }
}
