using Model.Interfaces;
using System;
using System.Collections.Generic;

namespace BazisGUI.Win.AdvanceSelection
{
    public class SelectInPlainEventArgs : EventArgs
    {
        public ObjType Objects { get; }
        public float Angle { get; }

        public List<int> SelectedNumbers { get; set; } = new List<int>();
        public SelectInPlainEventArgs(ObjType objects, float angle)
        {
            Objects = objects;
            Angle = angle;
        }
    }
}