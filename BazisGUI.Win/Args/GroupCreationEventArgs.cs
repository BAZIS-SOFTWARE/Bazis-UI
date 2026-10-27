using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazisGUI.Win.Args
{
    public class GroupCreationEventArgs : EventArgs
    {
        public ObjType Type { get; }
        public int Index { get; }
        public string Name { get; }

        public GroupCreationEventArgs(ObjType type, int index, string name)
        {
            Type = type;
            Index = index;
            Name = name;
        }
    }
}
