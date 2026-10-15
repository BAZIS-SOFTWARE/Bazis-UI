using Model.Interfaces;
using System;
using System.Collections.Generic;

namespace BazisAvaloniaGUI.AdvanceSelection
{
    /// <summary>Выбор узлов по направлению, заданному двумя выбранными узлами.</summary>
    public class SelectInDirectionEventArgs : EventArgs
    {
        public ObjType Objects { get; }

        public bool Reverse { get; set; }

        public float Angle { get; set; }

        public List<int> SelectedNumbers { get; protected set; } = new List<int>();

        public SelectInDirectionEventArgs(ObjType objects, bool reverse, float angle)
        {
            Objects = objects;
            Reverse = reverse;
            Angle = angle;
        }
    }

    /// <summary>Выбор узлов или 2D-элементов в плоскости.</summary>
    public class SelectInPlainEventArgs : EventArgs
    {
        public ObjType Objects { get; }
        public float Angle { get; set; }

        public List<int> SelectedNumbers { get; set; } = new List<int>();

        public SelectInPlainEventArgs(ObjType objects, float angle)
        {
            Objects = objects;
            Angle = angle;
        }
    }
}
