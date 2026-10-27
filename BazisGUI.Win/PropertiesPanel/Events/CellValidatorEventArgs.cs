using System;

namespace BazisGUI.Win.PropertiesPanel
{
    class CellValidatorEventArgs : EventArgs
    {
        public string Header { get; set; }
        public object NewValue { get; set; }
        public object OldValue { get; set; }

        public bool IsDataValid { get; set; }

        public CellValidatorEventArgs(string header, object newValue, object oldValue, bool isDataValid)
        {
            Header = header;
            NewValue = newValue;
            OldValue = oldValue;
            IsDataValid = isDataValid;
        }
    }
}
