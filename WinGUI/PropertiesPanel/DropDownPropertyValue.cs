using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinGUI.PropertiesPanel
{
    public class DropDownPropertyValue
    {
        public object Value { get; set; }
        public List<string> AvailableValues { get; } = new List<string>();

        public DropDownPropertyValue(object value, List<string>  _availableValues)
        {
            AvailableValues = _availableValues;

            Value = value;
        }
    }
}
