using System;

namespace AvaloniaGUI.Args
{
    public class ChangeFunctionsEventArgs : EventArgs
    {
        public string[] Functions { get; }

        public ChangeFunctionsEventArgs(string[] Functions) { this.Functions = Functions; }
    }
}
