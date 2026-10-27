using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazisGUI.Win.Args
{
    public class PreparedConditionsEventArgs : EventArgs
    {
        public string[] ConditionStrings { get; }

        public PreparedConditionsEventArgs(string[] conditionStrings)
        {
            ConditionStrings = conditionStrings;
        }
    }
}
