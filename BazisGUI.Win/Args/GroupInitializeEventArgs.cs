using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazisGUI.Win.Args
{
    public class GroupInitializeEventArgs : EventArgs
    {
        public Dictionary<ObjType, Dictionary<int, string>> Groups { get; }

        public GroupInitializeEventArgs(Dictionary<ObjType, Dictionary<int, string>> groups)
        {
            Groups = groups;
        }
    }
}
