using System.Drawing;

namespace BazisAvaloniaGUI.Properties
{
    public class RowProperty
    {
        public string Key { get; internal set; }
        public string LocalizedHeader { get; internal set; } // Заголовок
        public override string ToString()
        {
            return $"{Key} {LocalizedHeader} {Value}";
        }
        public object Value { get; set; } // Значение

        public ValidationType ValidationType { get; set; } = ValidationType.None;

        public bool IsReadOnly { get; set; }
        public Color Color { get; internal set; } = SystemColors.Control;

        public RowProperty(string key, string localizedHeader, object value, bool isReadOnly = false)
        {
            Key = key;
            LocalizedHeader = localizedHeader;
            Value = value;
            IsReadOnly = isReadOnly;

            if (value is string)
                ValidationType = ValidationType.Text;
            else if (value is float)
                ValidationType = ValidationType.Float;
        }
    }
}
