using Model.Interfaces;
using System;
using System.Collections.Generic;

namespace BazisGUI.Win.AdvanceSelection
{
    public class SelectInDirectionEventArgs : EventArgs 
    {
        public ObjType Objects { get; }

        public bool Reverse { get; set; }

        public float Angle { get; }

        //public int FirstNodeDirection { get; set; }

        //public int SecondNodeDirection { get; set; }

        public List<int> SelectedNumbers { get; protected set; } = new List<int>();

        public SelectInDirectionEventArgs(ObjType objects, bool reverse, float angle)
        {
            Objects = objects;
            Reverse = reverse;
            Angle = angle;
            //FirstNodeDirection = first;
            //SecondNodeDirection = second;
        }
    }
}