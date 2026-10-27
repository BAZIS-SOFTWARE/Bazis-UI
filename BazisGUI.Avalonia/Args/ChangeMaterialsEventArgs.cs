using System;

namespace BazisGUI.Avalonia.Args
{
    public class ChangeMaterialsEventArgs : EventArgs
    {
        public string[] Materials { get; }

        public ChangeMaterialsEventArgs(string[] materials) { Materials = materials; }
    }
}
