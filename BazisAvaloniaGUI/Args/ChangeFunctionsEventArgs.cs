using System;

namespace BazisAvaloniaGUI.Args
{
    public class ChangeFunctionsEventArgs : EventArgs
    {
        public string[] Functions { get; }

        public ChangeFunctionsEventArgs(string[] Functions) { this.Functions = Functions; }
    }
}
