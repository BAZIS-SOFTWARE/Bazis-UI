using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinGUI.Args
{
    public class ChangeMaterialsEventArgs : EventArgs
    {
        public string[] Materials { get; }

        public ChangeMaterialsEventArgs(string[] materials) { Materials = materials; }
    }
}
