using System;

namespace BazisAvaloniaGUI.Args
{
    public class ChangeMaterialsEventArgs : EventArgs
    {
        public string[] Materials { get; }

        public ChangeMaterialsEventArgs(string[] materials) { Materials = materials; }
    }
}
