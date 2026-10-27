
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazisGUI.SceneCore.EventsArgs
{
    public class InfoObjectsEventArgs : EventArgs
    {
        public string ObjsName { get; }

        IEnumerable<int> objsIndexes;
        public InfoObjectsEventArgs(string objsName, IEnumerable<int> objsIndexes)
        {
            ObjsName = objsName;
            this.objsIndexes = objsIndexes;
        }

        public int CountSelectedObjects { get { return objsIndexes.Count(); } }

        public IEnumerable<int> GetObjectsIndexes()
        {
            foreach (var obj in objsIndexes)
            {
                yield return obj;
            }
        }
    }
}
