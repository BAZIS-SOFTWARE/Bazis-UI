using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazisGUI.Win.Args
{
    public class ChangeFunctionsEventArgs : EventArgs
    {
        public string[] Functions { get; }

        public ChangeFunctionsEventArgs(string[] Functions) { this.Functions = Functions; }
    }
}
