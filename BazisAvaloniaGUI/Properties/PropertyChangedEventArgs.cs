using System;

namespace BazisAvaloniaGUI.Properties
{
    public class PropertyChangedEventArgs : EventArgs
    {
        public string Key { get; }
        public string LocalizedHeader { get; set; }
        public string NewValue { get; set; }
        public string OldValue { get; }
        public string ObjInfo { get; internal set; }
        public int Tag { get; internal set; }

        public PropertyChangedEventArgs(string key, string header, string newValue, string oldValue)
        {
            Key = key;
            LocalizedHeader = header;
            NewValue = newValue;
            OldValue = oldValue;
        }
    }
}
