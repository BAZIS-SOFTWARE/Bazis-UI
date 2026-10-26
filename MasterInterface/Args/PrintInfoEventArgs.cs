using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinGUI.Masters.Args
{
    public class PrintInfoEventArgs : EventArgs
    {
        public string Message { get; }
        public Color Color { get; }

        public PrintInfoEventArgs(string message, Color color)
        {
            Message = message;
            Color = color;
        }
    }
}
