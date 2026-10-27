using System;
using System.Collections.Generic;

namespace BazisGUI.Win.PropertiesPanel
{
    public class DrowPropertyOnPanelEventArgs : EventArgs
    {
        public IEnumerable<RowProperty> Properties;

        public DrowPropertyOnPanelEventArgs(IEnumerable<RowProperty> properties)
        {
            Properties = properties;
        }
    }
}
