using Geometry;

namespace WinGUI.Scene.EventsArgs
{
    public class SelectObjectsEventArgs
    {
        public RectangleBox SelectionBox { get; }
        public bool IsSorted { get; }

        public bool IsSelected { get; }

        public SelectObjectsEventArgs(RectangleBox selectionBox, bool isSorted,bool isSelected)
        {
            SelectionBox = selectionBox;
            IsSorted = isSorted;
            IsSelected = isSelected;
        }
    }
}
