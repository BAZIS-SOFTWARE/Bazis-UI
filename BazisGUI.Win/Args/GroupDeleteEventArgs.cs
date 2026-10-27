using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazisGUI.Win.Args
{
    public class GroupDeleteEventArgs : EventArgs
    {
        public ObjType Type { get; }
        public int Index { get; }

        public GroupDeleteEventArgs(ObjType type, int index)
        {
            Type = type;
            Index = index;
        }
    }
}
