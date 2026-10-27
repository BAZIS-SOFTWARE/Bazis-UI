using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazisGUI.MasterInterface
{
    public class WarningAttribute : Attribute
    {
        public string Message { get; private set; }
        public WarningAttribute(string message) 
        {
            Message = message;
        }
    }
}
